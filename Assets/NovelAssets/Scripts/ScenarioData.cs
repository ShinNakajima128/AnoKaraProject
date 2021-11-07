using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioMasterData;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "MyScriptable/Create ScenarioData")]
public class ScenarioData : ScriptableObject
{
    [SerializeField]
    string m_spreadSheetURL = default;

    [Header("シナリオのシート名")]
    [SerializeField]
    string m_scenarioSheetName = default;

    [Header("選択肢のスプレッドシート名")]
    [SerializeField]
    string m_choicesSheetName = default;

    [SerializeField]
    DialogData[] m_dialogData = default;

    [SerializeField]
    ChoicesData[] m_choicesDatas = default;

    [SerializeField]
    int m_backgroundType = default;

    delegate void ScenarioDataCallback<T>(T data);

    public string URL => m_spreadSheetURL;
    public string ScenarioSheetName => m_scenarioSheetName;
    public string ChoicesSheetName => m_choicesSheetName;
    public DialogData[] DialogData { get => m_dialogData; set => m_dialogData = value; }
    public ChoicesData[] ChoicesDatas { get => m_choicesDatas; set => m_choicesDatas = value; }
    public int BackgroundType { get => m_backgroundType; set => m_backgroundType = value; }

    public void LoadDialogDataFromSpreadsheet()
    {
        LoadDialogMasterData(m_scenarioSheetName, (ScenarioMasterDataClass<DialogData> data) =>
        {
            m_dialogData = data.Data;  //データ更新

            for (int n = 0; n < m_dialogData.Length; n++)
            {
                m_dialogData[n].MessagesAndFacetypeToArray();
            }
        });
    }

    public void LoadChoicesDataFromSpreadsheet()
    {
        LoadDialogMasterData(m_choicesSheetName, (ScenarioMasterDataClass<ChoicesData> data) =>
        {
            m_choicesDatas = data.Data;  //データ更新

            for (int n = 0; n < m_choicesDatas.Length; n++)
            {
                m_choicesDatas[n].MessagesAndNextIdToArray();
            }
        });
    }

    /// <summary>
    /// ダイアログデータを読み込む
    /// </summary>
    /// <typeparam name="T"> ダイアログデータのクラス </typeparam>
    /// <param name="file"> ダイアログ名 </param>
    /// <param name="callback"></param>
    void LoadDialogMasterData<T>(string file, ScenarioDataCallback<T> callback)
    {
        Network.WebRequest.Request<Network.WebRequest.GetString>("https://script.google.com/macros/s/AKfycbxkXM9so9l2drzNtbaSPIcMBJTV0_fScdRw-bVXREQdkJ8Vn1Tv/exec?sheet=" + file, Network.WebRequest.ResultType.String, (string json) =>
        {
            var dldata = JsonUtility.FromJson<T>(json);
            callback(dldata);
            Debug.Log("Network download. : " + file + " / " + json + "/" + dldata);
        });
    }
}
