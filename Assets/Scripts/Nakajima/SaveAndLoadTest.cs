using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadTest : MonoBehaviour
{
    public static SaveAndLoadTest Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SaveData()
    {
        SaveData data = SaveManager.GetData();
        GameDataObject gameData = FindObjectOfType<GameDataObject>();

        ISave saveIf = gameData.GetComponent<ISave>();
        saveIf.Save(data.CurrentGameData);
        Debug.Log(data.CurrentGameData);
        SaveManager.Save();
    }

    public void LoadData()
    {
        SaveManager.Load();
        SaveData data = SaveManager.GetData();

        var gameData = FindObjectOfType<GameDataObject>();

        ISave saveIf = gameData.GetComponent<ISave>();
        saveIf.Load(data.CurrentGameData);
        Debug.Log(data);
    }
}