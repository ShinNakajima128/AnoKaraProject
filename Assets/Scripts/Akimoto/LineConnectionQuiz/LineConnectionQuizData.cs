using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu]
public class LineConnectionQuizData : ScriptableObject
{
    //https://script.google.com/macros/s/AKfycbw3ecejz5h5Zeyl2oBjVngC53B-CsfFes0l5dWerDK4CHVlfN8U/exec
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