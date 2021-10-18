using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu(menuName = "MyScriptable / Create FourChoicesQuizData")]
public class FourChoicesQuizData : ScriptableObject
{
    [SerializeField]
    FourChoicesQuiz m_fourChoicesQuiz = default;

    public FourChoicesQuiz FourChoicesQuiz { get => m_fourChoicesQuiz; set => m_fourChoicesQuiz = value; }
}
