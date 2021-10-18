using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuizType 
{
    FourChoices
}

/// <summary>
/// クイズを表示するクラス
/// </summary>
public class QuizManager : MonoBehaviour
{
    [SerializeField]
    Text m_question = default;

    [SerializeField]
    Text m_choice1 = default;

    [SerializeField]
    Text m_choice2 = default;

    [SerializeField]
    Text m_choice3 = default;

    [SerializeField]
    Text m_choice4 = default;

    string m_playerAnswer = default;
    string m_correctAnswer = default;

    bool m_isAnswered = false;
    bool m_isCorrected = false;

    public static QuizManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(FourQuizSet());
    }

    IEnumerator FourQuizSet()
    {
        while (!QuizDataManager.Instance.OnData)
        {
            yield return null;
        }
        var quizData = QuizDataManager.Instance.FourChoicesQuizMaster;
        
        for (int i = 0; i < quizData.Length; i++)
        {
            m_isAnswered = false;

            m_question.text = quizData[i].Question;
            m_choice1.text = quizData[i].Choices1;
            m_choice2.text = quizData[i].Choices2;
            m_choice3.text = quizData[i].Choices3;
            m_choice4.text = quizData[i].Choices4;
            m_correctAnswer = quizData[i].Answer;

            while (!m_isAnswered)
            {
                yield return null;
            }

            yield return StartCoroutine(Judge());
        }
    }
    
    public void SelectAnswer(int answerType)
    {
        switch (answerType)
        {
            case 1:
                m_playerAnswer = m_choice1.text;
                break;
            case 2:
                m_playerAnswer = m_choice2.text;
                break;
            case 3:
                m_playerAnswer = m_choice3.text;
                break;
            case 4:
                m_playerAnswer = m_choice4.text;
                break;
        }
        Debug.Log("プレイヤーの回答 : " + m_playerAnswer);
        Debug.Log("正しい答え : " + m_correctAnswer);
        m_isAnswered = true;
    }
    IEnumerator Judge()
    {
        if (m_playerAnswer == m_correctAnswer)
        {
            m_isCorrected = true;
            ShowJudge(m_isCorrected);
        }
        else
        {
            m_isCorrected = false;
            ShowJudge(m_isCorrected);
        }
        yield return new WaitForSeconds(2.0f);
    }

    void ShowJudge(bool correct)
    {
        if (correct)
        {
            Debug.Log("正解!");
        }
        else
        {
            Debug.Log("不正解");
        }
    }
}
