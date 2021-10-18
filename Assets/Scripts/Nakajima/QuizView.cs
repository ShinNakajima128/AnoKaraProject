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
public class QuizView : MonoBehaviour
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

    [SerializeField]
    Button[] m_choicesButtons = default;

    string m_answer = default;

    public static QuizView Instance { get; private set; }

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
            m_question.text = quizData[i].Question;
            m_choice1.text = quizData[i].Choices1;
            m_choice2.text = quizData[i].Choices2;
            m_choice3.text = quizData[i].Choices3;
            m_choice4.text = quizData[i].Choices4;
            m_answer = quizData[i].Answer;

            yield return new WaitUntil(() => Input.anyKeyDown);
        }
    }
    
    IEnumerator PlayerAnswer()
    {
        yield break;
    }
}
