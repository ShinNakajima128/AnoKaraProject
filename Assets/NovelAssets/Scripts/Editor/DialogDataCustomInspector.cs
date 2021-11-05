using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogData))]
public class DialogDataCustomInspector : Editor
{
    DialogData m_dialogData;

    private void OnEnable()
    {
        m_dialogData = target as DialogData;
    }

    public override void OnInspectorGUI()
    {
        if (!m_dialogData) return;

        if (GUILayout.Button("CharaUpdate"))
        {
            m_dialogData.LoadCharaDataFromSpreadsheet();
        }

        if (GUILayout.Button("ChoiceUpdate"))
        {
            m_dialogData.LoadChoicesDataFromSpreadsheet();
        }
        base.OnInspectorGUI();
        EditorUtility.SetDirty(m_dialogData);
    }
}
