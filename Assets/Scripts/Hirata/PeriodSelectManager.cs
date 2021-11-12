using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>時代選択画面マネージャー</summary>
public class PeriodSelectManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>ドラッグしているか否か</summary>
    bool m_isDrag = false;

    /// <summary>ドラッグ開始時のポインターポジション</summary>
    Vector2 m_pointerPosition;

    /// <summary>ドラッグ開始時のUIのポジション</summary>
    Vector2 m_startPosition;

    /// <summary>CanvasのRectTransform</summary>
    [SerializeField]
    RectTransform m_canvasRectTransform;

    /// <summary>PanelのRectTransform</summary>
    RectTransform m_panelRectTransform;

    /// <summary>Panel右上のポジション</summary>
    Vector2 m_panelRightTop;

    /// <summary>Panel左上のポジション</summary>
    Vector2 m_panelBottomLeft;


    Vector3[] m_panelFourCorners = new Vector3[4];
    Vector3[] m_canvasFourCorners = new Vector3[4];
    float depth = -1f;
    Vector3 rightTop;
    Vector3 leftBottom;

    private void Start()
    {
        m_panelRectTransform = GetComponent<RectTransform>();
        //GetFourConers();

        m_panelRectTransform.GetWorldCorners(m_panelFourCorners);
        foreach (var item in m_panelFourCorners)
        {
            Debug.Log(item);
        }

        m_canvasRectTransform.GetWorldCorners(m_canvasFourCorners);
        foreach (var item in m_canvasFourCorners)
        {
            Debug.Log(item);
        }

        //////////////////////////////////////////
        m_panelRightTop = m_panelRectTransform.anchorMax;
        m_panelBottomLeft = m_panelRectTransform.anchorMin;
    }

    void GetFourConers()
    {
        m_panelRectTransform.GetWorldCorners(m_panelFourCorners);
        //foreach (var item in m_fourCorners)
        //{
        //    Debug.Log(item);
        //}
    }

    /// <summary>
    /// ドラッグ開始処理
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDrag = true;
        m_pointerPosition = eventData.position;
        m_startPosition = transform.position;
        Debug.Log("ドラッグ開始");
    }

    /// <summary>
    /// ドラッグ中処理
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        //Vector2 vector = eventData.position - m_pointerPosition;
        //vector.y = 0;
        //transform.position = m_startPosition + vector;
        //Debug.Log("ドラッグ中");
        m_panelRectTransform.GetWorldCorners(m_panelFourCorners);
        //左にドラッグ
        if (m_pointerPosition.magnitude > eventData.position.magnitude)
        {
            Debug.Log("左にドラッグ");
            Debug.Log(m_canvasFourCorners[2].magnitude);
            Debug.Log(m_panelFourCorners[2].magnitude);
            if (m_canvasFourCorners[2].magnitude > m_panelFourCorners[2].magnitude)
            {
                Vector2 vector = eventData.position - m_pointerPosition;
                vector.y = 0;
                transform.position = m_startPosition + vector;
            }
        }

        //右にドラッグ
    }

    /// <summary>
    /// ドラッグ終了処理
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        m_isDrag = false;
        Debug.Log("ドラッグ終了");
    }

    
    /// <summary>
    /// 選択された時代に移動する
    /// </summary>
    /// <param name="period">時代のシーン名</param>
    public void PeriodButton(string period)
    {
        Debug.Log("押された");
        //LoadSceneManager.AnyLoadScene(period, () =>
        //{
        //    LoadSceneManager.FadeOutPanel();
        //});
    }
}