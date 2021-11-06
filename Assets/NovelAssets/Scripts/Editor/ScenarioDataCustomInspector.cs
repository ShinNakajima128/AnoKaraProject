using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScenarioData))]
public class ScenarioDataCustomInspector : Editor
{
    ScenarioData m_ScenarioData;

    private void OnEnable()
    {
        m_ScenarioData = target as ScenarioData;
    }

    public override void OnInspectorGUI()
    {
        if (!m_ScenarioData) return;

        if (GUILayout.Button("DialogUpdate"))
        {
            if (DataManager.Instance == null)
            {
                DataManager.GetDataManager();
            }
            DataManager.Instance.LoadDialogDataFromSpreadsheet(m_ScenarioData.URL, m_ScenarioData.ScenarioSheetName);
        }

        if (GUILayout.Button("ChoicesUpdate"))
        {
            if (DataManager.Instance == null)
            {
                DataManager.GetDataManager();
            }
            DataManager.Instance.LoadDialogChoicesDataFromSpreadsheet(m_ScenarioData.URL, m_ScenarioData.ChoicesSheetName);
        }
        base.OnInspectorGUI();
        EditorUtility.SetDirty(m_ScenarioData);
    }
}
