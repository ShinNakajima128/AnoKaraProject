using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>クラス説明</summary>
public class Samaple : MonoBehaviour
{
    public static Samaple Instance { get; private set; }

    /// <summary>サマリーは一行で記載する</summary>
    [SerializeField]                //SerializeFieldで改行
    GameObject m_obj = default;     //メンバー変数には"m_"
                                    //次の変数は改行する
    /// <summary>サンプル</summary>
    GameObject m_obj2 = default;

    /// <summary>
    /// 関数名
    /// </summary>
    void SampleVoid()
    {

    }

    /// <summary>
    /// 関数名
    /// </summary>
    /// <param name="value">引数の詳細</param>
    /// <returns>返り値</returns>
    int Sample(int value)
    {
        int a = value;
        return a;
    }
}


