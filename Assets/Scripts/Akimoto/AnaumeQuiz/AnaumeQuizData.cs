using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnaumeQuizData : ScriptableObject
{
    public List<AnaumeQuizDatabase> m_database = new List<AnaumeQuizDatabase>();
}

[System.Serializable]
public class AnaumeQuizDatabase
{
    /// <summary>問題文</summary>
    [SerializeField]
    string m_question;

    /// <summary>穴埋め本文</summary>
    [SerializeField]
    string m_anaume;

    /// <summary>正解の文字</summary>
    [SerializeField]
    string[] m_correctText;

    /// <summary>不正解文字</summary>
    [SerializeField]
    string[] m_dummyText;

    public string Question { get { return m_question; } }
    public string Anaume { get { return m_anaume; } }
    public string CorrectText(int index) { return m_correctText [index]; }
    public string DummyText(int index) { return m_dummyText[index]; }
}