using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FourChoicesQuizData))]
public class FourChoicesQuizDataCustomInspector : Editor
{
    FourChoicesQuizData m_fourChoicesQuizData;
    
    private void OnEnable()
    {
        m_fourChoicesQuizData = target as FourChoicesQuizData;
    }

    public override void OnInspectorGUI()
    {
        if (!m_fourChoicesQuizData) return;
        
        if (GUILayout.Button("Update"))
        {
            QuizDataManager.Instance.LoadDataFromSpreadsheet(m_fourChoicesQuizData.PeriodName);
        }
        base.OnInspectorGUI();
        EditorUtility.SetDirty(m_fourChoicesQuizData);
    }
}
