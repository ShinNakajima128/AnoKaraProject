using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全てのクイズデータが入っているスクリプタブルオブジェクト
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable / Create AllQuizData")]
public class AllQuizData : ScriptableObject
{
    [Header("4択クイズのデータ")]
    [SerializeField]
    FourChoicesQuizData[] m_fourChoicesQuizDatas = default;

    [Header("穴埋めクイズのデータ")]
    [SerializeField]
    AnaumeQuizData[] m_anaumeQuizDatas = default;

    [Header("線繋ぎクイズのデータ")]
    [SerializeField]
    LineConnectionQuizData[] m_lineConnectionsQuizDatas = default;

    public FourChoicesQuizData[] FourChoicesQuizDatas { get => m_fourChoicesQuizDatas; set => m_fourChoicesQuizDatas = value; }
    public AnaumeQuizData[] AnaumeQuizDatas { get => m_anaumeQuizDatas; set => m_anaumeQuizDatas = value; }
    public LineConnectionQuizData[] LineConnectionQuizDatas { get => m_lineConnectionsQuizDatas; set => m_lineConnectionsQuizDatas = value; }
}
