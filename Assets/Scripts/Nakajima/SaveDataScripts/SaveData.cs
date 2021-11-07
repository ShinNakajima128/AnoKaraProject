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
        public string PlayerName;
        public GenderType Gender;
        public int Progress;
    }

    public GameData CurrentGameData = new GameData();
}
