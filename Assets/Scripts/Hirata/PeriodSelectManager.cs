using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MasterData;

/// <summary>時代選択画面マネージャー</summary>
public class PeriodSelectManager : MonoBehaviour
{
    /// <summary>
    /// 選択された時代に移動する
    /// </summary>
    /// <param name="period">時代のシーン名</param>
    public void PeriodButton(int period)
    {
        Debug.Log("押された");
        GameManager.Instance.CurrentPeriod = (MasterData.PeriodTypes)period;
        LoadSceneManager.AnyLoadScene("StageSelect", () =>
        {
            LoadSceneManager.FadeOutPanel();
        });
    }
}