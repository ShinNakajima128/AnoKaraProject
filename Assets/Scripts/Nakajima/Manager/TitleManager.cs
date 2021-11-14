﻿using System.Collections;
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
    [Header("デバッグ用")]
    [SerializeField]
    TitleStates m_titleState = TitleStates.Start;

    [SerializeField]
    GenderType m_tempGender = default;

    [SerializeField]
    string m_tempName = default;

    [Header("タイトル画面の各Panel")]
    [SerializeField]
    GameObject m_startPanel = default;

    [SerializeField]
    GameObject m_initialSettingPanel = default;

    [SerializeField]
    GameObject[] m_settingPanels = default;

    [SerializeField]
    Image m_tempGenderImage = default;

    [SerializeField]
    Text m_tempPlayerName = default;

    [SerializeField]
    GameDataObject gameDataObject = default;

    PlayerData m_playerData = default;

    public static TitleManager Instance { get; private set; }

    public string TempPlayerName { get => m_tempName; set => m_tempName = value; }
    public GenderType TempGender { get=> m_tempGender; set => m_tempGender = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangePanel(TitleStates.Start);
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void GameStart()
    {
        DataManager.LoadData();
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

    /// <summary>
    /// Panelを切り替える
    /// </summary>
    /// <param name="States"> 切り替えるPanelの索引 </param>
    public void ChangePanel(TitleStates States)
    {
        m_titleState = States;

        switch (m_titleState)
        {
            //タイトル画面表示
            case TitleStates.Start:
                m_startPanel.SetActive(true);
                m_initialSettingPanel.SetActive(false);
                break;
            //性別選択画面表示
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
            //名前入力画面表示
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
                m_startPanel.SetActive(false);
                m_initialSettingPanel.SetActive(true);
                for (int i = 0; i < m_settingPanels.Length; i++)
                {
                    if (i == 2)
                    {
                        m_settingPanels[i].SetActive(true);
                    }
                    else
                    {
                        m_settingPanels[i].SetActive(false);
                    }
                }

                m_tempPlayerName.text = $"プレイヤー名\n{m_tempName}";
                m_tempGenderImage.sprite = m_tempGender == GenderType.Boy ? DataManager.Instance.PlayerData.BoyImages[0] : DataManager.Instance.PlayerData.GirlImages[0];
                break;
        }
    }


    public void ChangePanelByButton(int type)
    {
        ChangePanel((TitleStates)type);
    }

    
}