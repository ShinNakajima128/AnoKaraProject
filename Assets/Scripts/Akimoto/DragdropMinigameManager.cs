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
    GameObject[] m_dragitem;

    /// <summary>データベース</summary>
    [SerializeField]
    DragdropData m_data;

    /// <summary>使うデータ</summary>
    [SerializeField]
    int m_selectData = 0;

    private DragDropDataBase m_database;

    void Start()
    {
        m_database = m_data.m_database[m_selectData];
        m_objectiveText.text = m_database.m_objective;
        CreateDragObj();
    }

    private void CreateDragObj()
    {
        int[] rnums = new int[3];
        for (int i = 0; i < 2; i++)
        {
            int r = Random.Range(0, 3); //ランダム
            if (i == 0)
            {
                rnums[r] = 1;
            }
            else
            {
                rnums[r] = 2;
            }
        }
        for (int i = 0; i < m_dragitem.Length; i++)
        {
            m_dragitem[i].GetComponent<Image>().sprite = m_database.m_dragImage[i];
            m_dragitem[i].GetComponent<DragItem>().num = 0;
        }
    }
}
