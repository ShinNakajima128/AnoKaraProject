using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectionDatabase")]
public class CollectionData : ScriptableObject
{
    public List<CollectionDataBase> m_dataBases = new List<CollectionDataBase>();
}

[System.Serializable]
public class CollectionDataBase
{
    /// <summary>コレクション名</summary>
    [SerializeField]
    private string m_name;

    /// <summary>コレクション説明</summary>
    [SerializeField]
    private string m_tooltip;

    /// <summary>獲得フラグ</summary>
    private bool m_isGet = false;

    public string Name { get { return m_name; } }
    public string Tooltip { get { return m_tooltip; } }
    public bool IsGet
    {
        get
        {
            return m_isGet;
        }
        set
        {
            m_isGet = value;
        }
    }
}