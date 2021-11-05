using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DropText : MonoBehaviour, IDropHandler
{
    //[NonSerialized]
    public string m_correct;
    public string m_text = "";
    private Text m_viewText;

    void Start()
    {
        m_viewText = transform.GetChild(0).gameObject.GetComponent<Text>();
        SetText();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (m_correct == eventData.pointerDrag.GetComponent<DragText>().m_text)
        {
            m_text = eventData.pointerDrag.GetComponent<DragText>().m_text;
            SetText();
        }
    }

    private void SetText()
    {
        m_viewText.text = m_text;
    }

    public void SetText(string text)
    {
        m_text = text;
    }
}
