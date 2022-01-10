using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary>ステージのフラグクラス</summary>
    [System.Serializable]
    public class ClearFlagArray
    {
        /// <summary>ステージのフラグ</summary>
        public bool[] m_stageClearFlag = new bool[5];
    }

    /// <summary> 現在いる時代 </summary>
    [Header("デバッグ用")]
    [SerializeField]
    PeriodTypes m_currentPeriod = default;

    /// <summary> 現在のステージのID </summary>
    [SerializeField]
    int m_currentStageId = default;

    /// <summary> クイズ後かどうかのフラグ </summary>
    [SerializeField]
    bool m_isAfterQuized = false;

    public int CurrentStageId { get => m_currentStageId; set => m_currentStageId = value; }
    public PeriodTypes CurrentPeriod { get => m_currentPeriod; set => m_currentPeriod = value; }

    /// <summary> クイズシーンから戻っていたかどうかのフラグ </summary>
    public bool IsAfterQuized { get => m_isAfterQuized; set => m_isAfterQuized = value; }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
