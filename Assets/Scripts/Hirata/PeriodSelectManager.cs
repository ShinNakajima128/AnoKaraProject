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
    Image m_stageCharaImage;

    /// <summary>
    /// ステージ選択のPanelをアクティブにする
    /// </summary>
    public void SelectStage(int period)
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
    /// 時代選択に戻る
    /// </summary>
    public void BackSelectPeriod()
    {
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)0;
        m_stageSelectPanel.SetActive(false);
    }
}