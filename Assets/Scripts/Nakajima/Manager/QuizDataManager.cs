using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

/// <summary>
/// クイズデータを管理するクラス
/// </summary>
public class QuizDataManager : SingletonMonoBehaviour<QuizDataManager>
{
    [Header("クイズデータ")]
    [SerializeField]
    AllQuizData m_allQuizData = default;

    delegate void LoadQuizDataCallback<T>(T data);

    public FourChoicesQuizData[] FourChoicesQuizDatas => m_allQuizData.FourChoicesQuizDatas;

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
        for (int i = 0; i < m_allQuizData.FourChoicesQuizDatas.Length; i++)
        {
            if (m_allQuizData.FourChoicesQuizDatas[i].PeriodTypeName == sheetName)  //プロパティとシート名が一致したら
            {
                LoadQuizMasterData(sheetName, (QuizMasterDataClass<FourChoicesQuiz> data) =>
                {
                    m_allQuizData.FourChoicesQuizDatas[i].FourChoicesQuiz = data.Data;  //データ更新
                });
                return;
            }
        }
        //データがロードできなかった場合
        Debug.LogError("データをロードできませんでした");　
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
