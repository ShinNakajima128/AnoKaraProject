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
    string m_objective;

    /// <summary>ドロップされる対象の画像</summary>
    [SerializeField]
    Sprite m_dropImage;

    [System.Serializable]
    public class DragItemSettings
    {
        /// <summary>ドラッグする画像</summary>
        [SerializeField]
        Sprite m_dragImage;

        /// <summary>下部のテキスト</summary>
        [SerializeField] string m_text;

        public Sprite DragImage { get { return m_dragImage; } }
        public string Text { get { return m_text; } }
    }
    /// <summary>ドラッグするアイテム達の画像やドロップ後の下部テキスト</summary>
    public DragItemSettings[] m_dragitems;

    [SerializeField]
    string m_crearText;

    public string Objective { get { return m_objective; } }
    public Sprite DropImage { get { return m_dropImage; } }
    public string CrearText { get { return m_crearText; } }
}