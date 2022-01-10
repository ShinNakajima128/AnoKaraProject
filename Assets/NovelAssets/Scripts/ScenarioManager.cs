using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScenarioMasterData;
using UnityEngine.Events;
using DG.Tweening;

public enum HighlightTextType
{
    None,
    Bold,
    Color,
    BoldAndColor
}

public enum FeelingType
{
    Happy,
    Angry,
    Cry,
    raku,
    Surprise,
    Think,
    Correct,
    Incorrect
}

/// <summary>
/// シナリオを管理するクラス
/// </summary>
public class ScenarioManager : MonoBehaviour
{
    #region serialize field
    [Header("デバッグ作業用")]
    [SerializeField]
    bool isDebug = default;

    [Header("ダイアログリスト")]
    [SerializeField]
    ScenarioData[] m_data = default;

    [Header("テキストのスピード")]
    [SerializeField]
    float m_textSpeed = 1;

    [Header("自動再生モードで次に進むまでの時間")]
    [SerializeField]
    float m_autoflowTime = 1.5f;

    [Header("プレイヤー名確認用")]
    [SerializeField]
    string m_playerName = default;

    [Header("プレイヤーが男の子の場合の一人称")]
    [SerializeField]
    string m_malePronoun = default;

    [Header("プレイヤーが女の子の場合の一人称")]
    [SerializeField]
    string m_famalePronoun = default;

    [Header("強調する文章を判定する用の文字")]
    [SerializeField]
    char m_triggerChar = '#';

    [Header("強調表現用の項目")]
    [SerializeField]
    HighlightTextType m_textType = HighlightTextType.None;

    [Header("強調文字の色")]
    [SerializeField]
    Color m_HighlightTextColor = default;

    [Header("探索パート用の仕様に変更するフラグ")]
    [SerializeField]
    bool m_isSearchPart = false;

    [Header("パネルの各オブジェクト")]
    [SerializeField]
    GameObject m_display = default;

    /// <summary> 背景の機能を持つクラス </summary>
    [SerializeField]
    BackGroundController m_bgCtrl = default;

    /// <summary> キャラクターを表示するオブジェクト </summary>
    [SerializeField]
    GameObject[] m_character = default;

    /// <summary> 名前を表示するパネル </summary>
    [SerializeField]
    GameObject m_namePanel = default;

    /// <summary> キャラクター名が入るテキスト </summary>
    [SerializeField]
    Text m_characterName = default;

    /// <summary> セリフのテキスト </summary>
    [SerializeField]
    Text m_messageText = default;

    /// <summary> クリック待機時に表示するアイコン </summary>
    [SerializeField]
    GameObject m_clickIcon = default;

    /// <summary> 選択肢が表示されるパネル </summary>
    [SerializeField]
    GameObject m_choicesPanel = default;

    /// <summary> 選択肢生成用オブジェクト </summary>
    [SerializeField]
    GameObject m_choicesPrefab = default;

    /// <summary> 会話ログのパネル </summary>
    [SerializeField]
    GameObject m_logPanel = default;

    /// <summary> 会話ログのテキスト </summary>
    [SerializeField]
    Text m_logText = default;

    /// <summary> 設定ボタンのクラス </summary>
    [SerializeField]
    SettingButton m_settingButton = default;

    [Header("キャラクターリスト")]
    [SerializeField]
    CharacterImageData[] m_imageDatas = default;

    [Header("プレイヤーの性別毎の各表情Sprite")]
    [SerializeField]
    Sprite[] m_boySprites = default;

    [SerializeField]
    Sprite[] m_girlSprites = default;

    [Header("感情用のエフェクト")]
    [SerializeField]
    Sprite[] m_feelingEffects = default;

    [Header("画像差し替え用の透明画像")]
    [SerializeField]
    Sprite m_transparentSprite = default;

    [Header("エフェクトを表示するポジション")]
    [SerializeField]
    Image[] m_effectPositions = default;
    #endregion

    #region public field
    [Header("Sceneが始まった時に呼び出されるイベント")]
    public UnityEvent StartEvent = new UnityEvent();
    [Header("シナリオデータを継続する際に呼び出されるイベント")]
    public UnityEvent ContinueEvent = new UnityEvent();
    [Header("シナリオデータが全て終了した時に呼び出されるイベント")]
    public UnityEvent EndEvent = new UnityEvent();
    #endregion

    #region field
    int m_nextMessageId = 0;
    int m_AfterReactionMessageId = 0;
    int m_currentBackgroundType = default;
    string m_htmlStartCode = default;
    string m_htmlEndCode = default;
    string m_tempLog = "";
    bool m_endMessage = false;
    bool isClicked = false;
    bool isAnimPlaying = false;
    bool isChoiced = false;
    bool isReactioned = false;
    bool m_clickReception = false;
    bool isFirst = true;
    bool set1 = false;
    bool set2 = false;
    bool set3 = false;
    IEnumerator m_currentCoroutine = default;
    Image[] m_characterImage;
    Animator[] m_anim;
    #endregion

    #region property
    public static ScenarioManager Instance { get; private set; }

    public ScenarioData[] Data => m_data;
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
        m_playerName = DataManager.Instance.PlayerData.PlayerName;
        m_imageDatas[0].CharacterImages = DataManager.Instance.PlayerData.PlayerGender == GenderType.Boy ? m_boySprites : m_girlSprites;
        HighlightCodeSetup(m_textType, ColorToHex(m_HighlightTextColor));
        m_characterImage = new Image[m_character.Length];
        m_anim = new Animator[m_character.Length];

        for (int i = 0; i < m_character.Length; i++)
        {
            m_characterImage[i] = m_character[i].GetComponent<Image>();
            m_anim[i] = m_character[i].GetComponent<Animator>();
        }
        m_display.SetActive(false);
        StartEvent?.Invoke();
    }
    #endregion

    #region coroutine
    /// <summary>
    /// メッセージを表示する
    /// </summary>
    /// <returns></returns>
    IEnumerator StartAllMessage()
    {
        for (int i = 0; i < m_data.Length; i++)
        {
            m_currentCoroutine = DisplayMessage(m_data[i]);
            yield return m_currentCoroutine;
        }
        //全てのダイアログが終了したらこの下の処理が行われる
        m_display.SetActive(false);
        OnEndDialog();
    }

    /// <summary>
    /// 選択したメッセージを開始する
    /// </summary>
    /// <param name="index"> データの添え字番号 </param>
    /// <returns></returns>
    IEnumerator StartSelectMessage(int index)
    {
        m_currentCoroutine = DisplayMessage(m_data[index]);
        yield return m_currentCoroutine;

        //全てのダイアログが終了したらこの下の処理が行われる
        Debug.Log("会話終了");
        m_display.SetActive(false);
    }

    /// <summary>
    /// ダイアログを表示する
    /// </summary>
    /// <param name="data"> ダイアログデータ </param>
    /// <returns></returns>
    IEnumerator DisplayMessage(ScenarioData data)
    {
        m_choicesPanel.SetActive(false);
        m_display.SetActive(false);
        int currentDialogIndex = 0;
        m_nextMessageId = 0;
        int setCount = 0;

        for (int i = 0; i < data.DialogData.Length; i++)
        {
            if (data.DialogData[i].AllPosition[0] == 0 && !set1)
            {
                m_characterImage[data.DialogData[i].AllPosition[0]].sprite = SetCharaImage(data.DialogData[i].AllTalker[0]);
                set1 = true;
                setCount++;
            }
            else if (data.DialogData[i].AllPosition[0] == 1 && !set2)
            {
                m_characterImage[data.DialogData[i].AllPosition[0]].sprite = SetCharaImage(data.DialogData[i].AllTalker[0]);
                set2 = true;
                setCount++;
            }
            else if (data.DialogData[i].AllPosition[0] == 2 && !set3)
            {
                m_characterImage[data.DialogData[i].AllPosition[0]].sprite = SetCharaImage(data.DialogData[i].AllTalker[0]);
                set3 = true;
                setCount++;
            }

            if (setCount >= 3)
            {
                break;
            }
        }

        while (currentDialogIndex < data.DialogData.Length)
        {
            //ダイアログをリセット
            m_endMessage = false;
            isClicked = false;
            if (currentDialogIndex == 0 && !m_isSearchPart)
            {
                BackGroundController.BackgroundAnim += FinishReceive;

                m_bgCtrl.Setup(data.DialogData[currentDialogIndex].BackgroundType); //最初の背景をセットする
                m_bgCtrl.FadeIn(data.DialogData[currentDialogIndex].BackgroundType); //フェードイン
                m_currentBackgroundType = data.DialogData[currentDialogIndex].BackgroundType;
                isAnimPlaying = true;

                //Animationが終わるまで待つ
                while (isAnimPlaying)
                {
                    yield return null;
                }
                BackGroundController.BackgroundAnim -= FinishReceive;
            }
            else if (m_currentBackgroundType != data.DialogData[currentDialogIndex].BackgroundType)
            {
                m_settingButton.IsActived = false;
                m_display.SetActive(false);
                BackGroundController.BackgroundAnim += FinishReceive;
                m_bgCtrl.Crossfade(data.DialogData[currentDialogIndex].BackgroundType); //次の背景にクロスフェードする
                m_currentBackgroundType = data.DialogData[currentDialogIndex].BackgroundType;
                isAnimPlaying = true;

                //Animationが終わるまで待つ
                while (isAnimPlaying)
                {
                    yield return null;
                }
                BackGroundController.BackgroundAnim -= FinishReceive;
            }
            //デバッグの場合はデバッグ用のイベントを実行する
            if (isDebug)
            {
                EventManager.OnEvent(Events.Debug);
            }
            //キャラクターのアニメーションが終わるまで待つ
            yield return WaitForCharaAnimation(data.DialogData[currentDialogIndex].AllTalker,
                                               data.DialogData[currentDialogIndex].AllPosition,
                                               data.DialogData[currentDialogIndex].StartAnimationType);

            m_display.SetActive(true);
            OnContinueDialog();
            m_characterName.text = data.DialogData[currentDialogIndex].Talker.Replace("プレイヤー", m_playerName);

            if (m_characterName.text == "ナレーター")
            {
                m_namePanel.SetActive(false);
            }
            else
            {
                m_namePanel.SetActive(true);
            }

            if (!isReactioned)
            {
                m_tempLog += m_characterName.text + "：";
            }
            EmphasisCharacter(data.DialogData[currentDialogIndex].AllPosition); //アクティブなキャラ以外を暗転する

            //各キャラクターの全てのメッセージを順に表示する
            for (int i = 0; i < data.DialogData[currentDialogIndex].AllMessages.Length; i++)
            {
                if (data.DialogData[currentDialogIndex].AllMessages[i] == "")
                {
                    m_display.SetActive(false);
                    break;
                }

                //キャラクターの表情の変更があればここで変更
                if (m_characterName.text != "ナレーター")
                {
                    for (int n = 0; n < data.DialogData[currentDialogIndex].AllPosition.Length; n++)
                    {
                        if (m_characterImage[data.DialogData[currentDialogIndex].AllPosition[n]].enabled)
                        {
                            m_characterImage[data.DialogData[currentDialogIndex].AllPosition[n]].sprite = SetCharaImage(data.DialogData[currentDialogIndex].Talker, data.DialogData[currentDialogIndex].FaceTypes[i]);
                            SetFeelingAnim(m_effectPositions[data.DialogData[currentDialogIndex].AllPosition[n]], data.DialogData[currentDialogIndex].FaceTypes[i]);
                        }
                    }
                }

                //表示されたメッセージをリセット
                m_clickIcon.SetActive(false);
                m_messageText.text = "";
                string message = data.DialogData[currentDialogIndex].AllMessages[i].Replace("プレイヤー", m_playerName)
                                                                                   .Replace("私（僕or俺)", DataManager.Instance.PlayerData.PlayerGender == GenderType.Boy ? m_malePronoun : m_famalePronoun);
                bool isHighlighted = false;
                m_clickReception = false;

                //メッセージにボイスが設定されていたら
                if (data.DialogData[currentDialogIndex].AllVoiceId[i] != "なし")
                {
                    //各ボイスに合わせたSourceで音声を再生
                    if (m_characterName.text == "コマ")　//コマの音声を再生
                    {
                        FixPlayVoice(VoiceType.Koma, data.DialogData[currentDialogIndex].AllVoiceId[i]);
                    }
                    else if (m_characterName.text == m_playerName)　//プレイヤーの音声を再生
                    {
                        FixPlayVoice(VoiceType.Player, data.DialogData[currentDialogIndex].AllVoiceId[i]);

                    }
                    else　//ナレーターやモブが話している場合
                    {
                        if (data.DialogData[currentDialogIndex].AllVoiceId[i].Contains("player"))
                        {
                            FixPlayVoice(VoiceType.Player, data.DialogData[currentDialogIndex].AllVoiceId[i]);
                        }
                        else if (data.DialogData[currentDialogIndex].AllVoiceId[i].Contains("koma"))
                        {
                            FixPlayVoice(VoiceType.Koma, data.DialogData[currentDialogIndex].AllVoiceId[i]);
                        }
                    }
                }

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

                if (data.DialogData[currentDialogIndex].ChoicesId != 0) //選択肢がある場合
                {
                    m_choicesPanel.SetActive(true);

                    for (int k = 0; k < data.ChoicesDatas.Length; k++)
                    {
                        if (data.ChoicesDatas[k].ChoicesId == data.DialogData[currentDialogIndex].ChoicesId) //IDが一致したら
                        {
                            CreateChoices(data.DialogData, data.ChoicesDatas[k], data.ChoicesDatas[k].NextId); //選択肢を生成
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

                    if (i < data.DialogData[currentDialogIndex].AllMessages.Length - 1)
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
            yield return WaitForCharaAnimation(data.DialogData[currentDialogIndex].AllTalker,
                                               data.DialogData[currentDialogIndex].AllPosition,
                                               data.DialogData[currentDialogIndex].EndAnimationType, data.DialogData[currentDialogIndex].FaceTypes[data.DialogData[currentDialogIndex].FaceTypes.Length - 1]);

            //選択肢に対応したメッセージが表示済みだったら
            if (isReactioned)
            {
                currentDialogIndex = m_AfterReactionMessageId;
                isReactioned = false;
            }
            else
            {
                //NextIdが100だったら、クイズUIを表示し、そのステージのクイズを開始する
                if (data.DialogData[currentDialogIndex].NextId == 100)
                {
                    Debug.Log("クイズ開始");
                    LoadSceneManager.FadeOutPanel();
                    //yield return クイズ開始のコルーチン;
                    currentDialogIndex++;
                }
                else
                {
                    currentDialogIndex = data.DialogData[currentDialogIndex].NextId;
                }
            }
            yield return null;
        }
        #region FinishDialog
        //ダイアログの内容が全て終了したら表示中のキャラクターをフェードアウトさせ、フェードが終了するまで待つ。
        yield return WaitForFinishDialogFadeOut();
        m_settingButton.IsActived = false;
        if (m_isSearchPart)
        {
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                SearchManager.Instance.IsTaskComplited = true;
            }
        }
        //OnEndDialog();
        EventManager.OnEvent(Events.FinishDialog);
        #endregion
    }

    /// <summary>
    /// アニメーションの再生が終了するまで待機する
    /// </summary>
    /// <param name="charaName"></param>
    /// <param name="positionIndex"></param>
    /// <param name="animation"></param>
    /// <returns></returns>
    IEnumerator WaitForCharaAnimation(string[] charaName, int[] positionIndex, string animation, int faceType = 3)
    {
        if (charaName[0] == "ナレーター")
        {
            m_namePanel.SetActive(false);
            yield break;
        }



        for (int i = 0; i < charaName.Length; i++)
        {
            if (!m_characterImage[positionIndex[i]].enabled)
            {
                m_characterImage[positionIndex[i]].enabled = true;
            }

            m_characterImage[positionIndex[i]].sprite = SetCharaImage(charaName[i], faceType);

            if (animation != null && animation != "なし") //アニメーションの指定があれば
            {
                AnimationAccordingType(animation, positionIndex[i]);
                isAnimPlaying = true;
                CharacterPanel.CharacterAnim += FinishReceive;
            }
            else if (animation == "なし")
            {
                AnimationAccordingType(animation, positionIndex[i]);
                yield break;
            }

            while (isAnimPlaying) //アニメーションが終わるまで待つ
            {
                if (IsInputed())
                {
                    m_anim[positionIndex[i]].Play("Idle");
                    isAnimPlaying = false;
                }
                yield return null;
            }
            CharacterPanel.CharacterAnim -= FinishReceive;
        }
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
        Debug.Log("終了");
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
    /// 全てシナリオを再生する
    /// </summary>
    public void StartAllScenario()
    {
        StartCoroutine(StartAllMessage());
    }

    /// <summary>
    /// 選択したシナリオを再生する。会話パートや探索パートの会話に使用する
    /// </summary>
    /// <param name="index"> データの添え字番号 </param>
    public void StartSelectScenario(int index)
    {
        if (m_currentCoroutine != null)
        {
            m_currentCoroutine = null;
        }

        StartCoroutine(StartSelectMessage(index));
    }

    /// <summary>
    /// 探索パートに入ったら最初に時代毎のシナリオを再生する
    /// </summary>
    public void StartBeginScenario()
    {
        switch (GameManager.Instance.CurrentPeriod)
        {
            case MasterData.PeriodTypes.None:
                Debug.LogError("時代が設定されていません");
                break;
            case MasterData.PeriodTypes.Jomon_Yayoi:
                StartCoroutine(StartSelectMessage(12));
                break;
            case MasterData.PeriodTypes.Asuka_Nara:
                break;
            case MasterData.PeriodTypes.Heian:
                break;
            case MasterData.PeriodTypes.Kamakura:
                break;
            case MasterData.PeriodTypes.Momoyama:
                break;
            case MasterData.PeriodTypes.Edo:
                break;
            default:
                break;
        }
    }

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

    public void Reception()
    {
        m_clickReception = true;
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
                for (int i = 0; i < m_effectPositions.Length; i++)
                {
                    m_effectPositions[i].sprite = m_transparentSprite;
                }
                break;
            case "AllFadeIn":
                for (int i = 0; i < m_characterImage.Length; i++)
                {
                    //if (!m_characterImage[i].enabled)
                    //{
                    //    m_characterImage[i].enabled = true;
                    //}
                    if (i == 0 && set1)
                    {
                        m_characterImage[i].enabled = true;
                    }
                    else if (i == 1 && set2)
                    {
                        m_characterImage[i].enabled = true;
                    }
                    else if (i == 2 && set3)
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
                    m_effectPositions[i].sprite = m_transparentSprite;
                }
                break;
            default:
                m_anim[index].Play("Idle");
                break;
        }
    }

    /// <summary>
    /// キャラクターのImageの状態をリセットする
    /// </summary>
    void ResetCharacterImages()
    {
        for (int i = 0; i < m_anim.Length; i++)
        {
            m_anim[i].Play("Idle");
        }
    }

    /// <summary>
    /// アクティブなキャラクター以外を暗転させる
    /// </summary>
    /// <param name="currentIndex"></param>
    void EmphasisCharacter(int[] currentIndex)
    {
        for (int i = 0; i < m_characterImage.Length; i++)
        {
            if (m_characterImage[i].enabled)
            {
                for (int n = 0; n < currentIndex.Length; n++)
                {
                    if (i == currentIndex[n])
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
    }

    /// <summary>
    /// 選択肢を生成する
    /// </summary>
    /// <param name="characterDatas"> キャラクターのデータ </param>
    /// <param name="data"> 選択肢のデータ </param>
    /// <param name="id"> 選択肢を押した後に表示するメッセージのID </param>
    void CreateChoices(DialogData[] characterDatas, ChoicesData data, int[] id)
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
        ContinueEvent?.Invoke();
    }

    void FixPlayVoice(VoiceType type, string id)
    {
        var s = id.Replace("player_", "voice").Replace("koma_", "voice");
        SoundManager.Instance.PlayVoice(type, s);
    }

    /// <summary>
    /// 全てのダイアログが終了したら実行されるイベント
    /// </summary>
    void OnEndDialog()
    {
        EndEvent?.Invoke();
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
        if (m_clickReception || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
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

                //感情のEffectを表示する処理をここに記述する

                break;
            }
        }
        return chara;
    }
    int m_beforeEmoteType = -1;
    void SetFeelingAnim(Image image, int emoteType)
    {
        if (m_beforeEmoteType == emoteType) return;
        image.sprite = m_feelingEffects[emoteType];
        m_beforeEmoteType = emoteType;
        FeelingType feelingType = (FeelingType)emoteType;
        //喜 => 一回上下
        //怒 => 左右に素早く
        //哀 => ゆっくり沈む
        //楽 => 数回上下
        //驚 => 一回上下
        //考 => 一回上下
        //正解 => 
        //不正解 => 
        Vector2 befPos = image.transform.position;
        Sequence sequence = DOTween.Sequence();
        switch (feelingType)
        {
            case FeelingType.Happy:
                sequence.Append(image.transform.DOMoveY(befPos.y + 50f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y, 0.1f));
                break;
            case FeelingType.Angry:
                sequence.Append(image.transform.DOMoveX(befPos.x + 50f, 0.1f))
                    .Append(image.transform.DOMoveX(befPos.x + -50f, 0.1f))
                    .Append(image.transform.DOMoveX(befPos.x, 0.1f));
                break;
            case FeelingType.Cry:
                sequence.Append(image.transform.DOMoveY(befPos.y + -50f, 1f))
                    .Append(image.transform.DOMoveY(befPos.y, 0.5f));
                break;
            case FeelingType.raku:
                sequence.Append(image.transform.DOMoveY(befPos.y + 30f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y + -30f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y + 30f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y + -30f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y, 0.1f));
                break;
            case FeelingType.Surprise:
                sequence.Append(image.transform.DOMoveY(befPos.y + 50f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y, 0.1f));
                break;
            case FeelingType.Think:
                sequence.Append(image.transform.DOMoveY(befPos.y + 50f, 0.1f))
                    .Append(image.transform.DOMoveY(befPos.y, 0.1f));
                break;
            case FeelingType.Correct:
                //正解
                break;
            case FeelingType.Incorrect:
                //不正解
                break;
        }
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
