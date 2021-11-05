using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogMasterData;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "MyScriptable/Create DialogData")]
public class DialogData : ScriptableObject
{
    [SerializeField]
    string m_scenarioSheetName = default;

    [SerializeField]
    string m_choicesSheetName = default;

    [SerializeField]
    CharacterData[] m_characterData = default;

    [SerializeField]
    ChoicesData[] m_choicesDatas = default;

    [SerializeField]
    int m_backgroundType = default;

    delegate void DialogDataCallback<T>(T data);

    public string ScenarioSheetName => m_scenarioSheetName;
    public string ChoicesSheetName => m_choicesSheetName;
    public CharacterData[] CharacterData { get => m_characterData; set => m_characterData = value; }
    public ChoicesData[] ChoicesDatas { get => m_choicesDatas; set => m_choicesDatas = value; }
    public int BackgroundType { get => m_backgroundType; set => m_backgroundType = value; }

    public void LoadCharaDataFromSpreadsheet()
    {
        LoadDialogMasterData(m_scenarioSheetName, (DialogMasterDataClass<CharacterData> data) =>
        {
            m_backgroundType = data.BGType;
            m_characterData = data.Data;  //データ更新

            for (int n = 0; n < m_characterData.Length; n++)
            {
                m_characterData[n].MessagesAndFacetypeToArray();
            }
        });
    }

    public void LoadChoicesDataFromSpreadsheet()
    {
        LoadDialogMasterData(m_choicesSheetName, (DialogMasterDataClass<ChoicesData> data) =>
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
    void LoadDialogMasterData<T>(string file, DialogDataCallback<T> callback)
    {
        Network.WebRequest.Request<Network.WebRequest.GetString>("https://script.google.com/macros/s/AKfycbxkXM9so9l2drzNtbaSPIcMBJTV0_fScdRw-bVXREQdkJ8Vn1Tv/exec?sheet=" + file, Network.WebRequest.ResultType.String, (string json) =>
        {
            var dldata = JsonUtility.FromJson<T>(json);
            callback(dldata);
            Debug.Log("Network download. : " + file + " / " + json + "/" + dldata);
        });
    }
}
