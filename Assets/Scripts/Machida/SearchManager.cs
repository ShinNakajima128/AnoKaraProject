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

    /// <summary>終わったタスク数<summary>
    int m_maxTaskNum = 0;

    /// <summary>ステージごとのタスク数<summary>
    public int CurrentTaskNum { get; set; } = 0;

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
            case MasterData.PeriodTypes.Asuka:
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
            case MasterData.PeriodTypes.Sengoku:
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
        CurrentTaskNum++;

        if (CurrentTaskNum >= m_maxTaskNum)
        {
            Debug.Log("全てのタスクが終了しました");
            DataManager.Instance.FlagOpen((int)GameManager.Instance.CurrentPeriod, (int)GameManager.Instance.CurrentStageId);
            LoadSceneManager.AnyLoadScene("PeriodSelect");
        }
    }
}
