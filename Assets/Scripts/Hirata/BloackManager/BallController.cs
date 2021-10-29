using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ブロック崩しのボールコントローラー</summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    /// <summary>Rigidbody2D</summary>
    Rigidbody2D m_rb2d;

    /// <summary>開始した時にボールが飛んで行く方向</summary>
    [SerializeField]
    Vector2 m_startDirection = Vector2.up + Vector2.left;

    /// <summary>開始した時にボールにかける力</summary>
    [SerializeField]
    float m_startPowar = 3f;

    /// <summary>失速判定になるスピード</summary>
    [SerializeField]
    float m_lowSpeed = 2f;

    /// <summary>失速した時に付与するスピード</summary>
    [SerializeField]
    float m_addSpeed = 5f;

    /// <summary>最高スピード</summary>
    [SerializeField]
    float m_maxSpeed = 5f;

    void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_rb2d.velocity.magnitude < m_lowSpeed)
        {
            m_rb2d.velocity = m_rb2d.velocity.normalized * m_addSpeed;
        }

        if (m_rb2d.velocity.magnitude > m_maxSpeed)
        {
            m_rb2d.velocity = m_rb2d.velocity.normalized * m_addSpeed;
        }
    }

    /// <summary>
    /// ボールを飛ばす
    /// </summary>
    public void StartPush()
    {
        m_rb2d.AddForce(m_startDirection.normalized * m_startPowar, ForceMode2D.Impulse);
    }

    /// <summary>
    /// ボールをランダムの方向に飛ばす
    /// ボールの軌道が詰んでる時に押してもらう
    /// </summary>
    public void Shake()
    {
        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(0, 1f);
        Vector2 dir = Vector2.right * x + Vector2.up * y;
        dir = dir.normalized;
        m_rb2d.AddForce(dir * m_startPowar, ForceMode2D.Impulse);
    }
}