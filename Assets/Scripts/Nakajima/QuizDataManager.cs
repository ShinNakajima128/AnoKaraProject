using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

/// <summary>
/// クイズデータを管理するクラス
/// </summary>
public class QuizDataManager : SingletonMonoBehaviour<QuizDataManager>
{
    [Header("クイズデータを更新したかどうかのフラグ")]
    [SerializeField]
    bool m_isVersionUpFlag = false;

    [Header("四択クイズのデータ")]
    [SerializeField]
    FourChoicesQuizData[] m_fourChoicesQuizDatas = default;

    QuizMasterDataClass<FourChoicesQuiz> m_fourChoicesQuisMaster;
    delegate void LoadQuizDataCallback<T>(T data);
    int m_loadingCount = 0;

    public FourChoicesQuiz[] FourChoicesQuizMaster => m_fourChoicesQuisMaster.Data;
    public bool OnData { get; private set; }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadQuizMasterData("Jomon", (QuizMasterDataClass<FourChoicesQuiz> data) => m_fourChoicesQuisMaster = data);
    }

    /// <summary>
    /// 各クイズのScriptableObjectにデータをセットする
    /// </summary>
    void SetData()
    {
        for (int i = 0; i < m_fourChoicesQuizDatas.Length; i++)
        {
            m_fourChoicesQuizDatas[i].FourChoicesQuiz = m_fourChoicesQuisMaster.Data[i];
        }
        OnData = true;
    }

    /// <summary>
    /// クイズデータを読み込む
    /// </summary>
    /// <typeparam name="T"> クイズデータのクラス </typeparam>
    /// <param name="file"> クイズの時代名 </param>
    /// <param name="callback"></param>
    void LoadQuizMasterData<T>(string file, LoadQuizDataCallback<T> callback)
    {
        var data = LocalData.Load<T>(file);
        if (data == null || m_isVersionUpFlag)
        {
            m_loadingCount++;
            Network.WebRequest.Request<Network.WebRequest.GetString>("https://script.google.com/macros/s/AKfycbySn0LqADyPQokOnUhHLJ_Bm6eai9oJXJmhnWn4jmInvmhepe8/exec?Sheet=" + file, Network.WebRequest.ResultType.String, (string json) =>
            {
                var dldata = JsonUtility.FromJson<T>(json);
                LocalData.Save<T>(file, dldata);
                callback(dldata);
                Debug.Log("Network download. : " + file + " / " + json + "/" + dldata);
                --m_loadingCount;
                SetData();
            });
        }
        else
        {
            Debug.Log("Local load. : " + file + " / " + data);
            callback(data);
        }
    }
}
