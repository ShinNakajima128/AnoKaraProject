using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public RectTransform t1;
    public RectTransform t2;

    void Start()
    {
        Vector3[] v = new Vector3[] { t1.anchoredPosition, t2.anchoredPosition };
        GetComponent<LineRenderer>().SetPositions(v);
    }
}
