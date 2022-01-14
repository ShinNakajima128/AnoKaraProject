using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragText : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Vector2 m_defPos;
    /// <summary>このオブジェクトが持ってるテキスト</summary>
    private string m_text;

    public string Text { get => m_text; set => m_text = value; }

    void Start()
    {
        transform.GetChild(0).gameObject.GetComponent<Text>().text = m_text;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        foreach (var hit in result)
        {
            DropText item = hit.gameObject.GetComponent<DropText>();
            if (!item) continue;
            item.GetDrop(m_text);
            SoundManager.Instance.PlaySe("SE_touch");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_defPos = transform.position;
        SoundManager.Instance.PlaySe("SE_touch");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = m_defPos;
    }
}
