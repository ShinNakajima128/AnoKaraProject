using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "MyScriptable/Create AllCharacterData")]
public class AllCharacterData : ScriptableObject
{
    [Header("村人のデータ")]
    [SerializeField]
    CharacterData[] m_villagersData = default;

    [Header("偉人のデータ")]
    [SerializeField]
    CharacterData[] m_historicalFiguresData = default;

    public CharacterData[] VillagersData => m_villagersData;

    /// <summary> 現在時代の村人データを取得する </summary>
    public CharacterData[] GetCurrentPeriodVillagersData
    {
        get
        {
            List<CharacterData> villagerList = new List<CharacterData>();

            villagerList = m_villagersData.Where(v => v.CharacterPeriod == GameManager.Instance.CurrentPeriod).ToList();
            return villagerList.ToArray();
        }
    }

    /// <summary> 現在の時代の偉人データを取得する </summary>
    public CharacterData GetCurrentHistoricalFiguresData 
    { 
        get 
        {
            if (GameManager.Instance.CurrentPeriod == MasterData.PeriodTypes.None) return m_historicalFiguresData[0];
            CharacterData hf = default;
            hf = m_historicalFiguresData.Where(h => h.CharacterPeriod == GameManager.Instance.CurrentPeriod)
                                        .FirstOrDefault(s => s.StageId == GameManager.Instance.CurrentStageId);

            if (hf != null)
            {
                return hf;
            }
            else
            {
                Debug.Log("データが見つかりませんでした");
                return default;
            }
        } 
    }
}
