using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnaumeQuizData))]
public class AnaumeQuizDataCustomInspector : Editor
{
    AnaumeQuizData m_anaumeQuizData;

    private void OnEnable()
    {
        m_anaumeQuizData = target as AnaumeQuizData;
    }

    public override void OnInspectorGUI()
    {
        if (!m_anaumeQuizData) return;

        if (GUILayout.Button("Update"))
        {
            if (DataManager.Instance == null)
            {
                //DataManager.GetDataManager();
            }
            //作業が完了したらコメントアウトしてテストしてください
            //DataManager.Instance.LoadFourChoicesQuizDataFromSpreadsheet(m_anaumeQuizData.URL, m_anaumeQuizData.PeriodTypeName);
            DataManager.Instance.LoadFourAnaumeQuizDataFromSpreadsheet(m_anaumeQuizData.URL, m_anaumeQuizData.PeriodTypeName);
        }
        base.OnInspectorGUI();
        EditorUtility.SetDirty(m_anaumeQuizData);
    }
}
