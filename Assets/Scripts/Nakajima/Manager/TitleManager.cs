using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum TitleStates
{
    Start,
    SelectGender,
    InputName,
    FinalConfirm
}

public class TitleManager : MonoBehaviour
{
    [Header("オープニングのScene名")]
    [SerializeField]
    string m_openingSceneName = default;

    [Header("時代選択画面のScene名")]
    [SerializeField]
    string m_periodSelectSceneName = default;

    [Header("データのリセットフラグ")]
    [Header("デバッグ用")]
    [SerializeField]
    bool m_resetGameData = false;

    [Header("タイトル画面の状態")]
    [SerializeField]
    TitleStates m_titleState = TitleStates.Start;

    [Header("設定中の性別")]
    [SerializeField]
    GenderType m_tempGender = default;

    [Header("設定中の名前")]
    [SerializeField]
    string m_tempName = default;

    /// <summary> スタート画面のPanel </summary>
    [Header("タイトル画面の各Panel")]
    [SerializeField]
    GameObject m_startPanel = default;

    /// <summary> 初期設定画面 </summary>
    [SerializeField]
    GameObject m_initialSettingPanel = default;

    /// <summary> 初期設定中の各設定画面 </summary>
    [SerializeField]
    GameObject[] m_settingPanels = default;

    /// <summary> 設定中の性別のImage </summary>
    [SerializeField]
    Image m_tempGenderImage = default;

    /// <summary> 設定中のプレイヤー名 </summary>
    [SerializeField]
    Text m_tempPlayerName = default;

    /// <summary>  </summary>
    GameDataObject m_gameDataObject = default;

    PlayerData m_playerData = default;

    GameManager.ClearFlagArray[] m_tempArray = default;
    StageAchieves[] m_tempStageAchieves = default;


    /// <summary>出てくるまでの時間</summary>
    [SerializeField]
    float m_fadeTime = 0.5f;

    public static TitleManager Instance { get; private set; }

    public bool IsAnim { get; set; } = false;
    public string TempPlayerName { get => m_tempName; set => m_tempName = value; }
    public GenderType TempGender { get => m_tempGender; set => m_tempGender = value; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_gameDataObject = FindObjectOfType<GameDataObject>();
        if (m_resetGameData)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("データをリセットしました");
        }
        m_tempArray = m_gameDataObject.ClearFlag;
        m_tempStageAchieves = m_gameDataObject.AllStageAchieves;
        m_gameDataObject.SetUp();
       
        ChangePanel(TitleStates.Start);
        SoundManager.Instance.PlayBgm(SoundManager.Instance.BgmName);
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void GameStart()
    {
        DataManager.LoadData();
        m_gameDataObject.UpdatePlayerData();
        m_playerData = DataManager.Instance.PlayerData;
        StartCoroutine(DataCheck());
        SoundManager.Instance.PlaySe("SE_title");
    }

    private IEnumerator TapScreenTextAnim()
    {
        while (!IsAnim)
        {
            yield return null;
        }
    }

    private IEnumerator DataCheck()
    {
        //ニューゲームであれば初期設定画面を表示
        if (m_gameDataObject.FirstPlay)
        {
            yield return StartCoroutine(TapScreenTextAnim());
            ChangePanel(TitleStates.SelectGender);

        }
        //つづきからの場合は時代選択画面に遷移する
        else
        {
            Debug.Log($"プレイヤー名：{m_playerData.PlayerName}、性別：{m_playerData.PlayerGender}");
            yield return StartCoroutine(TapScreenTextAnim());
            LoadSceneManager.AnyLoadScene(m_periodSelectSceneName, () =>
            {
                Debug.Log("時代選択画面に遷移します");
            });
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
                //SoundManager.Instance.PlaySe("SE_popup");
                RectTransform rect = m_initialSettingPanel.GetComponent<RectTransform>();
                rect.localScale = Vector3.zero;
                rect.DOScale(Vector3.one, m_fadeTime);
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
                //SoundManager.Instance.PlaySe("SE_popup");
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
                //SoundManager.Instance.PlaySe("SE_popup");
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

    /// <summary>
    /// オープニングSceneに遷移する
    /// </summary>
    public void LoadOpeningScene()
    {
        m_gameDataObject.FirstPlay = false;
        m_gameDataObject.PlayerName = m_tempName;
        m_gameDataObject.PlayerGender = m_tempGender;
        m_gameDataObject.ClearFlag = m_tempArray;
        m_gameDataObject.AllStageAchieves = m_tempStageAchieves;

        DataManager.SaveData();
        m_gameDataObject.UpdatePlayerData();
        LoadSceneManager.AnyLoadScene(m_openingSceneName);
        SoundManager.Instance.PlaySe("SE_title");
    }

    /// <summary>
    /// ゲームデータをリセットする
    /// </summary>
    public void ResetGameData()
    {
        m_gameDataObject.PlayerName = "";
        m_gameDataObject.PlayerGender = default;
        for (int i = 0; i < m_gameDataObject.ClearFlag.Length; i++)
        {
            for (int n = 0; n < m_gameDataObject.ClearFlag[i].m_stageClearFlag.Length; n++)
            {
                if (i == 0)
                {
                    if (i == 0 || i == 1)
                    {
                        m_gameDataObject.ClearFlag[i].m_stageClearFlag[n] = true;
                    }
                }
                else
                {
                    m_gameDataObject.ClearFlag[i].m_stageClearFlag[n] = false;
                }
            }
        }
        DataManager.SaveData();
    }
}
