using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GenderType
{
    Boy,
    Girl
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
    }

    public GameData CurrentGameData = new GameData();
}
