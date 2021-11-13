using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu]
public class AnaumeQuizData : ScriptableObject
{
    //https://script.google.com/macros/s/AKfycbw6vpPYJ-aWAUK75W6zlWgfcbVV_KdRySjaRUtslTSL_uWEHIQ/exec
    [Header("穴埋めクイズデータのスプレッドシートのURL")]
    [SerializeField]
    string m_spreadsheetURL = default;

    [Header("このオブジェクトに保管するデータの時代")]
    [SerializeField]
    PeriodTypes m_periodType = default;

    [Header("穴埋めクイズのデータ")]
    [SerializeField]
    AnaumeQuizDatabase[] m_database = default;

    public string URL => m_spreadsheetURL;
    public string PeriodTypeName => m_periodType.ToString();
    public PeriodTypes PeriodType => m_periodType;
    public AnaumeQuizDatabase[] AnaumeQuizDatabases { get => m_database; set => m_database = value; }
}