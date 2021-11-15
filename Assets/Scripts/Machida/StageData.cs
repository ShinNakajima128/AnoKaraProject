using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

/// <summary>
/// ステージごとの管理
/// </summary>
[System.Serializable]
public class StageData
{
    /// <summary>時代設定<summary>
    [SerializeField]
    PeriodTypes m_periodType = default;

    /// <summary>ステージID<summary>
    [SerializeField]
    int m_stageId = default;

    /// <summary>表示するPanel<summary>
    [SerializeField]
    GameObject m_stagePanel = default;

    /// <summary>タスク数<summary>
    [SerializeField]
    int m_taskCount = default;

    public PeriodTypes PeriodType => m_periodType;

    public int StageId => m_stageId;

    public GameObject StagePanel => m_stagePanel;

    public int Task => m_taskCount;
}
