using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DialogMasterData;

public enum HighlightTextType
{
    None,
    Bold,
    Color,
    BoldAndColor
}

/// <summary>
/// ダイアログを管理するクラス
/// </summary>
public class DialogManager : MonoBehaviour
{
    #region serialize field
    [Header("ダイアログリスト")]
    [SerializeField]
    DialogData[] m_data = default;

    [Header("テキストのスピード")]
    [SerializeField]
    float m_textSpeed = 1;

    [Header("自動再生モードで次に進むまでの時間")]
    [SerializeField]
    float m_autoflowTime = 1.5f;

    [SerializeField]
    string m_playerName = default;

    [Header("強調する文章を判定する用の文字")]
    [SerializeField]
    char m_triggerChar = '#';

    [Header("強調表現用の項目")]
    [SerializeField]
    HighlightTextType m_textType = HighlightTextType.None;

    [Header("強調文字の色")]
    [SerializeField]
    Color m_HighlightTextColor = default;

    [Header("パネルの各オブジェクト")]
    [SerializeField]
    GameObject m_display = default;

    [SerializeField]
    BackGroundController m_bgCtrl = default;

    [SerializeField]
    GameObject[] m_character = default;

    [SerializeField]
    Text m_characterName = default;

    [SerializeField]
    Text m_messageText = default;

    [SerializeField]
    GameObject m_clickIcon = default;

    [SerializeField]
    GameObject m_choicesPanel = default;

    [SerializeField]
    GameObject m_choicesPrefab = default;

    [SerializeField]
    GameObject m_logPanel = default;

    [SerializeField]
    Text m_logText = default;

    [SerializeField, Header("キャラクターリスト")]
    CharacterImageData[] m_imageDatas = default;
    #endregion

    #region public field
    public Action ContinueDialog = default;
    public Action EndDialog = default;
    #endregion

    #region field
    int m_nextMessageId = 0;
    int m_AfterReactionMessageId = 0;
    string m_htmlStartCode = default;
    string m_htmlEndCode = default;
    string m_tempLog = "";
    bool m_endMessage = false;
    bool isClicked = false;
    bool isAnimPlaying = false;
    bool isChoiced = false;
    bool isReactioned = false;
    IEnumerator m_currentCoroutine = default;
    Image[] m_characterImage;
    Animator[] m_anim;
    #endregion

    #region property
    public static DialogManager Instance { get; private set; }
    public bool IsAutoflow { get; set; }
    public int AfterReactionMessageId { get => m_AfterReactionMessageId; set => m_AfterReactionMessageId = value; }
    #endregion

    #region Unity function
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HighlightCodeSetup(m_textType, ColorToHex(m_HighlightTextColor));
        m_characterImage = new Image[m_character.Length];
        m_anim = new Animator[m_character.Length];

        for (int i = 0; i < m_character.Length; i++)
        {
            m_characterImage[i] = m_character[i].GetComponent<Image>();
            m_anim[i] = m_character[i].GetComponent<Animator>();
        }
        m_display.SetActive(false);
        StartCoroutine(StartMessage());
    }
    #endregion

    #region coroutine
    /// <summary>
    /// メッセージを表示する
    /// </summary>
    /// <returns></returns>
    IEnumerator StartMessage()
    {
        for (int i = 0; i < m_data.Length; i++)
        {
            BackGroundController.BackgroundAnim += FinishReceive;

            if (i == 0)
            {
                m_bgCtrl.Setup(m_data[i].BackgroundType); //最初の背景をセットする
                m_bgCtrl.FadeIn(m_data[i].BackgroundType); //フェードイン
                isAnimPlaying = true;
            }
            else
            {
                m_bgCtrl.Crossfade(m_data[i].BackgroundType); //次のダイアログの背景にクロスフェードする
                isAnimPlaying = true;
            }

            //Animationが終わるまで待つ
            while (isAnimPlaying)
            {
                yield return null;
            }
            BackGroundController.BackgroundAnim -= FinishReceive;

            m_currentCoroutine = DisplayMessage(m_data[i]);
            yield return m_currentCoroutine;
        }
        //全てのダイアログが終了したらこの下の処理が行われる
        m_display.SetActive(false);
        OnEndDialog();
    }

    /// <summary>
    /// ダイアログを表示する
    /// </summary>
    /// <param name="data"> ダイアログデータ </param>
    /// <returns></returns>
    IEnumerator DisplayMessage(DialogData data)
    {
        m_choicesPanel.SetActive(false);
        m_display.SetActive(false);
        int currentDialogIndex = 0;

        while (currentDialogIndex < data.CharacterData.Length)
        {
            //ダイアログをリセット
            m_endMessage = false;
            isClicked = false;

            //キャラクターのアニメーションが終わるまで待つ
            yield return WaitForCharaAnimation(data.CharacterData[currentDialogIndex].Talker,
                                               data.CharacterData[currentDialogIndex].Position,
                                               data.CharacterData[currentDialogIndex].StartAnimationType);

            m_display.SetActive(true);
            OnContinueDialog();
            m_characterName.text = data.CharacterData[currentDialogIndex].Talker.Replace("プレイヤー", m_playerName);
            if (!isReactioned)
            {
                m_tempLog += m_characterName.text + "：";
            }
            EmphasisCharacter(data.CharacterData[currentDialogIndex].Position); //アクティブなキャラ以外を暗転する

            //各キャラクターの全てのメッセージを順に表示する
            for (int i = 0; i < data.CharacterData[currentDialogIndex].AllMessages.Length; i++)
            {
                //キャラクターの表情の変更があればここで変更
                if (m_characterImage[data.CharacterData[currentDialogIndex].Position].enabled)
                {
                    m_characterImage[data.CharacterData[currentDialogIndex].Position].sprite = SetCharaImage(data.CharacterData[currentDialogIndex].Talker, data.CharacterData[currentDialogIndex].FaceTypes[i]);
                }

                //表示されたメッセージをリセット
                m_clickIcon.SetActive(false);
                m_messageText.text = "";
                string message = data.CharacterData[currentDialogIndex].AllMessages[i].Replace("プレイヤー", m_playerName);
                bool isHighlighted = false;

                //各メッセージを一文字ずつ表示する
                foreach (var m in message)
                {
                    if (m == m_triggerChar) //強調表現用の文字だった場合
                    {
                        if (!isHighlighted) 
                        {
                            isHighlighted = true; //強調開始
                        }
                        else
                        {
                            isHighlighted = false; //強調終了
                        }
                        continue;
                    }
                    else
                    {
                        if (!isHighlighted)
                        {
                            //普通に文字を出力
                            m_messageText.text += m;
                        }
                        else
                        {
                            //文字を強調するためのHTMLコードと一緒にまとめて出力
                            m_messageText.text += m_htmlStartCode + m + m_htmlEndCode;
                        }
                    }
                    yield return WaitTimer(m_textSpeed); //次の文字を表示するのを設定した時間待つ

                    //表示中にクリックされたら現在のメッセージを全て表示して処理を中断する
                    if (isClicked) 
                    {
                        m_messageText.text = HighlightKeyword(message);
                        break;
                    }
                    yield return null;
                }

                m_endMessage = true;

                //自動再生モードがOFFならクリックアイコンを表示
                if (!IsAutoflow)
                {
                    m_clickIcon.SetActive(true);
                }

                yield return null;

                if (data.CharacterData[currentDialogIndex].ChoicesId != 0) //選択肢がある場合
                {
                    m_choicesPanel.SetActive(true);

                    for (int k = 0; k < data.ChoicesDatas.Length; k++)
                    {
                        if (data.ChoicesDatas[k].ChoicesId == data.CharacterData[currentDialogIndex].ChoicesId) //IDが一致したら
                        {
                            CreateChoices(data.CharacterData, data.ChoicesDatas[k], data.ChoicesDatas[k].NextId); //選択肢を生成
                            break;
                        }
                    }
                    yield return new WaitUntil(() => isChoiced); //ボタンが押されるまで待つ

                    m_choicesPanel.SetActive(false); //選択肢画面を非表示にする

                    //選択肢を選択した直後だったら
                    if (isChoiced && !isReactioned)
                    {
                        currentDialogIndex = m_nextMessageId; //選択した項目に対応したメッセージに次に表示する
                        isChoiced = false;
                        isReactioned = true;
                    }
                }
                else
                {
                    float timer = default;

                    while (true)
                    {
                        //自動再生モードがONだったら
                        if (IsAutoflow)
                        {
                            if (m_clickIcon.activeSelf)
                            {
                                m_clickIcon.SetActive(false);
                            }
                            timer += Time.deltaTime;

                            if (timer >= m_autoflowTime)
                            {
                                m_endMessage = false;
                                break;
                            }
                        }
                        if (m_endMessage && IsInputed()) //テキストを全て表示した状態でクリックされたら
                        {
                            m_endMessage = false;
                            break;
                        }
                        yield return null;
                    }

                    if (i < data.CharacterData[currentDialogIndex].AllMessages.Length - 1)
                    {
                        m_tempLog += HighlightKeyword(message) + "\n" + new string('　', m_characterName.text.Length) + "　";
                    }
                    else
                    {
                        m_tempLog += HighlightKeyword(message) + "\n";
                    }
                }
                yield return null;
            }
            //キャラクターのアニメーションが終わるまで待つ
            yield return WaitForCharaAnimation(data.CharacterData[currentDialogIndex].Talker,
                                               data.CharacterData[currentDialogIndex].Position,
                                               data.CharacterData[currentDialogIndex].EndAnimationType);

            //選択肢に対応したメッセージが表示済みだったら
            if (isReactioned)
            {
                currentDialogIndex = m_AfterReactionMessageId;
                isReactioned = false;
            }
            else
            {
                currentDialogIndex = data.CharacterData[currentDialogIndex].NextId;
            }
            yield return null;
        }
        #region FinishDialog
        //ダイアログの内容が全て終了したら表示中のキャラクターをフェードアウトさせ、フェードが終了するまで待つ。
        yield return WaitForFinishDialogFadeOut();
        #endregion
    }

    /// <summary>
    /// アニメーションの再生が終了するまで待機する
    /// </summary>
    /// <param name="charaName"></param>
    /// <param name="positionIndex"></param>
    /// <param name="animation"></param>
    /// <returns></returns>
    IEnumerator WaitForCharaAnimation(string charaName, int positionIndex, string animation)
    {
        if (!m_characterImage[positionIndex].enabled)
        {
            m_characterImage[positionIndex].enabled = true;
        }

        m_characterImage[positionIndex].sprite = SetCharaImage(charaName);

        if (animation != null && animation != "なし") //アニメーションの指定があれば
        {
            AnimationAccordingType(animation, positionIndex);
            isAnimPlaying = true;
            CharacterPanel.CharacterAnim += FinishReceive;
        }
        else
        {
            yield break;
        }

        while (isAnimPlaying) //アニメーションが終わるまで待つ
        {
            if (IsInputed())
            {
                m_anim[positionIndex].Play("Idle");
                isAnimPlaying = false;
            }
            yield return null;
        }
        CharacterPanel.CharacterAnim -= FinishReceive;
    }

    /// <summary>
    /// 全てのダイアログが終了したら全てのキャラクターをフェードアウトさせる
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForFinishDialogFadeOut()
    {
        for (int i = 0; i < m_characterImage.Length; i++)
        {
            if (m_characterImage[i].enabled)
            {
                m_characterImage[i].color = new Color(1, 1, 1);
                m_anim[i].Play("FadeOut");
                isAnimPlaying = true;
                CharacterPanel.CharacterAnim += FinishReceive;
            }
        }
        m_display.SetActive(false);

        while (isAnimPlaying) //アニメーションが終わるまで待つ
        {
            yield return null;
        }
        CharacterPanel.CharacterAnim -= FinishReceive;
    }

    /// <summary>
    /// 指定した時間待機する
    /// </summary>
    /// <param name="time"> 待つ時間 </param>
    /// <returns></returns>
    IEnumerator WaitTimer(float time)
    {
        float timer = 0;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer >= time)
            {
                yield break;
            }
            else if (IsInputed())
            {
                isClicked = true;
                yield break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// スキップ判定
    /// </summary>
    /// <returns></returns>
    IEnumerator SkipDecision(int currentId)
    {
        for (int i = currentId; i < m_character.Length; i++)
        {

        }
        yield break;
    }
    #endregion

    #region public function
    /// <summary>
    /// 会話ログを表示する
    /// </summary>
    public void OpenLog()
    {
        m_logPanel.SetActive(true);
        m_logText.text = m_tempLog;
        //クリックアイコンが点滅していたら目立つので非表示にする
        if (m_clickIcon.activeSelf)
        {
            m_clickIcon.SetActive(false);
        }

        if (!m_choicesPanel.activeSelf)
        {
            Time.timeScale = 0f; //再生中のコルーチンを一時停止
        }
    }

    /// <summary>
    /// 会話ログを閉じる
    /// </summary>
    public void CloseLog()
    {
        m_logPanel.SetActive(false);

        if (m_endMessage)
        {
            m_clickIcon.SetActive(true);
        }

        if (!m_choicesPanel.activeSelf)
        {
            Time.timeScale = 1f; //コルーチン再開
        }
    }

    /// <summary>
    /// 次に表示するメッセージを切り替える
    /// </summary>
    /// <param name="nextId"> 次に表示するメッセージのID </param>
    public void SwitchIndex(int nextId)
    {
        m_nextMessageId = nextId;
    }
    #endregion

    #region private function
    /// <summary>
    /// 入力されたデータに応じてアニメーションを再生する。アニメーションの追加を行う場合はここに記述する
    /// </summary>
    /// <param name="animation"> アニメーション </param>
    /// <param name="index"> 現在アクティブのキャラクターの番号 </param>
    void AnimationAccordingType(string animation, int index)
    {
        switch (animation)
        {
            case "FadeIn":
                m_anim[index].Play("FadeIn");
                break;
            case "FadeOut":
                m_anim[index].Play("FadeOut");
                break;
            case "AllFadeIn":
                for (int i = 0; i < m_characterImage.Length; i++)
                {
                    if (!m_characterImage[i].enabled)
                    {
                        m_characterImage[i].enabled = true;
                    }
                    m_anim[i].Play("FadeIn");
                }
                break;
            case "AllFadeOut":
                for (int i = 0; i < m_characterImage.Length; i++)
                {
                    if (!m_characterImage[i].enabled)
                    {
                        m_characterImage[i].enabled = true;
                    }
                    m_anim[i].Play("FadeOut");
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// アクティブなキャラクター以外を暗転させる
    /// </summary>
    /// <param name="currentIndex"></param>
    void EmphasisCharacter(int currentIndex)
    {
        for (int i = 0; i < m_characterImage.Length; i++)
        {
            if (m_characterImage[i].enabled)
            {
                if (i == currentIndex)
                {
                    m_characterImage[i].color = new Color(1, 1, 1); //アクティブにする
                }
                else
                {
                    m_characterImage[i].color = new Color(0.5f, 0.5f, 0.5f); //非アクティブにする
                }
            }
        }
    }

    /// <summary>
    /// 選択肢を生成する
    /// </summary>
    /// <param name="characterDatas"> キャラクターのデータ </param>
    /// <param name="data"> 選択肢のデータ </param>
    /// <param name="id"> 選択肢を押した後に表示するメッセージのID </param>
    void CreateChoices(CharacterData[] characterDatas, ChoicesData data, int[] id)
    {
        //選択肢の数だけボタンを生成
        for (int i = 0; i < data.AllChoices.Length; i++)
        {
            var c = Instantiate(m_choicesPrefab, m_choicesPanel.transform);
            var choiceButton = c.GetComponent<ChoicesButton>();
            choiceButton.NextMessageId = id[i];

            //各ボタンに選択された項目に応じたメッセージのIDをセット
            for (int n = 0; n < characterDatas.Length; n++)
            {
                if (characterDatas[n].MessageId == id[i])
                {
                    choiceButton.AfterReactionMessageId = id[i];
                }
            }

            var b = c.GetComponent<Button>();

            //ボタンがクリックされた時の機能をセットする
            b.onClick.AddListener(() =>
            {
                //クリックフラグをON
                isChoiced = true;

                //生成した選択肢を全て消去
                foreach (Transform child in m_choicesPanel.transform)
                {
                    Destroy(child.gameObject);
                }
            });

            //選択肢の項目名をボタンオブジェクトの下にあるテキストに代入
            var t = c.GetComponentInChildren<Text>();
            t.text = data.AllChoices[i];
        }
    }

    /// <summary>
    /// アニメーションの終了コールバック
    /// </summary>
    void FinishReceive()
    {
        isAnimPlaying = false;
    }

    /// <summary>
    /// 次のダイアログが再生される前に実行されるイベント
    /// </summary>
    void OnContinueDialog()
    {
        ContinueDialog?.Invoke();
    }

    /// <summary>
    /// 全てのダイアログが終了したら実行されるイベント
    /// </summary>
    void OnEndDialog()
    {
        EndDialog?.Invoke();
    }

    /// <summary>
    /// 強調文字のコードのセットアップ
    /// </summary>
    /// <param name="textType"> 強調の種類 </param>
    /// <param name="hex"> 16進数表記の色 </param>
    void HighlightCodeSetup(HighlightTextType textType, string hex)
    {
        switch (textType)
        {
            //変更なし
            case HighlightTextType.None:
                m_htmlStartCode = "";
                m_htmlEndCode = "";
                break;
            //太字のみ
            case HighlightTextType.Bold:
                m_htmlStartCode = "<b>";
                m_htmlEndCode = "</b>";
                break;
            //色変更のみ
            case HighlightTextType.Color:
                m_htmlStartCode = $"<color=#{hex}>";
                m_htmlEndCode = $"</color>";
                break;
            //太字と色変更
            case HighlightTextType.BoldAndColor:
                m_htmlStartCode = $"<b><color=#{hex}>";
                m_htmlEndCode = $"</color></b>";
                break;
        }
    }

    /// <summary>
    /// 入力判定を行う
    /// </summary>
    /// <returns> 入力判定 </returns>
    bool IsInputed()
    {
        //会話ログが表示されていたらfalseを返す
        if (Time.timeScale == 0)
        {
            return false;
        }

        //左クリック、Spaceキー、Enterキーのいずれかが押されたらtrueを返す
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// キャラクターのImageをセットする
    /// </summary>
    /// <param name="charaName"> キャラクター名 </param>
    /// <param name="faceType"> 表情のタイプ </param>
    /// <returns></returns>
    Sprite SetCharaImage(string charaName, int faceType = 0)
    {
        Sprite chara = default;

        for (int i = 0; i < m_imageDatas.Length; i++)
        {
            if (charaName == m_imageDatas[i].CharacterName)
            {
                chara = m_imageDatas[i].CharacterImages[faceType];
                break;
            }
        }
        return chara;
    }

    /// <summary>
    /// メッセージの中で重要となるキーワードを強調する
    /// </summary>
    /// <param name="message"> メッセージ全文 </param>
    /// <returns> 修正後のメッセージ </returns>
    string HighlightKeyword(string message)
    {
        bool isHighlight = false;
        string fixMessage = default;

        foreach (var m in message)
        {
            if (m == m_triggerChar)
            {
                if (!isHighlight)
                {
                    isHighlight = true;
                    continue;
                }
                else
                {
                    isHighlight = false;
                    continue;
                }
            }
            else
            {
                if (!isHighlight)
                {
                    fixMessage += m;
                }
                else
                {
                    fixMessage += m_htmlStartCode + m + m_htmlEndCode;
                }
            }
        }
        return fixMessage;
    }

    /// <summary>
    /// Colorを16進数に変換する
    /// </summary>
    /// <param name="color"> 色 </param>
    /// <returns></returns>
    string ColorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }
    #endregion
}
