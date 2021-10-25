using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu(menuName = "MyScriptable / Create FourChoicesQuizData")]
public class FourChoicesQuizData : ScriptableObject
{
    [SerializeField]
    PeriodTypes m_periodType = default;

    [SerializeField]
    FourChoicesQuiz[] m_fourChoicesQuiz = default;

    public string PeriodTypeName => m_periodType.ToString();
    public PeriodTypes PeriodType => m_periodType;
    public FourChoicesQuiz[] FourChoicesQuiz { get => m_fourChoicesQuiz; set => m_fourChoicesQuiz = value; }
}
