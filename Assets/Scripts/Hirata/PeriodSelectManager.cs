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

    /// <summary>選択されたステージ番号の保存</summary>
    int m_selectedStageNum;

    /// <summary>
    /// ステージ選択のPanelをアクティブにする
    /// </summary>
    public void SelectPeriod(int period)
    {
        SetPeriod(period);
        m_stageSelectPanel.SetActive(true);
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
    /// ステージボタンが選択された処理
    /// ボタンに設定する
    /// </summary>
    public void SelectStage(int stage)
    {
        m_stageSelectImage[m_selectedStageNum].gameObject.SetActive(false);
        m_selectedStageNum = stage - 1;
        m_stageSelectImage[stage - 1].gameObject.SetActive(true);
        SetStage(stage);
    }

    /// <summary>
    /// ステージ番号をゲームマネージャーに設定する
    /// </summary>
    /// <param name="stageNum">ステージ番号</param>
    void SetStage(int stageNum)
    {
        GameManager.Instance.CurrentStageId = stageNum;
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
    /// シーン移動
    /// 決定ボタンに設定する
    /// </summary>
    /// <param name="scene"></param>
    public void Decision(string scene)
    {
        LoadSceneManager.AnyLoadScene(scene);
    }
}