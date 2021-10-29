using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragdropMinigameManager : MonoBehaviour
{
    /// <summary>上部テキスト</summary>
    [SerializeField]
    Text m_objectiveText;

    /// <summary>下部テキスト</summary>
    [SerializeField]
    Text m_viewText;

    [SerializeField]
    Image m_dropItem;

    /// <summary>動かせるやつら</summary>
    [SerializeField]
    GameObject[] m_dragitem;

    /// <summary>データベース</summary>
    [SerializeField]
    DragdropData m_data;

    /// <summary>問題データ</summary>
    public int m_datanum = 0;

    private DragDropDataBase m_database;

    private List<int> m_list = new List<int>();

    void Start()
    {
        m_database = m_data.m_database[m_datanum];
        m_dropItem.sprite = m_database.DropImage;
        m_objectiveText.text = m_database.Objective;
        CreateDragObj();
    }

    /// <summary>
    /// 被りなしでランダムでデータ数
    /// </summary>
    private void CreateDragObj()
    {
        for (int i = 0; i < m_database.m_dragitems.Length; i++)
        {
            m_list.Add(i);
        }
        for (int i = 0; m_list.Count > 0; i++)
        {
            int index = Random.Range(0, m_list.Count);
            int num = m_list[index];
            GameObject obj = m_dragitem[i];
            DragItem item = obj.GetComponent<DragItem>();
            item.num = num;
            obj.GetComponent<Image>().sprite = m_database.DropImage;
            m_list.RemoveAt(index);
        }
    }

    public void VIewTextChange(int index)
    {
        if (index == m_database.m_dragitems.Length)
        {
            m_viewText.text = m_database.CrearText;
            return;
        }
        m_viewText.text = m_database.m_dragitems[index].Text;
    }
}
