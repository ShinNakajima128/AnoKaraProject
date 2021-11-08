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

    /// <summary> 第何問目かを表示するUI </summary>
    [SerializeField]
    Text m_currentQuestionUI = default;

    /// <summary> 問題文 </summary>
    [SerializeField]
    Text m_question = default;

    /// <summary> 判定の結果を表示するOanel </summary>
    [SerializeField]
    GameObject m_JudgePanel = default;

    /// <summary> 判定の画像 </summary>
    [SerializeField]
    Image[] m_judgeImages = default;

    /// <summary> カウントダウン用のText </summary>
    [SerializeField]
    Text m_countDown = default;

    /// <summary> プレイヤーの画像データ </summary>
    [SerializeField]
    Image m_playerImage = default;

    /// <summary> 偉人の画像データ </summary>
    [SerializeField]
    Image m_historicalFiguresImage = default;

    /// <summary> クイズパートの画面のUIをまとめたPanel </summary>
    [SerializeField]
    GameObject m_quizPartObjects = default;

    /// <summary> プレイヤーの吹き出し </summary>
    [SerializeField]
    Text m_playerChat = default;

    /// <summary> 偉人の吹き出し </summary>
    [SerializeField]
    Text m_historicalFiguresChat = default;

    /// <summary> クイズの正答率 </summary>
    [SerializeField]
    Text m_correctRate = default;

    /// <summary> クイズの結果の履歴のUI </summary>
    [SerializeField]
    GameObject m_QuizResultUI = default;

    /// <summary> クイズの進行度を表すSlider </summary>
    [SerializeField]
    Slider m_quizResultUISlider = default;

    /// <summary> クイズ結果 </summary>
    [SerializeField]
    Animator[] m_quizResultUIAnims = default;

    [Header("4択クイズのオブジェクト")]
    [SerializeField]
    GameObject m_fourChoicesQuizPanel = default;

    /// <summary> 各選択肢のText </summary>
    [SerializeField]
    Text[] m_choices = default;

    /// <summary> クイズの最大出題数 </summary>
    [Header("デバッグ用")]
    [SerializeField]
    int questionLimit = 10;
    #endregion

    /// <summary> プレイヤーのデータ </summary>
    PlayerData m_playeData = default;
    /// <summary> 現在プレイ中の時代の偉人データ </summary>
    CharacterData m_historicalFiguresData = default;
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
    public int CurrentTurnNum { get; set; }
    public string QuestionResult { get; set; }
    public string PlayerAnswer { get => m_playerAnswer; set => m_playerAnswer = value; }
    public string CorrectAnswer { get => m_correctAnswer; set => m_correctAnswer = value; }
    public bool IsAnswered { get => m_isAnswered; set => m_isAnswered = value; }

    public bool QuizDataUpdated { get; set; } = false;
    #endregion
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_playeData = DataManager.Instance.PlayerData;
        m_historicalFiguresData = DataManager.Instance.CurrentPeriodHistoricalFigures;
        m_QuizResultUI.SetActive(false);
        m_quizPartObjects.SetActive(false);
        //各人物の画像をセットする
        SetCharacterPanel(m_playeData, m_historicalFiguresData);
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

        m_QuizResultUI.SetActive(true);
        foreach (var a in m_quizResultUIAnims)
        {
            a.enabled = true;
            a.Play("Standby");
        }
        yield return null;
        m_quizPartObjects.SetActive(true);
        m_playerChat.text = m_playeData.ThinkingChat;
        m_historicalFiguresChat.text = m_historicalFiguresData.ThinkingChat;
        m_correctRate.text = $"正答率：{0}%";

        //10問出し終えるまで続ける
        while (CurrentTurnNum < questionLimit)
        {
            m_currentQuestionUI.text = $"第{CurrentTurnNum + 1}問";
            QuizDataUpdated = false;
            m_currentQuestion = null;
            m_isAnswered = false;
            ResetQuizPanel();
            m_quizResultUISlider.value = CurrentTurnNum;
            m_quizResultUIAnims[CurrentTurnNum].Play("Thinking");

            while (!QuizDataUpdated)
            {
                int num = UnityEngine.Random.Range(0, 1); //各クイズからランダムで問題を抽選する

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
                yield return m_currentQuestion;
            }
            //正解した数を保存
            CorrectAnswersNum = m_questionResults.Count(b => b);
            m_correctRate.text = $"正答率：{CalculateCorrectAnswerRate(CurrentTurnNum, CorrectAnswersNum).ToString("F0")}%";
            yield return null;
        }
        ///ここから下にクイズが終了した時の処理を記述する///
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
        m_playerImage.sprite = m_playeData.PlayerImage[0];
        m_historicalFiguresImage.sprite = m_historicalFiguresData.CharacterImages[0];
        m_playerChat.text = m_playeData.ThinkingChat;
        m_historicalFiguresChat.text = m_historicalFiguresData.ThinkingChat;
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
            m_playerImage.sprite = m_playeData.PlayerImage[1];
            m_historicalFiguresImage.sprite = m_historicalFiguresData.CharacterImages[1];
            m_playerChat.text = m_playeData.CorrectChat;
            m_historicalFiguresChat.text = m_historicalFiguresData.CorrectChat;
            m_quizResultUIAnims[CurrentTurnNum].Play("Correct");
        }
        //不正解
        else
        {
            m_JudgePanel.SetActive(true);
            m_judgeImages[1].enabled = true;
            m_playerImage.sprite = m_playeData.PlayerImage[2];
            m_historicalFiguresImage.sprite = m_historicalFiguresData.CharacterImages[2];
            m_playerChat.text = m_playeData.IncorrectChat;
            m_historicalFiguresChat.text = m_historicalFiguresData.IncorrectChat;
            m_quizResultUIAnims[CurrentTurnNum].Play("InCorrect");
        }
        m_questionResults[CurrentTurnNum] = correct;
    }

    void SetCharacterPanel(PlayerData player, CharacterData historicalFigures)
    {
        m_playerImage.sprite = player.PlayerImage[0];
        m_historicalFiguresImage.sprite = historicalFigures.CharacterImages[0];
    }

    float CalculateCorrectAnswerRate(float currentQuestionNum, float correctNum)
    {
        return (correctNum / currentQuestionNum) * 100;
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
