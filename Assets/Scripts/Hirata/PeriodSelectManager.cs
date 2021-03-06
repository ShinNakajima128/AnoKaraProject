using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MasterData;
using DG.Tweening;

/// <summary>時代・ステージ選択画面マネージャー</summary>
public class PeriodSelectManager : MonoBehaviour
{
    /// <summary>時代のステージデータ</summary>
    [SerializeField]
    PeriodStageData m_periodStageData;

    /// <summary>ステージ選択画面のパネル</summary>
    [SerializeField]
    GameObject m_stageSelectPanel;

    /// <summary>ステージ選択画面のキャラクターを表示するイメージ</summary>
    [SerializeField]
    Image m_stageCharactorImage;

    /// <summary>ステージ選択のデフォルト画像</summary>
    [SerializeField]
    Sprite m_stageDefaultSprite;

    /// <summary>時代選択ボタン</summary>
    [SerializeField]
    Button[] m_periodButtons;

    /// <summary>ステージ選択のボタン</summary>
    [SerializeField]
    Button[] m_stageButtons;

    /// <summary>ステージ選択のテキスト</summary>
    [SerializeField]
    Text[] m_stageTexts;

    /// <summary>ボタンが選択されたフレーム</summary>
    [SerializeField]
    GameObject[] m_stageSelectImages;

    /// <summary> 各時代の巻物の背景 </summary>
    [SerializeField]
    Sprite[] m_periodBackgroundSprites = default;

    /// <summary> アチーブ表示用のクラス </summary>
    [SerializeField]
    StageAchieveControl[] m_achieveCtrls = default;

    /// <summary> 時代選択画面のヘルプ画面 </summary>
    [SerializeField]
    GameObject m_periodSelectHelpPanel = default;

    /// <summary> オプションボタンをまとめたPanel </summary>
    GameObject m_settingPanel = default;

    /// <summary>選択された時代番号の保存</summary>
    int m_periodNum;

    /// <summary>選択されたステージ番号の保存</summary>
    int m_selectedStageNum = 1;

    /// <summary>時代のクリアフラグの保存</summary>
    bool[] m_periodClearFlags = new bool[6];

    /// <summary>ステージのクリアフラグの保存</summary>
    bool[] m_stageClearFlag = new bool[4];

    Image m_periodBackground = default;
    bool m_isScrollAnimed = false;
    Sequence m_decisionButtonSeq = default;

    /// <summary>決定ボタン</summary>
    [SerializeField]
    Button m_decisionButton;

    [SerializeField]
    Text m_headerText = default;

    [SerializeField]
    bool m_isDebugMode = default;

    [SerializeField]
    float m_buttonAnimTimer = 0.5f;
    public static PeriodSelectManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        if (m_isDebugMode)
        {
            for (int i = 0; i < m_periodClearFlags.Length; i++)
            {
                m_periodClearFlags[i] = true;
            }

            for (int i = 0; i < m_stageClearFlag.Length; i++)
            {
                m_stageClearFlag[i] = true;
            }
            SetButtonFlag(m_periodButtons, m_periodClearFlags, 6);
        }
        else
        {
            GetPeriodClearFlag();
            SetButtonFlag(m_periodButtons, m_periodClearFlags, 6);
            DataManager.UpdateData();

            //縄文をまだクリアしていなかったらヘルプ画面を表示
            var a = DataManager.Instance.PlayerData.StageAchieves[0];
            if (a.Achieves[0] == StageQuizAchieveStates.None)
            {
                OnHelpPanel();
            }
        }
    }

    private void Start()
    {
        SoundManager.Instance.PlayBgm(SoundManager.Instance.BgmName);
        m_periodBackground = m_stageSelectPanel.GetComponent<Image>();
        m_settingPanel = GameObject.FindGameObjectWithTag("Setting");
    }

    /// <summary>
    /// ステージ選択のPanelをアクティブにする
    /// ボタンに設定する
    /// </summary>
    public void SelectPeriod(int period)
    {
        if (m_isScrollAnimed)
        {
            return;
        }

        m_isScrollAnimed = true;

        var s = DOTween.Sequence();

        s.Append(m_periodButtons[period - 1].gameObject.transform.DOMoveY(-150, m_buttonAnimTimer))
         .AppendCallback(() => { SoundManager.Instance.PlaySe("SE_title"); })
         .Append(m_periodButtons[period - 1].gameObject.transform.DOMoveY(200, m_buttonAnimTimer + 0.2f))
         .OnComplete(() => 
         {
             LoadSceneManager.WhiteFadeOutPanel(() =>
             {
                 m_periodButtons[period - 1].gameObject.transform.DOMoveY(-48.5f, 0.01f).OnComplete(() => 
                 {
                     m_periodNum = period;
                     GetStageClearFlag(period);
                     SetButtonFlag(m_stageButtons, m_stageClearFlag, StageCheck((PeriodTypes)period));
                     SetStageButton(m_stageTexts, m_periodStageData, period);
                     m_stageSelectPanel.SetActive(true);
                     if (m_settingPanel != null)
                     {
                         m_settingPanel.SetActive(false);
                     }
                     SetStageBuckground(period);
                     m_headerText.text = "ステージ選択";
                     m_stageCharactorImage.sprite = DataManager.Instance.PlayerData.PlayerImage[0];

                     foreach (var c in m_achieveCtrls)
                     {
                         c.gameObject.SetActive(false);
                     }

                     for (int i = 0; i < DataManager.Instance.PlayerData.StageAchieves[period - 1].Achieves.Length; i++)
                     {
                         m_achieveCtrls[i].gameObject.SetActive(true);
                         m_achieveCtrls[i].ViewAchieve(DataManager.Instance.PlayerData.StageAchieves[period - 1].Achieves[i]);
                     }
                     LoadSceneManager.WhiteFadeInPanel();
                     m_isScrollAnimed = false;
                     SelectStage(0);
                 });          
             });
         })
         .Play(); 
    }

    /// <summary>
    /// ヘルプ画面を開く
    /// </summary>
    public void OnHelpPanel()
    {
        m_periodSelectHelpPanel.SetActive(true);
        m_periodSelectHelpPanel.transform.localScale = Vector3.zero;
        m_periodSelectHelpPanel.transform.DOScale(Vector3.one, 0.1f).OnComplete(() => { SoundManager.Instance.PlaySe("SE_touch"); });
    }


    /// <summary>
    /// 選択したらSEを再生する。ボタンに設置
    /// </summary>
    public void PlaySelectSound()
    {
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// ヘルプ画面を閉じる
    /// </summary>
    public void OffHelpPanel()
    {
        m_periodSelectHelpPanel.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            m_periodSelectHelpPanel.SetActive(false);
        });
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// PlayerDataから、時代のクリアフラグを受け取る
    /// </summary>
    void GetPeriodClearFlag()
    {
        for (int i = 0; i < 6; i++)
        {
            m_periodClearFlags[i] = DataManager.Instance.
                PlayerData.ClearFlags[i].m_stageClearFlag[0];
        }
    }

    /// <summary>
    /// PlayerDataから、指定した時代のステージのクリアフラグを受け取る
    /// </summary>
    /// <param name="period">時代番号</param>
    void GetStageClearFlag(int period)
    {
        for (int i = 0; i < 4; i++)
        {
            m_stageClearFlag[i] = DataManager.Instance.
                PlayerData.ClearFlags[period - 1].m_stageClearFlag[i + 1];
        }
    }

    /// <summary>
    /// フラグに応じて、ボタンのinteractableを有効化する
    /// </summary>
    /// <param name="buttons">ボタンの配列</param>
    /// <param name="flags">クリアフラグ</param>
    Button[] SetButtonFlag(Button[] buttons, bool[] flags, int stageNum)
    {
        if (m_isDebugMode)
        {
            foreach (var b in buttons)
            {
                b.gameObject.SetActive(true);
            }
            for (int i = 0; i < stageNum; i++)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].interactable = true;
            }
            return buttons;
        }

        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }

        for (int i = 0; i < stageNum; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].interactable = flags[i] ? true : false;
        }

        return buttons;
    }

    /// <summary>
    /// ボタンのテキストに、データをセットする
    /// </summary>
    /// <param name="texts">ボタンのテキスト</param>
    /// <param name="data">時代データ</param>
    /// <param name="period">時代番号</param>
    void SetStageButton(Text[] texts, PeriodStageData data, int period)
    {
        for (int i = 0; i < 4; i++)
        {
            texts[i].text = data.m_dataBases[period - 1].StageText[i];
        }
    }

    /// <summary>
    /// 選択された時代のステージ数を返す
    /// </summary>
    /// <param name="period"> 選択した時代 </param>
    /// <returns> 選択した時代のステージ数 </returns>
    int StageCheck(PeriodTypes period)
    {
        int n = 0;

        switch (period)
        {
            case PeriodTypes.Jomon_Yayoi:
                n = 1;
                break;
            case PeriodTypes.Asuka_Nara:
                n = 2;
                break;
            case PeriodTypes.Heian:
                n = 3;
                break;
            case PeriodTypes.Kamakura:
                n = 4;
                break;
            case PeriodTypes.Momoyama:
                n = 3;
                break;
            case PeriodTypes.Edo:
                n = 3;
                break;
            default:
                break;
        }
        return n;
    }

    /// <summary>
    /// ステージボタンが選択された処理
    /// ボタンに設定する
    /// </summary>
    public void SelectStage(int stage)
    {
        m_decisionButton.interactable = true;
        SetStageNum(stage);
        SetStageSprite(m_periodStageData, m_periodNum, m_selectedStageNum);

        DecisionButtonAnim();
    }

    /// <summary>
    /// 選ばれたステージを保存し、強調表示するようにする
    /// </summary>
    /// <param name="stage">ステージ番号</param>
    void SetStageNum(int stage)
    {
        m_stageSelectImages[m_selectedStageNum].gameObject.SetActive(false);
        m_selectedStageNum = stage;
        m_stageSelectImages[m_selectedStageNum].gameObject.SetActive(true);
    }

    /// <summary>
    /// ステージ選択の決定ボタンのアニメーション
    /// </summary>
    void DecisionButtonAnim()
    {
        if (m_decisionButtonSeq == null)
        {
            m_decisionButtonSeq = DOTween.Sequence();
            var db = m_decisionButton.gameObject.GetComponent<RectTransform>();
            var startEndValue = new Vector3(1.1f, 1.1f, 1.1f);

            m_decisionButtonSeq.Append(db.DOScale(startEndValue, 1f))
                                                      .Append(db.DOScale(Vector3.one, 1f))
                                                      .SetLoops(-1)
                                                      .Play();
        }     
    }

    /// <summary>
    /// ステージのイメージをセットする
    /// </summary>
    /// <param name="data">ステージデータ</param>
    /// <param name="period">時代番号</param>
    /// <param name="stage">ステージ番号</param>
    void SetStageSprite(PeriodStageData data, int period, int stage)
    {
        m_stageCharactorImage.sprite = data.m_dataBases[period - 1].StageSprite[stage];
    }

    void SetStageBuckground(int period)
    {
        switch ((PeriodTypes)period)
        {
            case PeriodTypes.Jomon_Yayoi:
                m_periodBackground.sprite = m_periodBackgroundSprites[0];
                break;
            case PeriodTypes.Asuka_Nara:
                m_periodBackground.sprite = m_periodBackgroundSprites[1];
                break;
            case PeriodTypes.Heian:
                m_periodBackground.sprite = m_periodBackgroundSprites[2];
                break;
            case PeriodTypes.Kamakura:
                m_periodBackground.sprite = m_periodBackgroundSprites[3];
                break;
            case PeriodTypes.Momoyama:
                m_periodBackground.sprite = m_periodBackgroundSprites[4];
                break;
            case PeriodTypes.Edo:
                m_periodBackground.sprite = m_periodBackgroundSprites[5];
                break;
            default:
                Debug.LogError("時代が正しく設定されていません");
                break;
        }
    }

    /// <summary>
    /// 時代選択に戻る
    /// ボタンに設定する
    /// </summary>
    public void BackSelectPeriod()
    {
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)0;
        m_stageCharactorImage.sprite = DataManager.Instance.PlayerData.PlayerImage[0];
        ResetSelectStage();
        m_stageSelectPanel.SetActive(false);
        if (m_settingPanel != null)
        {
            m_settingPanel.SetActive(true);
        }
        m_headerText.text = "時代選択";
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// ステージ選択をリセットする
    /// </summary>
    void ResetSelectStage()
    {
        m_decisionButton.interactable = false;
        m_stageSelectImages[m_selectedStageNum].gameObject.SetActive(false);
    }

    /// <summary>
    /// ゲームマネージャーに時代とステージを設定して、シーン移動する
    /// 決定ボタンに設定する
    /// </summary>
    /// <param name="scene">シーン名</param>
    public void Decision()
    {
        //選択したステージの時代とIDをゲームマネージャーに保存
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)m_periodNum;
        GameManager.Instance.CurrentStageId = m_selectedStageNum;

        //探索Sceneへ遷移
        LoadSceneManager.AnyLoadScene("SearchScenes");
        SoundManager.Instance.PlaySe("SE_title");
    }
}