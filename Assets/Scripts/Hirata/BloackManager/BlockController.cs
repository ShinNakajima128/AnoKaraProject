using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ブロックコントローラー</summary>
public class BlockController : MonoBehaviour
{
    /// <summary>壊れるかどうか</summary>
    [SerializeField]
    bool m_isDestroy = true;

    /// <summary>ブロックの耐久力</summary>
    [SerializeField]
    int m_life = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (m_isDestroy)
            {
                m_life--;
                if (m_life <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            m_life--;
            if (m_life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
