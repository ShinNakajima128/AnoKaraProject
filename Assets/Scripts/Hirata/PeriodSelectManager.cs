using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>時代選択画面マネージャー</summary>
public class PeriodSelectManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    /// <summary>ドラッグしているか否か</summary>
    bool m_isDrag = false;

    /// <summary>ドラッグ開始時のポインターポジション</summary>
    Vector2 m_pointerPosition;

    /// <summary>ドラッグ開始時のUIのポジション</summary>
    Vector2 m_startPosition;

    /// <summary></summary>
    [SerializeField]
    GameObject m_stopLeft;

    [SerializeField]
    GameObject m_stopRight;

    private void Start()
    {

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
        Vector2 vector = eventData.position - m_pointerPosition;
        vector.y = 0;
        transform.position = m_startPosition + vector;
        Debug.Log("ドラッグ中");
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
    /// クリックした処理
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_isDrag)
        {
            return;
        }
        Debug.Log("タッチ");
    }

    public void Button()
    {
        Debug.Log("押された");
    }
}