using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu]
public class LineConnectionQuizData : ScriptableObject
{
    //
    [Header("選つなぎクイズデータのスプレッドシートのURL")]
    [SerializeField]
    string m_spreadsheetURL = default;

    [Header("このオブジェクトに保管するデータの時代")]
    [SerializeField]
    PeriodTypes m_periodType = default;

    [Header("選つなぎクイズのデータ")]
    [SerializeField]
    LineConnectionQuizDatabase[] m_database = default;

    public string URL => m_spreadsheetURL;
    public string PeriodTypeName => m_periodType.ToString();
    public PeriodTypes PeriodType => m_periodType;
    public LineConnectionQuizDatabase[] LineConnectQuizDatabases { get => m_database; set => m_database = value; }
}

[System.Serializable]
public class LineConnectionQuizDatabase
{
    //QuizMastarDataに移す予定
    public int Id;
    [Tooltip("問題文")]
    public string Question;

    [Tooltip("正しい答え")]
    public string Answer;

    [HideInInspector]
    [SerializeField]
    public string Dragtext;

    [SerializeField, Tooltip("動かせる文字達")]
    string[] m_dragTexts;
    public string[] DragTexts { get => m_dragTexts; }

    public void ConvartToArray()
    {
        m_dragTexts = Dragtext.Split(',');
    }
}