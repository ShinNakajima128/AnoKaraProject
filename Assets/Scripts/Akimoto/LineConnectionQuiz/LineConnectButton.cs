using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnectButton : MonoBehaviour
{
    private RectTransform m_transform;

    /// <summary>自分が左右どちらに属しているか</summary>
    private LCQSide m_side;

    void Start()
    {
        m_transform = (RectTransform)transform.GetChild(0);
    }

    /// <summary>
    /// 押された事を受け取る
    /// </summary>
    public void SetConnection()
    {
        if (LineConnectionQuizManager.Instance.IsConnected)
        {
            //LineConnectionQuizManager.Instance.SetTransform(1, m_transform);
            LineConnectionQuizManager.Instance.IsConnected = false;
            //LineConnectionQuizManager.Instance.LineCast();
            LineConnectionQuizManager.Instance.EndLine(transform.GetChild(0).position);
            LineConnectionQuizManager.Instance.LineCast();
        }
        else
        {
            //LineConnectionQuizManager.Instance.SetTransform(0, m_transform);
            LineConnectionQuizManager.Instance.IsConnected = true;
            LineConnectionQuizManager.Instance.StartLine(transform.GetChild(0).position);
        }
    }
}
