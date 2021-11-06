using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu(menuName = "MyScriptable / Create FourChoicesQuizData")]
public class FourChoicesQuizData : ScriptableObject
{
    [Header("4択クイズデータのスプレッドシートのURL")]
    [SerializeField]
    string m_spreadsheetURL = default;

    [Header("このオブジェクトに保管するデータの時代")]
    [SerializeField]
    PeriodTypes m_periodType = default;

    [Header("4択クイズのデータ")]
    [SerializeField]
    FourChoicesQuiz[] m_fourChoicesQuizzes = default;

    public string URL => m_spreadsheetURL;
    public string PeriodTypeName => m_periodType.ToString();
    public PeriodTypes PeriodType => m_periodType;
    public FourChoicesQuiz[] FourChoicesQuizzes { get => m_fourChoicesQuizzes; set => m_fourChoicesQuizzes = value; }
}
