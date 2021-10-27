using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ブロックコントローラー</summary>
public class BlockController : MonoBehaviour
{
    /// <summary>ブロックの耐久力</summary>
    [SerializeField]
    int m_life = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Hit();
        }
    }

    /// <summary>
    /// 当てられた時の処理
    /// </summary>
    void Hit()
    {
        m_life--;
        if (m_life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
