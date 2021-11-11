using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    /// <summary> 現在いる時代 </summary>
    [Header("デバッグ用")]
    [SerializeField]
    PeriodTypes m_currentPeriod = default;

    /// <summary> 現在のステージのID </summary>
    [SerializeField]
    int m_currentStageId = default;

    /// <summary> プレイヤーの性別 </summary>
    [SerializeField]
    GenderType m_playerGender = default;

    public int CurrentStageId { get => m_currentStageId; set => m_currentStageId = value; }
    public PeriodTypes CurrentPeriod { get => m_currentPeriod; set => m_currentPeriod = value; }

    public GenderType PlayerGender => m_playerGender;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        DataManager.Instance.PlayerData.PlayerGender = m_playerGender;
    }

    void Start()
    {
        
    }
}
