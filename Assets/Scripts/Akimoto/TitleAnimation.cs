using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField]
    float m_maxDuration = 500f;

    [SerializeField]
    float m_minDuration = 500f;
    
    [SerializeField]
    float m_moveTime = 0.3f;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        var sequence = DOTween.Sequence();
        sequence
            .Append(rect.DOAnchorPos3DY(m_maxDuration, m_moveTime))
            .Append(rect.DOAnchorPos3DY(m_minDuration, m_moveTime))
            .SetLoops(-1);
    }

    void Update()
    {
        
    }
}
