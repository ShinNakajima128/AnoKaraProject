using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterData;

/// <summary>
/// 時代選択画面のスタンプ機能を持つクラス
/// </summary>
public class Stamp : MonoBehaviour
{
    [Header("肉球スタンプの差し替え用Image")]
    [SerializeField]
    Image m_stamp = default;

    [Header("スタンプの背景")]
    [SerializeField]
    Image m_stampBackground = default;

    [Header("スタンプが配置してある時代")]
    [SerializeField]
    PeriodTypes m_period = default;

    [Header("通常クリアの場合のスタンプ")]
    [SerializeField]
    Sprite m_normalClearStamp = default;

    [Header("全てのステージを花3でクリアした場合のスタンプ")]
    [SerializeField]
    Sprite m_perfectClearStamp = default;

    void Start()
    {
        SetStampImage();
    }

    void SetStampImage()
    {
        var stageAchieves = DataManager.Instance.PlayerData.StageAchieves[(int)m_period - 1];
        bool normal = true;
        bool perfect = true;

        Debug.Log($"時代:{m_period}、ステージ数：{stageAchieves.Achieves.Length}");

        for (int i = 0; i < stageAchieves.Achieves.Length; i++)
        {
            if (stageAchieves.Achieves[i] == StageQuizAchieveStates.None)
            {
                normal = false;
                perfect = false;
                break;
            }
            else if (stageAchieves.Achieves[i] == StageQuizAchieveStates.One || stageAchieves.Achieves[i] == StageQuizAchieveStates.Two)
            {
                normal = true;
                perfect = false;
            }
        }

        if (perfect)
        {
            m_stampBackground.enabled = true;
            m_stamp.enabled = true;
            m_stamp.sprite = m_perfectClearStamp;
        }
        else if (normal && !perfect)
        {
            m_stampBackground.enabled = true;
            m_stamp.enabled = true;
            m_stamp.sprite = m_normalClearStamp;
        }
        else
        {
            m_stampBackground.enabled = false;
            m_stamp.enabled = false;
        }
    }
}
