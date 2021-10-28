using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour, IDropHandler
{
    //ドロップ=>正誤判定=>合ってたらオブジェクトを消してfaze++、間違ってたら何も起こらない
    [SerializeField]
    DragdropMinigameManager m_gamemanager;

    private int[] m_clearNum = { 0, 1, 2 };

    private int m_faze = 0;

    private void Start()
    {
        SetText();
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragItem item = eventData.pointerDrag.GetComponent<DragItem>();
        if (item.num == m_clearNum[m_faze])
        {
            item.gameObject.SetActive(false);
            m_faze++;
            SetText();
        }
    }

    private void SetText()
    {
        m_gamemanager.VIewTextChange(m_faze);
    }
}
