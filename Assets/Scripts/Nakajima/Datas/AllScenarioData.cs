using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シナリオデータを管理するクラス
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/Create AllScenarioData")]
public class AllScenarioData : ScriptableObject
{
    [Header("全てのシナリオのデータ")]
    [SerializeField]
    ScenarioData[] m_allScenarioDatas = default;

    public ScenarioData[] AllScenarioDatas { get => m_allScenarioDatas; set => m_allScenarioDatas = value; }
}
