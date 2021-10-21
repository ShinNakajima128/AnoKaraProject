using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, ISave
{
    [SerializeField]
    SaveData.GameData gameData;

    public SaveData.GameData GameData => gameData;
    public string PlayerName { get => gameData.PlayerName; set => gameData.PlayerName = value; }

    public int Progress { get => gameData.Progress; set => gameData.Progress = value; }

    void Start()
    {
        gameData = SaveManager.GetData().CurrentGameData;
    }

    public void Save(SaveData.GameData data)
    {
        data.PlayerName = gameData.PlayerName;
        data.Gender = gameData.Gender;
        data.Progress = gameData.Progress;
    }

    public void Load(SaveData.GameData data)
    {
        gameData.PlayerName = data.PlayerName;
        gameData.Gender = data.Gender;
        gameData.Progress = data.Progress;
    }
}
