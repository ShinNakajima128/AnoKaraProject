using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debug.Log(Application.dataPath);
        //LoadData();   
    }

    public void SaveData()
    {
        SaveData data = SaveManager.GetData();
        TestPlayer gameData = FindObjectOfType<TestPlayer>();

        ISave saveIf = gameData.GetComponent<ISave>();
        saveIf.Save(data.CurrentGameData);
        Debug.Log(data.CurrentGameData);
        SaveManager.Save();
    }

    public void LoadData()
    {
        SaveManager.Load();
        SaveData data = SaveManager.GetData();

        var gameData = FindObjectOfType<TestPlayer>();

        ISave saveIf = gameData.GetComponent<ISave>();
        saveIf.Load(data.CurrentGameData);
        Debug.Log(data);
    }
}
