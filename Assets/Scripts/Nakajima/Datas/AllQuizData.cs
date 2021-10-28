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

    public FourChoicesQuizData[] FourChoicesQuizDatas { get => m_fourChoicesQuizDatas; set => m_fourChoicesQuizDatas = value; }
}
