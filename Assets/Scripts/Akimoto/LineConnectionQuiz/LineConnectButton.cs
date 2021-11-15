using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineConnectButton : MonoBehaviour
{
    /// <summary>テキスト</summary>
    public string m_text;
    private Text m_viewText;

    /// <summary>自分が左右どちらに属しているか<br/>左が０、右が１</summary>
    public int Side { get; set; }

    void Start()
    {
        //SetText();
    }

    public void SetText()
    {
        m_viewText = transform.GetChild(1).GetComponent<Text>();
        m_viewText.text = m_text;
    }

    /// <summary>
    /// 押された事を受け取る
    /// </summary>
    public void SetConnection()
    {
        if (LineConnectionQuizManager.Instance.IsConnected)
        {
            LineConnectionQuizManager.Instance.IsConnected = false;
            LineConnectionQuizManager.Instance.EndLine(transform.GetChild(0).position, Side);
            LineConnectionQuizManager.Instance.LineCast();
        }
        else
        {
            LineConnectionQuizManager.Instance.IsConnected = true;
            LineConnectionQuizManager.Instance.StartLine(transform.GetChild(0).position, Side);
        }
    }
}
