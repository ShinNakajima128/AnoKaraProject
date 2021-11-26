using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>リザルトアイコンの移動クラス</summary>
public class ResultIconMove : MonoBehaviour
{
    /// <summary>移動先のポジション</summary>
    [SerializeField]
    Transform m_setPosition;

    /// <summary>移動時間</summary>
    [SerializeField]
    float m_moveTime;

    /// <summary>
    /// アイコンをセットする
    /// </summary>
    public void IconSet()
    {
        var m_sequence = DOTween.Sequence();
        m_sequence.Append(transform.DOMove(m_setPosition.position, m_moveTime).SetEase(Ease.Linear));
        m_sequence.Join(transform.DORotate(new Vector3(0, 0, 720), m_moveTime, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        m_sequence.Play();
    }
}
