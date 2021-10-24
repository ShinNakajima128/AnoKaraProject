using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

/// <summary>
/// クイズデータを管理するクラス
/// </summary>
public class QuizDataManager : SingletonMonoBehaviour<QuizDataManager>
{
    [Header("四択クイズのデータ")]
    [SerializeField]
    FourChoicesQuizData m_fourChoicesQuizData = default;

    delegate void LoadQuizDataCallback<T>(T data);

    public FourChoicesQuiz[] FourChoicesQuizMaster => m_fourChoicesQuizData.FourChoicesQuiz;

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// スプレッドシートからデータをロードする。※この関数はEditor上でのみ使用する関数なので、ゲーム中に実行されるクラスでは使わないでください。
    /// </summary>
    /// <param name="sheetName"> シート名 </param>
    public void LoadDataFromSpreadsheet(string sheetName)
    {
        LoadQuizMasterData(sheetName, (QuizMasterDataClass<FourChoicesQuiz> data) =>
        {
            if (m_fourChoicesQuizData.FourChoicesQuiz != null)
            {
                m_fourChoicesQuizData.FourChoicesQuiz = null;
            }
            var fourChoicesQuisMaster = data;
            m_fourChoicesQuizData.FourChoicesQuiz = fourChoicesQuisMaster.Data;
        });
    }

    /// <summary>
    /// クイズデータを読み込む
    /// </summary>
    /// <typeparam name="T"> クイズデータのクラス </typeparam>
    /// <param name="file"> クイズの時代名 </param>
    /// <param name="callback"></param>
    void LoadQuizMasterData<T>(string file, LoadQuizDataCallback<T> callback)
    {
        if (file == "None")
        {
            Debug.LogError("クイズデータの時代を指定してください");
            return;
        }
        Network.WebRequest.Request<Network.WebRequest.GetString>("https://script.google.com/macros/s/AKfycbwMDfg7S09aVHqyLve2ypq1jwgbYDgIZT25abH-Yp3oHIhtieA/exec?sheet=" + file, Network.WebRequest.ResultType.String, (string json) =>
        {
            var dldata = JsonUtility.FromJson<T>(json);
            callback(dldata);
            Debug.Log("Network download. : " + file + " / " + json + "/" + dldata);
        });
    }
}
