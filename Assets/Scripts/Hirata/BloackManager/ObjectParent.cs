using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ブロックの親オブジェクトにアタッチする</summary>
public class ObjectParent : MonoBehaviour
{
    private void FixedUpdate()
    {
        if (gameObject.transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
