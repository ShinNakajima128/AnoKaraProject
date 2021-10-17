using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

public class QuizDataManager : SingletonMonoBehaviour<QuizDataManager>
{
    [SerializeField]
    bool IsVersionUpFlag = false;

    [SerializeField]
    FourChoicesQuizData[] m_fourChoicesQuizDatas = default;

    QuizMasterDataClass<FourChoicesQuiz> fourChoicesQuisMaster;
    delegate void LoadQuizDataCallback<T>(T data);
    int m_loadingCount = 0;

    FourChoicesQuiz[] FourChoicesQuizMaster => fourChoicesQuisMaster.Data;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadQuizMasterData("Jomon", (QuizMasterDataClass<FourChoicesQuiz> data) => fourChoicesQuisMaster = data);
    }

    void SetData()
    {
        for (int i = 0; i < m_fourChoicesQuizDatas.Length; i++)
        {

        }
    }

    void LoadQuizMasterData<T>(string file, LoadQuizDataCallback<T> callback)
    {
        var data = LocalData.Load<T>(file);
        if (data == null || IsVersionUpFlag)
        {
            m_loadingCount++;
            Network.WebRequest.Request<Network.WebRequest.GetString>("https://script.google.com/macros/s/AKfycbySn0LqADyPQokOnUhHLJ_Bm6eai9oJXJmhnWn4jmInvmhepe8/exec?Sheet=" + file, Network.WebRequest.ResultType.String, (string json) =>
            {
                var dldata = JsonUtility.FromJson<T>(json);
                LocalData.Save<T>(file, dldata);
                callback(dldata);
                Debug.Log("Network download. : " + file + " / " + json + "/" + dldata);
                --m_loadingCount;
            });
        }
        else
        {
            Debug.Log("Local load. : " + file + " / " + data);
            callback(data);
        }
    }
}
