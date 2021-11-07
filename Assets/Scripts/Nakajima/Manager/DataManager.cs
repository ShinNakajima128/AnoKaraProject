﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;
using ScenarioMasterData;

/// <summary>
/// クイズデータを管理するクラス
/// </summary>
public class DataManager : MonoBehaviour
{
    [Header("プレイヤーデータ")]
    [SerializeField]
    PlayerData m_playerData = default;

    [Header("人物データ")]
    [SerializeField]
    AllCharacterData m_allCharacterData = default;

    [Header("クイズデータ")]
    [SerializeField]
    AllQuizData m_allQuizData = default;

    [Header("シナリオデータ")]
    [SerializeField]
    AllScenarioData m_allScenarioData = default;

    delegate void LoadDataCallback<T>(T data);

    public static DataManager Instance { get; private set; }

    /// <summary> プレイヤーのデータを取得する </summary>
    public PlayerData PlayerData => m_playerData;
    /// <summary> 現在の時代の村人のデータを取得する </summary>
    public CharacterData[] CurrentPeriodAllVillegersData => m_allCharacterData.GetCurrentPeriodVillagersData;
    /// <summary> 現在の時代の偉人データを取得する </summary>
    public CharacterData CurrentPeriodHistoricalFigures => m_allCharacterData.GetCurrentHistoricalFiguresData;
    /// <summary> 4択クイズのデータ </summary>
    public FourChoicesQuizData[] FourChoicesQuizDatas => m_allQuizData.FourChoicesQuizDatas;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// スプレッドシートからデータを取得するためにManagerをセットする
    /// </summary>
    public static void GetDataManager()
    {
        Instance = GameObject.Find("DataManager").GetComponent<DataManager>();
    }
    /// <summary>
    /// スプレッドシートからシナリオデータをロードする。※この関数はEditor上でのみ使用する関数なので、ゲーム中に実行されるクラスでは使わないでください。
    /// </summary>
    /// <param name="url"> スプレッドシートのURL </param>
    /// <param name="sheetName"> シート名 </param>
    public void LoadDialogDataFromSpreadsheet(string url, string sheetName)
    {
        for (int i = 0; i < m_allScenarioData.AllScenarioDatas.Length; i++)
        {
            if (m_allScenarioData.AllScenarioDatas[i].ScenarioSheetName == sheetName)  //プロパティとシート名が一致したら
            {
                LoadMasterData(url, sheetName, (ScenarioMasterDataClass<DialogData> data) =>
                {
                    m_allScenarioData.AllScenarioDatas[i].DialogData = data.Data;  //データ更新

                    for (int n = 0; n < m_allScenarioData.AllScenarioDatas[i].DialogData.Length; n++)
                    {
                        m_allScenarioData.AllScenarioDatas[i].DialogData[n].MessagesAndFacetypeToArray();
                    }
                });
                return;
            }
        }
        //データがロードできなかった場合
        Debug.LogError("データをロードできませんでした");
    }

    /// <summary>
    /// スプレッドシートからシナリオの選択肢データをロードする。※この関数はEditor上でのみ使用する関数なので、ゲーム中に実行されるクラスでは使わないでください。
    /// </summary>
    /// <param name="url"> スプレッドシートのURL </param>
    /// <param name="sheetName"> シート名 </param>
    public void LoadDialogChoicesDataFromSpreadsheet(string url, string sheetName)
    {
        for (int i = 0; i < m_allScenarioData.AllScenarioDatas.Length; i++)
        {
            if (m_allScenarioData.AllScenarioDatas[i].ScenarioSheetName == sheetName)  //プロパティとシート名が一致したら
            {
                LoadMasterData(url, sheetName, (ScenarioMasterDataClass<ChoicesData> data) =>
                {
                    m_allScenarioData.AllScenarioDatas[i].ChoicesDatas = data.Data;  //データ更新
                });
                return;
            }
        }
        //データがロードできなかった場合
        Debug.LogError("データをロードできませんでした");
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
                LoadMasterData(url, sheetName, (QuizMasterDataClass<FourChoicesQuiz> data) =>
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
    void LoadMasterData<T>(string url, string file, LoadDataCallback<T> callback)
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