using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataObject : MonoBehaviour, ISave
{
    [SerializeField]
    SaveData.GameData gameData;

    [SerializeField]
    PlayerData m_playerData = default;

    public SaveData.GameData GameData => gameData;
    public string PlayerName { get => gameData.PlayerName; set => gameData.PlayerName = value; }

    public GenderType PlayerGender { get => gameData.Gender; set => gameData.Gender = value; }

    public GameManager.ClearFlagArray[] ClearFlag { get => gameData.ClearFlags; set => gameData.ClearFlags = value; }

    void Start()
    {
        SetUp();
    }

    public void Save(SaveData.GameData data)
    {
        data.PlayerName = gameData.PlayerName;
        data.Gender = gameData.Gender;
        data.ClearFlags = gameData.ClearFlags;
    }

    public void Load(SaveData.GameData data)
    {
        gameData.PlayerName = data.PlayerName;
        gameData.Gender = data.Gender;
        gameData.ClearFlags = data.ClearFlags;
    }

    void SetUp()
    {
        gameData = SaveManager.GetData().CurrentGameData;
        Debug.Log(gameData.PlayerName);
        m_playerData.PlayerName = gameData.PlayerName;
        m_playerData.PlayerGender = gameData.Gender;
        m_playerData.ClearFlags = gameData.ClearFlags;
    }
}
