using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MasterData;
using System;

/// <summary>
/// クイズを表示するクラス
/// </summary>
public class QuizManager : MonoBehaviour
{
    #region serialize field
    [Header("制限時間")]
    [SerializeField]
    float m_answerTime = 10f;

    [Header("次の問題を表示するまでの時間")]
    [SerializeField]
    float m_nextQuestionTimer = 2.0f;

    [Header("クイズ画面の各オブジェクト")]
    [SerializeField]
    Text m_timeLimit = default;

    [SerializeField]
    Text m_question = default;

    [SerializeField]
    GameObject m_JudgePanel = default;

    [SerializeField]
    Image[] m_judgeImages = default;

    [SerializeField]
    Text m_countDown = default;

    [Header("4択クイズのオブジェクト")]
    [SerializeField]
    GameObject m_fourChoicesQuizPanel = default;

    [SerializeField]
    Text[] m_choices = default;

    [Header("デバッグ用")]
    [SerializeField]
    PeriodTypes m_periodType = default;

    [SerializeField]
    int questionLimit = 10;
    #endregion

    /// <summary> プレイヤーの解答 </summary>
    string m_playerAnswer = default;
    /// <summary> 現在のクイズの正しい答え </summary>
    string m_correctAnswer = default;
    /// <summary> 各問題の判定 </summary>
    bool[] m_questionResults = default;
    /// <summary> プレイヤーの回答フラグ </summary>
    bool m_isAnswered = false;
    /// <summary> 正誤フラグ </summary>
    bool m_isCorrected = false;
    /// <summary> 現在のクイズのコルーチン </summary>
    IEnumerator m_currentQuestion = default;
    #region property
    public static QuizManager Instance { get; private set; }
    /// <summary> 正解した数 </summary>
    public static int CorrectAnswersNum { get;  private set; }
    public static PeriodTypes PeriodType { get; set; }
    public int CurrentTurnNum { get; set; }
    public string QuestionResult { get; set; }
    public string PlayerAnswer { get => m_playerAnswer; set => m_playerAnswer = value; }
    public string CorrectAnswer { get => m_correctAnswer; set => m_correctAnswer = value; }
    public bool IsAnswered { get => m_isAnswered; set => m_isAnswered = value; }
    #endregion
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_questionResults = new bool[questionLimit];
        StartCoroutine(QuizStart());
    }

    /// <summary>
    /// クイズを開始する
    /// </summary>
    /// <returns></returns>
    IEnumerator QuizStart()
    {
        //カウントダウン開始
        yield return StartCoroutine(CountDown());

        //10問出し終えるまで続ける
        while (CurrentTurnNum < questionLimit)
        {
            Debug.Log("現在の問題は" + (CurrentTurnNum + 1) + "番目");
            m_currentQuestion = null;
            m_isAnswered = false;
            ResetQuizPanel();

            while (m_currentQuestion == null)
            {
                int num = UnityEngine.Random.Range(0, 3); //各クイズからランダムで問題を抽選する

                switch (num)
                {
                    //4択クイズが抽選された場合
                    case 0:

                        m_currentQuestion = FourChoicesQuizManager.Instance.OnFourQuizQuestion(m_fourChoicesQuizPanel,
                                                                                               m_question,
                                                                                               m_choices[0],
                                                                                               m_choices[1],
                                                                                               m_choices[2],
                                                                                               m_choices[3]);

                        break;
                    //穴埋めクイズが抽選された場合
                    case 1:
                        Debug.Log("穴埋めクイズ");
                        //記述例
                        //m_currentQuestion = AnaumeQuizManager.Instance.OnAnaumeQuizQuestion(各オブジェクトの引数);
                        break;
                    //線繋ぎクイズが抽選された場合
                    case 2:
                        Debug.Log("線繋ぎクイズ");
                        break;
                }
                yield return null;
            }           
            yield return m_currentQuestion;
            CurrentTurnNum++;
            yield return null;
        }
        //正解した数を保存
        CorrectAnswersNum = m_questionResults.Count(b => b);
        Debug.Log(CorrectAnswersNum);
        //仮にここでResult画面へ遷移の記述。できればGameManagerのOnGameEnd関数などを用意してここに書きたい
        LoadSceneManager.AnyLoadScene("Result");
    }

    #region common
    #region coroutine

    /// <summary>
    /// 制限時間を計測を開始する
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimeLimit()
    {
        float timer = m_answerTime;

        while (!m_isAnswered)
        {
            timer -= Time.deltaTime;
            m_timeLimit.text = timer.ToString("F1");

            if (timer < 0) //時間切れになった場合
            {
                yield break; //コルーチン終了
            }
            yield return null;
        }
    }

    /// <summary>
    /// 判定する
    /// </summary>
    /// <returns></returns>
    public IEnumerator Judge()
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

    IEnumerator CountDown()
    {
        m_countDown.enabled = true;
        m_countDown.text = "3";

        yield return new WaitForSeconds(1.0f);
        m_countDown.text = "2";

        yield return new WaitForSeconds(1.0f);
        m_countDown.text = "1";

        yield return new WaitForSeconds(1.0f);
        m_countDown.text = "スタート！";
        yield return new WaitForSeconds(1.0f);
        m_countDown.enabled = false;
    }
    #endregion

    /// <summary>
    /// 各クイズの画面を非表示にする
    /// </summary>
    void ResetQuizPanel()
    {
        m_fourChoicesQuizPanel.SetActive(false); //4択のクイズ画面が出ていたら非表示にする
        //ここに追加で他のクイズのパネルを非表示にするコードを書いてください
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
        m_questionResults[CurrentTurnNum] = correct;
    }
    #endregion

    #region FourChoicesQuizMethod
    /// <summary>
    /// 4択問題を解答する
    /// </summary>
    /// <param name="answerType"> 選択肢の種類 </param>
    public void FourChoicesQuizSelectAnswer(int answerType)
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
    #endregion
}
