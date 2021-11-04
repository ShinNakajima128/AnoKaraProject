using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>パドルコントローラー</summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PaddleController : MonoBehaviour
{
    /// <summary>Rigidbody2D</summary>
    Rigidbody2D m_rb2d;

    /// <summary>スピード</summary>
    [SerializeField]
    float m_speed = 5f;

    /// <summary>右ボタンの長押しフラグ</summary>
    bool m_isRightHold = false;

    /// <summary>左ボタンの長押しフラグ</summary>
    bool m_isLeftHold = false;

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// 右ボタンが押された時(ボタンに設定)
    /// </summary>
    public void RightButtonDown()
    {
        m_isRightHold = true;
    }

    /// <summary>
    /// 右ボタンが離された時(ボタンに設定)
    /// </summary>
    public void RightButtonUp()
    {
        m_isRightHold = false;
    }

    /// <summary>
    /// 左ボタンが押された時(ボタンに設定)
    /// </summary>
    public void LeftButtonDown()
    {
        m_isLeftHold = true;
    }

    /// <summary>
    /// 左ボタンが離された時(ボタンに設定)
    /// </summary>
    public void LeftButtonUp()
    {
        m_isLeftHold = false;
    }

    /// <summary>
    /// パドルの移動処理
    /// </summary>
    void Move()
    {
        if (m_isRightHold && !m_isLeftHold)
        {
            m_rb2d.velocity = m_speed * Vector2.right;
        }
        else if (m_isLeftHold && !m_isRightHold)
        {
            m_rb2d.velocity = m_speed * Vector2.left;
        }
        else
        {
            m_rb2d.velocity = Vector2.zero;
        }
    }
}
