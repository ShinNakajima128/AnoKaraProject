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

    /// <summary>動かせるやつら</summary>
    [SerializeField]
    DragItem[] m_dragitem;

    /// <summary>データベース</summary>
    [SerializeField]
    DragdropData m_data;

    [SerializeField]
    int m_datanum = 0;

    private DragDropDataBase m_database;

    void Start()
    {
        m_database = m_data.m_database[m_datanum];
        m_objectiveText.text = m_database.GetObjective;
        CreateDragObj();
    }

    private void CreateDragObj()
    {
        for (int i = 0; i < 3; i++)
        {
            // この辺汎用化出来てないです
            DragItem item = m_dragitem[i];
            item.num = i;

            //int r = Random.Range(0, 3); //ランダム
            //obj.GetComponent<DragItem>().num = numList[r];
        }
        //for (int i = 0; i < m_dragitem.Length; i++)
        //{
        //    m_dragitem[i].GetComponent<Image>().sprite = m_database.m_dragImage[i];
        //    m_dragitem[i].GetComponent<DragItem>().num = 0;
        //}
    }

    public void VIewTextChange(int index)
    {
        if (index == m_database.m_dragitems.Length)
        {
            m_viewText.text = "クリア";
            return;
        }
        m_viewText.text = m_database.m_dragitems[index].GetText;
    }
}
