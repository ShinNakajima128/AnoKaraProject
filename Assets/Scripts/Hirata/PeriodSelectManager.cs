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

    RectTransform m_rectTransform;

    Vector3[] m_fourCorners = new Vector3[4];

    float depth = -1f;

    Vector3 rightTop;

    Vector3 leftBottom;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        GetFourConers();
        rightTop = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        leftBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));
    }

    void GetFourConers()
    {
        m_rectTransform.GetWorldCorners(m_fourCorners);
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
        GetFourConers();
        Debug.Log(m_fourCorners[0].x);
        Debug.Log(leftBottom.x);
        if (m_fourCorners[0].x < leftBottom.x)
        {
            Vector2 vector = eventData.position - m_pointerPosition;
            vector.y = 0;
            transform.position = m_startPosition + vector;
        }
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