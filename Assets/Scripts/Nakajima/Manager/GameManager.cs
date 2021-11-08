using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("デバッグ用")]
    [SerializeField]
    int m_currentStageId = default;

    [SerializeField]
    PeriodTypes m_currentPeriod = default;

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
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
