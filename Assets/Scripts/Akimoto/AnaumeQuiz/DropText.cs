using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropText : MonoBehaviour
{
    /// <summary>所持テキスト</summary>
    [SerializeField]
    private string m_text;
    /// <summary>表示テキスト</summary>
    private Text m_viewText;
    
    public string Text { get => m_text; set => m_text = value; }

    void Start()
    {
        m_viewText = transform.GetChild(0).gameObject.GetComponent<Text>();
        m_viewText.text = "";
        //SetText();
    }

    //public void OnDrop(PointerEventData eventData)
    //{
    //    m_text = eventData.pointerDrag.GetComponent<DragText>().Text;
    //    SetText();
    //    //m_gameManager.CrearCheck();
    //}

    /// <summary>
    /// テキスト更新
    /// </summary>
    private void SetText()
    {
        m_viewText.text = m_text;
    }

    /// <summary>
    /// ドロップされた時
    /// </summary>
    /// <param name="text"></param>
    public void GetDrop(string text)
    {
        m_text = text;
        SetText();
    }

    /// <summary>
    /// 一致判定
    /// </summary>
    /// <returns></returns>
    //public bool CrearCheck()
    //{
    //    if (m_text == m_correctText) return true;
    //    else return false;
    //}
}
