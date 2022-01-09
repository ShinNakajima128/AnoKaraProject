using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GenderType
{
    Boy,
    Girl
}

/// <summary>
/// 各ステージのクイズ達成状況のステータス
/// </summary>
public enum StageQuizAchieveStates
{
    /// <summary> 未達成orHP0でやり直し </summary>
    None,
    /// <summary> HP1～2でステージクリア </summary>
    One,
    /// <summary> HP3～4でステージクリア </summary>
    Two,
    /// <summary> HP5(ノーダメージ)でステージクリア </summary>
    Three
}

[Serializable]
public class SaveData
{
    [Serializable]
    public class GameData
    {
        public bool FirstPlay = true;
        public string PlayerName;
        public GenderType Gender;
        public GameManager.ClearFlagArray[] ClearFlags;
        public StageAchieves[] AllStageAchieves;
    }

    public GameData CurrentGameData = new GameData();
}

[Serializable]
public class StageAchieves
{
    public StageQuizAchieveStates[] Achieves = new StageQuizAchieveStates[5];
}
