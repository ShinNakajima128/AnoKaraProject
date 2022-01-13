using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchManager : MonoBehaviour
{
    CharacterData[] m_villegersData;

    [Header("タスク終了時にシナリオを表示するまでの時間")]
    [SerializeField]
    float m_finishTaskScenarioTimer = 0.5f;

    /// <summary>全時代のCanvas<summary>
    [SerializeField]
    GameObject[] m_jidaiCanvas;

    /// <summary>全ステージのPanel<summary>
    [SerializeField]
    StageData[] m_stagePanel;

    /// <summary> シナリオ終了までスクロール等を不可にするPanel </summary>
    [SerializeField]
    GameObject m_InoperablePanel = default;

    #region sheetName
    [Header("タスク終了後のシナリオシート名")]
    [SerializeField]
    string m_jomon_yayoi_sheetName01 = default;

    [SerializeField]
    string m_asuka_nara_sheetName01 = default;

    [SerializeField]
    string m_asuka_nara_sheetName02 = default;

    [SerializeField]
    string m_heian_sheetName01 = default;

    [SerializeField]
    string m_heian_sheetName02 = default;

    [SerializeField]
    string m_heian_sheetName03 = default;

    [SerializeField]
    string m_kamakura_sheetName01 = default;

    [SerializeField]
    string m_kamakura_sheetName02 = default;

    [SerializeField]
    string m_kamakura_sheetName03 = default;

    [SerializeField]
    string m_kamakura_sheetName04 = default;

    [SerializeField]
    string m_momoyama_sheetName01 = default;

    [SerializeField]
    string m_momoyama_sheetName02 = default;

    [SerializeField]
    string m_momoyama_sheetName03 = default;

    [SerializeField]
    string m_edo_sheetName01 = default;

    [SerializeField]
    string m_edo_sheetName02 = default;

    [SerializeField]
    string m_edo_sheetName03 = default;

    [SerializeField]
    string m_edo_sheetName04 = default;

    #endregion

    /// <summary>終わったタスク数<summary>
    int m_maxTaskNum = 0;

    /// <summary>ステージごとのタスク数<summary>
    public int CurrentTaskNum { get; set; } = 0;

    public bool IsTaskComplited { get; set; } = false;

    public static SearchManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_villegersData = new CharacterData[DataManager.Instance.CurrentPeriodAllVillegersData.Length];
        m_villegersData = DataManager.Instance.CurrentPeriodAllVillegersData;
        JidaiSelect();
        StageSelect();
        StartCoroutine(PlayEachStageFirstScenario());
        m_InoperablePanel.SetActive(true);
        EventManager.ListenEvents(Events.BeginTask, OnInoperablePanel);
        EventManager.ListenEvents(Events.FinishDialog, OnlyOnceMethod);
        SoundManager.Instance.PlayBgm(SoundManager.Instance.BgmName);

        //クイズ終了後の場合はシナリオ終了後、時代選択画面に戻る。最後のステージのクイズ後の場合はエンディングに遷移するイベントを登録する
        if (GameManager.Instance.CurrentPeriod == MasterData.PeriodTypes.Edo && GameManager.Instance.CurrentStageId == 2 && GameManager.Instance.IsAfterQuized)
        {
            EventManager.ListenEvents(Events.FinishDialog, () => 
            {
                LoadSceneManager.AnyLoadScene("Ending");
            });
        }
        else if (GameManager.Instance.IsAfterQuized)
        {
            EventManager.ListenEvents(Events.FinishDialog, StageClear);
        }
    }

    void OnlyOnceMethod()
    {
        OffInoperablePanel();
        EventManager.RemoveEvents(Events.FinishDialog, OnlyOnceMethod);
    }
    /// <summary>
    /// 選んだステージのCanvasを表示
    /// </summary>
    void JidaiSelect()
    {
        var jidai = GameManager.Instance.CurrentPeriod;
        switch (jidai)
        {
            case MasterData.PeriodTypes.None:
                Debug.Log("時代選択されてない");
                break;
            case MasterData.PeriodTypes.Jomon_Yayoi:
                for (int i = 0; i < m_jidaiCanvas.Length; i++)
                {
                    if (i == 0)
                    {
                        m_jidaiCanvas[i].SetActive(true);
                    }
                    else
                    {
                        m_jidaiCanvas[i].SetActive(false);
                    }
                }
                break;
            case MasterData.PeriodTypes.Asuka_Nara:
                for (int i = 0; i < m_jidaiCanvas.Length; i++)
                {
                    if (i == 1)
                    {
                        m_jidaiCanvas[i].SetActive(true);
                    }
                    else
                    {
                        m_jidaiCanvas[i].SetActive(false);
                    }
                }
                break;
            case MasterData.PeriodTypes.Heian:
                for (int i = 0; i < m_jidaiCanvas.Length; i++)
                {
                    if (i == 2)
                    {
                        m_jidaiCanvas[i].SetActive(true);
                    }
                    else
                    {
                        m_jidaiCanvas[i].SetActive(false);
                    }
                }
                break;
            case MasterData.PeriodTypes.Kamakura:
                for (int i = 0; i < m_jidaiCanvas.Length; i++)
                {
                    if (i == 3)
                    {
                        m_jidaiCanvas[i].SetActive(true);
                    }
                    else
                    {
                        m_jidaiCanvas[i].SetActive(false);
                    }
                }
                break;
            case MasterData.PeriodTypes.Momoyama:
                for (int i = 0; i < m_jidaiCanvas.Length; i++)
                {
                    if (i == 4)
                    {
                        m_jidaiCanvas[i].SetActive(true);
                    }
                    else
                    {
                        m_jidaiCanvas[i].SetActive(false);
                    }
                }
                break;
            case MasterData.PeriodTypes.Edo:
                for (int i = 0; i < m_jidaiCanvas.Length; i++)
                {
                    if (i == 5)
                    {
                        m_jidaiCanvas[i].SetActive(true);
                    }
                    else
                    {
                        m_jidaiCanvas[i].SetActive(false);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 選んだステージのPanelを表示
    /// </summary>
    void StageSelect()
    {
        //クイズ終了後のシナリオの場合はその時代の背景のみを表示する
        if (GameManager.Instance.IsAfterQuized)
        {
            return;
        }

        var jidai = GameManager.Instance.CurrentPeriod;

        var p = m_stagePanel.Where(a => a.PeriodType == jidai).ToArray();

        var stage = GameManager.Instance.CurrentStageId;

        var s = p.FirstOrDefault(b => b.StageId == stage);
        s.StagePanel.SetActive(true);
        m_maxTaskNum = p[stage].Task;
        Debug.Log($"このステージのタスク数：{m_maxTaskNum}");
    }

    /// <summary>
    /// buttonに設定　すべてのタスクが完了したか
    /// </summary>
    public void TaskCount()
    {
        StartCoroutine(WaitTask());
    }

    IEnumerator WaitTask()
    {
        while (!IsTaskComplited)
        {
            yield return null;
        }

        CurrentTaskNum++;
        EventManager.OnEvent(Events.TaskComplite);
        m_InoperablePanel.SetActive(false);
        Debug.Log(CurrentTaskNum);

        while (IsTaskComplited)
        {
            yield return null;
        }
        yield return new WaitForSeconds(m_finishTaskScenarioTimer);

        if (CurrentTaskNum >= m_maxTaskNum)
        {
            var a = ScenarioManager.Instance.Data;
            var stage = GameManager.Instance.CurrentStageId;

            switch (GameManager.Instance.CurrentPeriod)
            {
                case MasterData.PeriodTypes.Jomon_Yayoi:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_jomon_yayoi_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Asuka_Nara:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (stage == 0 && a[i].ScenarioSheetName == m_asuka_nara_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 1 && a[i].ScenarioSheetName == m_asuka_nara_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Heian:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (stage == 0 && a[i].ScenarioSheetName == m_heian_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 1 && a[i].ScenarioSheetName == m_heian_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 2 && a[i].ScenarioSheetName == m_heian_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Kamakura:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (stage == 0 && a[i].ScenarioSheetName == m_kamakura_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 1 && a[i].ScenarioSheetName == m_kamakura_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 2 && a[i].ScenarioSheetName == m_kamakura_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 3 && a[i].ScenarioSheetName == m_kamakura_sheetName04)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Momoyama:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (stage == 0 && a[i].ScenarioSheetName == m_momoyama_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 1 && a[i].ScenarioSheetName == m_momoyama_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 2 && a[i].ScenarioSheetName == m_momoyama_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Edo:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (stage == 0 && a[i].ScenarioSheetName == m_edo_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 1 && a[i].ScenarioSheetName == m_edo_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 2 && a[i].ScenarioSheetName == m_edo_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                        else if (stage == 3 && a[i].ScenarioSheetName == m_edo_sheetName04)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            EventManager.ListenEvents(Events.FinishDialog, () =>
            {
                LoadSceneManager.AnyLoadScene("QuizPart");
                //LoadSceneManager.AnyLoadScene("PeriodSelect", () =>
                //{
                //    DataManager.Instance.FlagOpen((int)GameManager.Instance.CurrentPeriod, GameManager.Instance.CurrentStageId);
                //});
            });
            Debug.Log("全てのタスクが終了しました");    
        }
    }

    /// <summary>
    /// 各時代、各ステージの最初のシナリオを開始する
    /// </summary>
    IEnumerator PlayEachStageFirstScenario()
    {
        yield return null;
        switch (GameManager.Instance.CurrentPeriod)
        {
            case MasterData.PeriodTypes.Jomon_Yayoi:

                if (!GameManager.Instance.IsAfterQuized)
                {
                    ScenarioManager.Instance.StartSelectScenario(0);
                }
                else
                {
                    ScenarioManager.Instance.StartSelectScenario(8);
                }

                break;
            case MasterData.PeriodTypes.Asuka_Nara:

                if (GameManager.Instance.CurrentStageId == 0)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(9);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(14);
                    }
                }
                else
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(15);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(20);
                    }
                }
                break;
            case MasterData.PeriodTypes.Heian:
                if (GameManager.Instance.CurrentStageId == 0)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(21);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(26);
                    }
                }
                else if (GameManager.Instance.CurrentStageId == 1)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(27);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(32);
                    }
                }
                else
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(33);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(38);
                    }
                }
                break;
            case MasterData.PeriodTypes.Kamakura:
                if (GameManager.Instance.CurrentStageId == 0)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(39);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(42);
                    }
                }
                else if (GameManager.Instance.CurrentStageId == 1)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(43);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(47);
                    }
                }
                else if (GameManager.Instance.CurrentStageId == 2)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(48);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(52);
                    }
                }
                else
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(53);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(58);
                    }
                }
                break;
            case MasterData.PeriodTypes.Momoyama:
                if (GameManager.Instance.CurrentStageId == 0)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(59);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(65);
                    }
                }
                else if (GameManager.Instance.CurrentStageId == 1)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(66);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(71);
                    }
                }
                else
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(72);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(77);
                    }
                }
                break;
            case MasterData.PeriodTypes.Edo:
                if (GameManager.Instance.CurrentStageId == 0)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(78);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(83);
                    }
                }
                else if (GameManager.Instance.CurrentStageId == 1)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(84);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(89);
                    }
                }
                else if (GameManager.Instance.CurrentStageId == 2)
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(90);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(96);
                    }
                }
                else
                {
                    if (!GameManager.Instance.IsAfterQuized)
                    {
                        ScenarioManager.Instance.StartSelectScenario(96);
                    }
                    else
                    {
                        ScenarioManager.Instance.StartSelectScenario(100);
                    }
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ステージクリアした場合、シナリオ終了後に時代選択画面に遷移させる
    /// </summary>
    void StageClear()
    {
        GameManager.Instance.IsAfterQuized = false;

        LoadSceneManager.AnyLoadScene("PeriodSelect");
    }

    /// <summary>
    /// 操作制限用のパネルをONにする
    /// </summary>
    void OnInoperablePanel()
    {
        m_InoperablePanel.SetActive(true);
    }

    void OffInoperablePanel()
    {
        m_InoperablePanel.SetActive(false);
    }
}
     
