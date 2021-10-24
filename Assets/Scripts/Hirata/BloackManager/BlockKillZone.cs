using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ブロック崩しのキルゾーン</summary>
public class BlockKillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Destroy(collision.gameObject);
            //ゲームオーバー時の処理を書く
            Debug.Log("ゲームオーバー");
        }
    }
}
