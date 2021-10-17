using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "MyScriptable / Create FourChoicesQuizData")]
public class FourChoicesQuizData : ScriptableObject
{
    public int PeriodsID;
    public string Question;
    public string Choices1;
    public string Choices2;
    public string Choices3;
    public string Choices4;
    public string Answer;
}
