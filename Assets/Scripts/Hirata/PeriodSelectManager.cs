using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MasterData;

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

    /// <summary>選択された時代番号の保存</summary>
    int m_periodNum;

    /// <summary>選択されたステージ番号の保存</summary>
    int m_selectedStageNum = 1;

    /// <summary>時代のクリアフラグの保存</summary>
    bool[] m_periodClearFlags = new bool[6];

    /// <summary>ステージのクリアフラグの保存</summary>
    bool[] m_stageClearFlag = new bool[4];

    /// <summary>決定ボタン</summary>
    [SerializeField]
    Button m_decisionButton;

    private void Awake()
    {
        GetPeriodClearFlag();
        SetButtonFlag(m_periodButtons, m_periodClearFlags);
    }

    /// <summary>
    /// ステージ選択のPanelをアクティブにする
    /// ボタンに設定する
    /// </summary>
    public void SelectPeriod(int period)
    {
        m_periodNum = period;
        GetStageClearFlag(period);
        SetButtonFlag(m_stageButtons, m_stageClearFlag);
        SetStageButton(m_stageTexts, m_periodStageData, period);
        m_stageSelectPanel.SetActive(true);
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
    Button[] SetButtonFlag(Button[] buttons, bool[] flags)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (flags[i])
            {
                buttons[i].interactable = true;
            }
            else
            {
                buttons[i].interactable = false;
            }
        }
        return buttons;
    }

    /// <summary>
    /// ボタンのテキストに、データをセットする
    /// </summary>
    /// <param name="texts">ボタンのテキスト</param>
    /// <param name="data">時代データ</param>
    /// <param name="period">時代番号</param>
    void SetStageButton(Text[] texts,PeriodStageData data, int period)
    {
        for (int i = 0; i < 4; i++)
        {
            texts[i].text = data.m_dataBases[period - 1].StageText[i];
        }
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
    }

    /// <summary>
    /// 選ばれたステージを保存し、強調表示するようにする
    /// </summary>
    /// <param name="stage">ステージ番号</param>
    void SetStageNum(int stage)
    {
        m_stageSelectImages[m_selectedStageNum - 1].gameObject.SetActive(false);
        m_selectedStageNum = stage;
        m_stageSelectImages[m_selectedStageNum - 1].gameObject.SetActive(true);
    }

    /// <summary>
    /// ステージのイメージをセットする
    /// </summary>
    /// <param name="data">ステージデータ</param>
    /// <param name="period">時代番号</param>
    /// <param name="stage">ステージ番号</param>
    void SetStageSprite(PeriodStageData data, int period, int stage)
    {
        m_stageCharactorImage.sprite = data.m_dataBases[period - 1].StageSprite[stage - 1];
    }

    /// <summary>
    /// 時代選択に戻る
    /// ボタンに設定する
    /// </summary>
    public void BackSelectPeriod()
    {
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)0;
        ResetSelectStage();
        m_stageSelectPanel.SetActive(false);
    }

    /// <summary>
    /// ステージ選択をリセットする
    /// </summary>
    void ResetSelectStage()
    {
        m_decisionButton.interactable = false;
        m_stageSelectImages[m_selectedStageNum - 1].gameObject.SetActive(false);
    }

    /// <summary>
    /// ゲームマネージャーに時代とステージを設定して、シーン移動する
    /// 決定ボタンに設定する
    /// </summary>
    /// <param name="scene">シーン名</param>
    public void Decision()
    {
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)m_periodNum;
        GameManager.Instance.CurrentStageId = m_selectedStageNum;

        if (m_selectedStageNum < 4)
        {
            LoadSceneManager.AnyLoadScene("SearchScenes");
        }
        else
        {
            LoadSceneManager.AnyLoadScene("QuizPart");
        }
    }
}