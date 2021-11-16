using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineConnectionQuizData))]
public class LineConnectionQuizDataCustominspector : Editor
{
    LineConnectionQuizData m_lineconnectionQuizData;

    private void OnEnable()
    {
        m_lineconnectionQuizData = target as LineConnectionQuizData;
    }

    public override void OnInspectorGUI()
    {
        if (!m_lineconnectionQuizData) return;

        if (GUILayout.Button("Update"))
        {
            if (DataManager.Instance == null)
            {
                DataManager.GetDataManager();
            }
            //作業が完了したらコメントアウトしてテストしてください
            //DataManager.Instance.LoadFourChoicesQuizDataFromSpreadsheet(m_anaumeQuizData.URL, m_anaumeQuizData.PeriodTypeName);
            DataManager.Instance.LoadFourLineConnectionQuizDataFromSpreadsheet(m_lineconnectionQuizData.URL, m_lineconnectionQuizData.PeriodTypeName);
        }
        base.OnInspectorGUI();
        EditorUtility.SetDirty(m_lineconnectionQuizData);
    }
}
