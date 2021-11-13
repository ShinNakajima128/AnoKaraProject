﻿using System.Collections;
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
        public bool[] m_stageClearFlag;
    }

    /// <summary>各クリアフラグ</summary>
    [SerializeField]
    ClearFlagArray[] m_periodClearFlag;

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

    /// <summary>
    /// 各クリアフラグ
    /// 行:時代開放フラグ,ステージ1～4    列:時代
    /// </summary>
    bool[,] m_clearFlag = new bool[6, 5];

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
        m_clearFlag[0, 0] = true;
        m_clearFlag[0, 1] = true;

        m_periodClearFlag[0].m_stageClearFlag[0] = true;
    }

    /// <summary>
    /// クリアフラグを有効化する
    /// </summary>
    /// <param name="periodNum">時代番号</param>
    /// <param name="stageNum">ステージ番号</param>
    public void FlagOpen(int periodNum, int stageNum)
    {
        m_clearFlag[periodNum - 1, stageNum] = true;
    }

    /// <summary>
    /// 指定された時代の各ステージのフラグ状況を返す
    /// </summary>
    /// <param name="periodNum">時代番号</param>
    /// <returns>各ステージフラグ</returns>
    public bool[] CheckFlag(int periodNum)
    {
        bool[] flag = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            flag[i] = m_clearFlag[periodNum - 1, i + 1];
        }
        return flag;
    }
}
