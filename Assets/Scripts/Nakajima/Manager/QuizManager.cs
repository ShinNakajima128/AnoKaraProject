using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterData;
using System;

/// <summary>
/// クイズを表示するクラス
/// </summary>
public class QuizManager : MonoBehaviour
{
    [Header("制限時間")]
    [SerializeField]
    float m_answerTime = 10f;

    [Header("次の問題を表示するまでの時間")]
    [SerializeField]
    float m_nextQuestionTimer = 2.0f;

    [Header("ゲーム画面の各オブジェクト")]
    #region
    [SerializeField]
    GameObject m_quizPanel = default;

    [SerializeField]
    Text m_question = default;

    [SerializeField]
    Text[] m_choices = default;

    [SerializeField]
    Text m_timeLimit = default;

    [SerializeField]
    GameObject m_JudgePanel = default;

    [SerializeField]
    Image[] m_judgeImages = default;
    #endregion
    string m_playerAnswer = default;
    string m_correctAnswer = default;

    /// <summary> プレイヤーの回答フラグ </summary>
    bool m_isAnswered = false;
    /// <summary> 正誤フラグ </summary>
    bool m_isCorrected = false;

    [Header("デバッグ用")]
    [SerializeField]
    PeriodTypes m_periodType = default;

    public static QuizManager Instance { get; private set; }

    public static PeriodTypes PeriodType { get; set; }

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
        FourChoicesQuiz[] quizData = default;

        foreach (var data in QuizDataManager.Instance.FourChoicesQuizDatas)
        {
            if (m_periodType == data.PeriodType)    //取得しようとしているデータが見つかったら
            {
                quizData = data.FourChoicesQuiz;    //クイズ表示用の変数に入れる
                break;
            }
            else
            {
                continue;
            }
        }
        //データが無かった場合
        if (quizData == null)
        {
            Debug.LogError("クイズデータが見つかりませんでした。");
            yield break;
        }

        m_quizPanel.SetActive(true);

        for (int i = 0; i < quizData.Length; i++)
        {
            m_isAnswered = false;
            
            //各テキストを更新する
            m_question.text = quizData[i].Question;
            m_choices[0].text = quizData[i].Choices1;
            m_choices[1].text = quizData[i].Choices2;
            m_choices[2].text = quizData[i].Choices3;
            m_choices[3].text = quizData[i].Choices4;
            m_correctAnswer = quizData[i].Answer;

            float timer = m_answerTime;

            while (!m_isAnswered)
            {
                timer -= Time.deltaTime;
                m_timeLimit.text = timer.ToString("F1");

                if (timer < 0) //時間切れになった場合
                {
                    m_isAnswered = true;
                    SelectAnswer(0);    //解答無し
                }
                yield return null;
            }
            yield return StartCoroutine(Judge());
        }
        m_quizPanel.SetActive(false);
    }

    /// <summary>
    /// 解答する
    /// </summary>
    /// <param name="answerType"> 選択肢の種類 </param>
    public void SelectAnswer(int answerType)
    {
        m_timeLimit.text = "";

        switch (answerType)
        {
            case 0:
                m_playerAnswer = "解答無し";
                break;
            case 1:
                m_playerAnswer = m_choices[0].text;
                break;
            case 2:
                m_playerAnswer = m_choices[1].text;
                break;
            case 3:
                m_playerAnswer = m_choices[2].text;
                break;
            case 4:
                m_playerAnswer = m_choices[3].text;
                break;
        }
        Debug.Log("プレイヤーの回答 : " + m_playerAnswer);
        Debug.Log("正しい答え : " + m_correctAnswer);
        m_isAnswered = true;
    }

    /// <summary>
    /// 判定する
    /// </summary>
    /// <returns></returns>
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
        yield return new WaitForSeconds(m_nextQuestionTimer);

        foreach (var images in m_judgeImages)
        {
            images.enabled = false;
        }
        m_JudgePanel.SetActive(false);
    }

    /// <summary>
    /// 判定の結果を表示する
    /// </summary>
    /// <param name="correct"> 判定 </param>
    void ShowJudge(bool correct)
    {
        //正解
        if (correct)    
        {
            m_JudgePanel.SetActive(true);
            m_judgeImages[0].enabled = true;
        }
        //不正解
        else
        {
            m_JudgePanel.SetActive(true);
            m_judgeImages[1].enabled = true;
        }
    }
}
