using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MasterData;

/// <summary>時代・ステージ選択画面マネージャー</summary>
public class PeriodSelectManager : MonoBehaviour
{
    /// <summary>ステージ選択画面のパネル</summary>
    [SerializeField]
    GameObject m_stageSelectPanel;

    /// <summary>ステージ選択画面のキャラクターを表示するイメージ</summary>
    [SerializeField]
    Image m_stageCharactorImage;

    /// <summary>ステージ選択のボタン</summary>
    [SerializeField]
    Button[] m_stageButtons;

    /// <summary>ステージ選択のテキスト</summary>
    [SerializeField]
    Text[] m_stageText;

    /// <summary>ボタンが選択されたフレーム</summary>
    [SerializeField]
    GameObject[] m_stageSelectImage;

    /// <summary>選択された時代番号の保存</summary>
    int m_periodNum;

    /// <summary>選択されたステージ番号の保存</summary>
    int m_selectedStageNum;

    /// <summary>ステージのクリアフラグの保存</summary>
    bool[] m_clearFlag = new bool[4];

    /// <summary>決定ボタン</summary>
    [SerializeField]
    Button m_decisionButton;

    /// <summary>
    /// ステージ選択のPanelをアクティブにする
    /// </summary>
    public void SelectPeriod(int period)
    {
        m_periodNum = period;
        GetPeriodFlag(period);
        SetButtonFlag(m_stageButtons, m_clearFlag);
        m_stageSelectPanel.SetActive(true);
    }

    /// <summary>
    /// ゲームマネージャーから、その時代のステージフラグを受け取る
    /// </summary>
    void GetPeriodFlag(int period)
    {
        m_clearFlag = GameManager.Instance.CheckFlag(period);
    }

    /// <summary>
    /// フラグに応じて、ボタンのinteractableを有効化する
    /// </summary>
    /// <param name="buttons">ステージボタンの配列</param>
    /// <param name="flags">ステージのクリアフラグ</param>
    Button[] SetButtonFlag(Button[] buttons, bool[] flags)
    {
        for (int i = 0; i < 4; i++)
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
    /// ステージボタンが選択された処理
    /// ボタンに設定する
    /// </summary>
    public void SelectStage(int stage)
    {
        m_decisionButton.interactable = true;
        m_stageSelectImage[m_selectedStageNum].gameObject.SetActive(false);
        m_selectedStageNum = stage - 1;
        m_stageSelectImage[stage - 1].gameObject.SetActive(true);
    }

    /// <summary>
    /// 時代選択に戻る
    /// ボタンに設定する
    /// </summary>
    public void BackSelectPeriod()
    {
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)0;
        m_stageSelectPanel.SetActive(false);
    }

    /// <summary>
    /// ゲームマネージャーに時代とステージを設定して、シーン移動する
    /// 決定ボタンに設定する
    /// </summary>
    /// <param name="scene">シーン名</param>
    public void Decision(string scene)
    {
        SetPeriod(m_periodNum);
        SetStage(m_selectedStageNum);
        LoadSceneManager.AnyLoadScene(scene);
    }

    /// <summary>
    /// 時代をゲームマネージャーに設定する
    /// </summary>
    /// <param name="period">時代の列挙番号</param>
    void SetPeriod(int period)
    {
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)period;
    }

    /// <summary>
    /// ステージ番号をゲームマネージャーに設定する
    /// </summary>
    /// <param name="stageNum">ステージ番号</param>
    void SetStage(int stageNum)
    {
        GameManager.Instance.CurrentStageId = stageNum;
    }
}