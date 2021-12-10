using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchManager : MonoBehaviour
{
    CharacterData[] m_villegersData;

    /// <summary>全時代のCanvas<summary>
    [SerializeField]
    GameObject[] m_jidaiCanvas;

    /// <summary>全ステージのPanel<summary>
    [SerializeField]
    StageData[] m_stagePanel;

    #region sheetName

    [SerializeField]
    string m_jomon_yayoi_sheetName01 = default;

    [SerializeField]
    string m_jomon_yayoi_sheetName02 = default;

    [SerializeField]
    string m_jomon_yayoi_sheetName03 = default;

    [SerializeField]
    string m_asuka_nara_sheetName01 = default;

    [SerializeField]
    string m_asuka_nara_sheetName02 = default;

    [SerializeField]
    string m_asuka_nara_sheetName03 = default;

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
        IsTaskComplited = false;

        if (CurrentTaskNum >= m_maxTaskNum)
        {
            var a = ScenarioManager.Instance.Data;

            switch (GameManager.Instance.CurrentPeriod)
            {
                case MasterData.PeriodTypes.Jomon_Yayoi:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_jomon_yayoi_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_jomon_yayoi_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_jomon_yayoi_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Asuka_Nara:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_asuka_nara_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_asuka_nara_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_asuka_nara_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Heian:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_heian_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_heian_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_heian_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Kamakura:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_kamakura_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_kamakura_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_kamakura_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Momoyama:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_momoyama_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_momoyama_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_momoyama_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                case MasterData.PeriodTypes.Edo:
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i].ScenarioSheetName == m_edo_sheetName01)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_edo_sheetName02)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }

                        if (a[i].ScenarioSheetName == m_edo_sheetName03)
                        {
                            ScenarioManager.Instance.StartSelectScenario(i);
                        }
                    }
                    break;
                default:
                    break;
            }

            ScenarioManager.Instance.EndEvent.AddListener(() =>
            {
                LoadSceneManager.AnyLoadScene("PeriodSelect", () =>
                {
                    DataManager.Instance.FlagOpen((int)GameManager.Instance.CurrentPeriod, GameManager.Instance.CurrentStageId);
                });
            });
            Debug.Log("全てのタスクが終了しました");    
        }
    }
}
