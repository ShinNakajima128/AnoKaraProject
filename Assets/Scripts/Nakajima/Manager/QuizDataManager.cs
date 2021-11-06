using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

/// <summary>
/// クイズデータを管理するクラス
/// </summary>
public class QuizDataManager : MonoBehaviour
{
    [Header("クイズデータ")]
    [SerializeField]
    AllQuizData m_allQuizData = default;

    delegate void LoadQuizDataCallback<T>(T data);

    public static QuizDataManager Instance { get; private set; }
    /// <summary> 4択クイズのデータ </summary>
    public FourChoicesQuizData[] FourChoicesQuizDatas => m_allQuizData.FourChoicesQuizDatas;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// スプレッドシートから4択クイズデータをロードする。※この関数はEditor上でのみ使用する関数なので、ゲーム中に実行されるクラスでは使わないでください。
    /// </summary>
    /// <param name="url"> スプレッドシートのURL </param>
    /// <param name="sheetName"> シート名 </param>
    public void LoadFourChoicesQuizDataFromSpreadsheet(string url, string sheetName)
    {
        for (int i = 0; i < m_allQuizData.FourChoicesQuizDatas.Length; i++)
        {
            if (m_allQuizData.FourChoicesQuizDatas[i].PeriodTypeName == sheetName)  //プロパティとシート名が一致したら
            {
                LoadQuizMasterData(url, sheetName, (QuizMasterDataClass<FourChoicesQuiz> data) =>
                {
                    m_allQuizData.FourChoicesQuizDatas[i].FourChoicesQuizzes = data.Data;  //データ更新
                });
                return;
            }
        }
        //データがロードできなかった場合
        Debug.LogError("データをロードできませんでした");　
    }

    /// <summary>
    /// スプレッドシートから穴埋めクイズデータをロードする。※この関数はEditor上でのみ使用する関数なので、ゲーム中に実行されるクラスでは使わないでください。
    /// </summary>
    /// <param name="url"> スプレッドシートのURL </param>
    /// <param name="sheetName"> シート名 </param>
    public void LoadFourAnaumeQuizDataFromSpreadsheet(string url, string sheetName)
    {
        //もしクラス名に変更があれば、m_allQuizData.AnaumeQuizDatasと仮で書いてある「AnaumeQuizDatas」の部分を
        //作成したScriptableObjectのクラス名に変更してください

        //for (int i = 0; i < m_allQuizData.AnaumeQuizDatas.Length; i++)
        //{
        //    if (m_allQuizData.AnaumeQuizDatas[i].PeriodTypeName == sheetName)  //プロパティとシート名が一致したら
        //    {
        //        LoadQuizMasterData(url, sheetName, (QuizMasterDataClass<AnaumeQuizData> data) =>
        //        {
        //            m_allQuizData.AnaumeQuizDatas[i].AnaumeQuizzes = data.Data;  //データ更新
        //        });
        //        return;
        //    }
        //}
        ////データがロードできなかった場合
        //Debug.LogError("データをロードできませんでした");
    }

    /// <summary>
    /// クイズデータを読み込む
    /// </summary>
    /// <typeparam name="T"> クイズデータのクラス </typeparam>
    /// /// <param name="url"> スプレッドシートのURL </param>
    /// <param name="file"> クイズの時代名 </param>
    /// <param name="callback"></param>
    void LoadQuizMasterData<T>(string url, string file, LoadQuizDataCallback<T> callback)
    {
        if (file == "None")
        {
            Debug.LogError("クイズデータの時代を指定してください");
            return;
        }
        Network.WebRequest.Request<Network.WebRequest.GetString>(url + "?sheet=" + file, Network.WebRequest.ResultType.String, (string json) =>
        {
            var dldata = JsonUtility.FromJson<T>(json);
            callback(dldata);
            Debug.Log("Network download. : " + file + " / " + json + "/" + dldata);
        });
    }
}