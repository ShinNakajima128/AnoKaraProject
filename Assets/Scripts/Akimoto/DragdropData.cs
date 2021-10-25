using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DragdropData : ScriptableObject
{
    public List<DragDropDataBase> m_database = new List<DragDropDataBase>();
}

[System.Serializable]
public class DragDropDataBase
{
    /// <summary>目的</summary>
    [SerializeField]
    public string m_objective;

    /// <summary>ドロップされる対象の画像</summary>
    public Sprite m_dropImage;

    /// <summary>ドラッグする画像</summary>
    public Sprite[] m_dragImage;

    /// <summary>下部のテキスト</summary>
    public string[] m_texts;
}