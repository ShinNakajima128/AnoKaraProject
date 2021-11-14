using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TitleStates
{
    Start,
    SelectGender,
    InputName,
    FinalConfirm
}

public class TitleManager : MonoBehaviour
{
    
    [SerializeField]
    TitleStates m_titleState = TitleStates.Start;

    [Header("タイトル画面の各Panel")]
    [SerializeField]
    GameObject m_startPanel = default;

    [SerializeField]
    GameObject m_initialSettingPanel = default;

    [SerializeField]
    GameObject[] m_settingPanels = default;

    [SerializeField]
    GameDataObject gameDataObject = default;

    PlayerData m_playerData = default;

    public static TitleManager Instance { get; private set; }

    public string TempPlayerName { get; set; }
    public GenderType TempGender { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangePanel(TitleStates.Start);
    }

    /// <summary>
    /// Panelを切り替える
    /// </summary>
    /// <param name="States"> 切り替えるPanelの索引 </param>
    public void ChangePanel(TitleStates States)
    {
        m_titleState = States;

        switch (m_titleState)
        {
            case TitleStates.Start:
                m_startPanel.SetActive(true);
                m_initialSettingPanel.SetActive(false);
                break;
            case TitleStates.SelectGender:
                m_startPanel.SetActive(false);
                m_initialSettingPanel.SetActive(true);
                for (int i = 0; i < m_settingPanels.Length; i++)
                {
                    if (i == 0)
                    {
                        m_settingPanels[i].SetActive(true);
                    }
                    else
                    {
                        m_settingPanels[i].SetActive(false);
                    }
                }
                break;
            case TitleStates.InputName:
                m_startPanel.SetActive(false);
                m_initialSettingPanel.SetActive(true);
                for (int i = 0; i < m_settingPanels.Length; i++)
                {
                    if (i == 1)
                    {
                        m_settingPanels[i].SetActive(true);
                    }
                    else
                    {
                        m_settingPanels[i].SetActive(false);
                    }
                }
                break;
            case TitleStates.FinalConfirm:

                break;
        }
    }


    public void ChangPanelByButton(int type)
    {

    }
    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void GameStart()
    {
        SaveAndLoadTest.Instance.LoadData();
        m_playerData = DataManager.Instance.PlayerData;

        //プレイヤーデータがあれば時代選択画面に遷移する
        if (m_playerData.PlayerName != "")
        {
            Debug.Log($"プレイヤー名：{m_playerData.PlayerName}、性別：{m_playerData.PlayerGender}");

            LoadSceneManager.AnyLoadScene("時代選択画面", () =>
            {
                Debug.Log("時代選択画面に遷移します");
            });
        }
        //プレイヤーデータが無ければ初期設定画面を表示
        else
        {
            ChangePanel(TitleStates.SelectGender);
        }
    }
}
