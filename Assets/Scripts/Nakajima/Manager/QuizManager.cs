using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MasterData;
using System;
using DG.Tweening;

public enum QuizTypes
{
    FourChoice,
    Anaume,
    FourChoiceAndAnaume
}

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

    [Header("出題するクイズの種類")]
    [SerializeField]
    QuizTypes m_quizType = QuizTypes.FourChoice;

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

    /// <summary> 判定画面の各Text </summary>
    [SerializeField]
    Text[] m_judgePanelTexts = default;

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

    /// <summary> クイズ画面の背景 </summary>
    [SerializeField]
    Image m_background = default;

    /// <summary> 時代毎の背景 </summary>
    [SerializeField]
    Sprite[] m_periodBackgrounds = default;

    /// <summary> 吹き出し画面のオブジェクトをまとめているPanel </summary>
    [SerializeField]
    GameObject m_blowingPanel = default;

    /// <summary>デフォルトのキャラ画像の親</summary>
    [SerializeField]
    Transform m_defaultParent = default;

    /// <summary>正解時のキャラ画像の親</summary>
    [SerializeField]
    Transform m_correctParent = default;

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

    /// <summary> クイズの進行度を表すImage </summary>
    [SerializeField]
    Image m_quizResultUIImage = default;

    /// <summary> クイズ結果 </summary>
    [SerializeField]
    Animator[] m_quizResultUIAnims = default;

    /// <summary> クイズ終了時に表示するPanel </summary>
    [SerializeField]
    GameObject m_finishPanel = default;

    [SerializeField]
    Button m_tipsButton = default;

    [Header("4択クイズのオブジェクト")]
    [SerializeField]
    GameObject m_fourChoicesQuizPanel = default;

    /// <summary> 各選択肢のText </summary>
    [SerializeField]
    Text[] m_choices = default;

    [Header("穴埋めクイズのオブジェクト")]
    [SerializeField]
    GameObject m_AnaumeQuizPanel = default;

    [Header("チュートリアル画面用")]
    [SerializeField]
    GameObject m_tutorialSelectPanel = default;

    [SerializeField]
    GameObject m_tutorialPanel = default;

    [SerializeField]
    GameObject m_guidePanel = default;

    [SerializeField]
    Button[] m_tutorialButtons = default;

    [SerializeField]
    Text m_tutorialGuideText = default;

    /// <summary> クイズの最大出題数 </summary>
    [Header("デバッグ用")]
    [SerializeField]
    int questionLimit = 10;

    [SerializeField]
    bool m_isDebugMode = false;

    /// <summary>GameOverパネル /summary>
    [SerializeField]
    GameObject m_panel;
    #endregion

    /// <summary> プレイヤーのデータ </summary>
    PlayerData m_playeData = default;
    /// <summary> 現在プレイ中の時代の偉人データ </summary>
    CharacterData m_historicalFiguresData = default;
    /// <summary> プレイヤーの解答 </summary>
    string m_playerAnswer = default;
    /// <summary> 現在のクイズの正しい答え </summary>
    string m_correctAnswer = default;
    /// <summary> 現在のクイズのヒント </summary>
    string m_currentQuizTips = default;
    /// <summary> 各問題の判定 </summary>
    bool[] m_questionResults = default;
    /// <summary> プレイヤーの回答フラグ </summary>
    bool m_isAnswered = false;
    /// <summary> 正誤フラグ </summary>
    bool m_isCorrected = false;
    /// <summary> クイズ出題の乱数の最小値 </summary>
    int m_minRangeNum = default;
    /// <summary> クイズ出題の乱数の最大値 </summary>
    int m_maxRangeNum = default;

    /// <summary> チュートリアル画面でボタンを押したかどうかのフラグ </summary>
    bool m_isSelect = false;

    /// <summary> チュートリアルを見るか見ないかのフラグ </summary>
    bool m_TutorialSelect = false;

    /// <summary> 現在のクイズのコルーチン </summary>
    IEnumerator m_currentQuestion = default;
    #region property
    public static QuizManager Instance { get; private set; }
    /// <summary> 正解した数 </summary>
    public static int CorrectAnswersNum { get;  private set; }
    /// <summary> クイズ終了時の残りHP </summary>
    public static int RemainingHP { get; private set; }
    public int CurrentTurnNum { get; set; }
    public string QuestionResult { get; set; }
    public string PlayerAnswer { get => m_playerAnswer; set => m_playerAnswer = value; }
    public string CorrectAnswer { get => m_correctAnswer; set => m_correctAnswer = value; }
    public bool IsAnswered { get => m_isAnswered; set => m_isAnswered = value; }
    public string CurrentQuizTips { get => m_currentQuizTips; set => m_currentQuizTips = value; }
    public bool QuizDataUpdated { get; set; } = false;

    public bool IsDebugMode => m_isDebugMode;
    #endregion
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_playeData = DataManager.Instance.PlayerData;
        m_historicalFiguresData = DataManager.Instance.CurrentPeriodHistoricalFigures;
        SetBackground(GameManager.Instance.CurrentPeriod);
        m_QuizResultUI.SetActive(false);
        m_quizPartObjects.SetActive(false);
        m_blowingPanel.SetActive(false);
        m_finishPanel.SetActive(false);
        //各人物の画像をセットする
        SetCharacterPanel(m_playeData, m_historicalFiguresData);
        m_questionResults = new bool[questionLimit];
        
        //縄文時代のみチュートリアルを見るかどうかの画面を表示
        if (GameManager.Instance.CurrentPeriod == PeriodTypes.Jomon_Yayoi)
        {
            Debug.Log("チュートリアル画面表示");
            m_tutorialSelectPanel.SetActive(true);
            m_tutorialSelectPanel.transform.localScale = Vector3.zero;
            m_tutorialSelectPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.35f);

            m_tutorialButtons[0].onClick.AddListener(() => //「はい」ボタンに登録
            {
                m_isSelect = true;
                m_TutorialSelect = true;
            });
            m_tutorialButtons[1].onClick.AddListener(() => //「いいえ」ボタンに登録
            {
                m_isSelect = true;
                m_TutorialSelect = false;
            });
            StartCoroutine(WaitForTutorialSelect());
        }
        else
        {
            StartCoroutine(QuizStart());
        }
        SoundManager.Instance.PlayBgm(SoundManager.Instance.BgmName);

        if (!m_isDebugMode) 
        {
            //縄文時代と平安時代のステージ1,ステージ2のみ、穴埋め問題も出題する
            if (GameManager.Instance.CurrentPeriod == PeriodTypes.Jomon_Yayoi)
            {
                m_quizType = QuizTypes.FourChoiceAndAnaume;
                SetQuizType(m_quizType);
            }
            else　//その他の時代は4択問題のみ出題
            {
                m_quizType = QuizTypes.FourChoice;
                SetQuizType(m_quizType);
            }
        }
        else //デバッグ中は指定したクイズを出題する
        {
            SetQuizType(m_quizType);
        }
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
        m_blowingPanel.SetActive(true);
        m_playerImage.sprite = m_playeData.PlayerImage[1]; //考え中の画像を表示
        m_playerChat.text = m_playeData.ThinkingChat; //考え中のプレイヤー吹き出し文を表示
        m_historicalFiguresChat.text = m_historicalFiguresData.ThinkingChat; //考え中の偉人吹き出し文を表示
        m_correctRate.text = $"正答率：{0}%";

        //10問出し終えるまで続ける
        while (CurrentTurnNum < questionLimit)
        {
            m_currentQuestionUI.text = $"第{CurrentTurnNum + 1}問";
            m_playerAnswer = "";
            QuizDataUpdated = false;
            m_currentQuestion = null;
            m_isAnswered = false;
            ResetQuizPanel();
            //m_quizResultUISlider.value = CurrentTurnNum;
            m_quizResultUIImage.fillAmount = CurrentTurnNum * 0.1f;
            m_quizResultUIAnims[CurrentTurnNum].Play("Thinking");

            while (!QuizDataUpdated)
            {
                int num = UnityEngine.Random.Range(m_minRangeNum, m_maxRangeNum); //各クイズからランダムで問題を抽選する

                switch (num)
                {
                    //4択クイズが抽選された場合
                    case 0:
                        Debug.Log("4択クイズ");
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
                        m_currentQuestion = AnaumeQuiz.Instance.OnAnaumeQuizQuestion(m_AnaumeQuizPanel, m_question);
                        break;
                    //線繋ぎクイズが抽選された場合
                    case 2:
                        Debug.Log("線繋ぎクイズ");
                        break;
                }
                Debug.Log(m_currentQuizTips);
                EventManager.OnEvent(Events.QuizStart);
                SoundManager.Instance.PlaySe("SE_quiz");
                yield return m_currentQuestion;
            }
            //正解した数を保存
            CorrectAnswersNum = m_questionResults.Count(b => b);
            m_correctRate.text = $"正答率：{CalculateCorrectAnswerRate(CurrentTurnNum, CorrectAnswersNum).ToString("F0")}%";
            yield return null;
        }
        ///ここから下にクイズが終了した時の処理を記述する///
        m_quizResultUIImage.fillAmount = CurrentTurnNum * 0.1f;
        m_playerChat.text = "難しかった…";
        m_historicalFiguresChat.text = m_historicalFiguresData.ClearChat;
        m_finishPanel.SetActive(true);
        //仮にここでResult画面へ遷移の記述。できればEventManagerのOnGameEnd関数等を用意してここに書きたい
        yield return new WaitForSeconds(2.0f);
        LoadSceneManager.AnyLoadScene("QuizResult");
        RemainingHP = HPController.Instance.CurrentHP;
    }

    IEnumerator WaitForTutorialSelect()
    {
        //ボタンを押すまで待機
        while (!m_isSelect)
        {
            yield return null;
        }

        //チュートリアルを見る場合
        if (m_TutorialSelect)
        {
            OnTutorialPanel();
        }
    }
    #region common
    #region coroutine

    /// <summary>
    /// 制限時間を計測を開始する
    /// </summary>
    /// <returns></returns>
    public IEnumerator TimeLimit()
    {
        float timer = m_answerTime + 0.5f;

        while (!m_isAnswered)
        {
            timer -= Time.deltaTime;
            m_timeLimit.text = timer.ToString("F0");

            if (timer < 0.5) //時間切れになった場合
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
        m_defaultParent.GetChild(0).SetParent(m_correctParent); //キャラ画像を手前に持ってくる
        yield return new WaitForSeconds(m_nextQuestionTimer);

        foreach (var images in m_judgeImages)
        {
            images.enabled = false;
        }
        m_playerImage.sprite = m_playeData.PlayerImage[1]; //考え中の画像を表示
        m_historicalFiguresImage.sprite = m_historicalFiguresData.CharacterImages[0];
        m_playerChat.text = m_playeData.ThinkingChat;
        m_historicalFiguresChat.text = m_historicalFiguresData.ThinkingChat;
        m_correctParent.GetChild(0).SetParent(m_defaultParent); //キャラ画像を奥に戻す
        m_JudgePanel.SetActive(false);
    }

    IEnumerator CountDown()
    {
        if (GameManager.Instance.CurrentPeriod != PeriodTypes.Jomon_Yayoi)
        {
            yield return new WaitForSeconds(0.4f);
        }

        m_countDown.enabled = true;
        //ヒントボタンOFF
        m_tipsButton.interactable = false;
        m_countDown.text = "3";
        SoundManager.Instance.PlaySe("SE_touch");

        yield return new WaitForSeconds(1.0f);
        m_countDown.text = "2";
        SoundManager.Instance.PlaySe("SE_touch");

        yield return new WaitForSeconds(1.0f);
        m_countDown.text = "1";
        SoundManager.Instance.PlaySe("SE_touch");

        yield return new WaitForSeconds(1.0f);
        m_countDown.text = "開始！";
        SoundManager.Instance.PlaySe("SE_title");
        yield return new WaitForSeconds(1.0f);
        
        //ヒントボタンON
        m_tipsButton.interactable = true;
        m_countDown.enabled = false;
    }
    #endregion

    IEnumerator Gameover()
    {
        yield return new WaitForSeconds(2.0f);

        //m_JudgePanel.SetActive(false);
        m_panel.SetActive(true);
        m_historicalFiguresChat.text = m_historicalFiguresData.FailureChat;

        while (true)
        {
            yield return null;
        }
        //yield return new WaitForSeconds(2.0f);
        //LoadSceneManager.AnyLoadScene("PeriodSelect");
    }

    /// <summary>
    /// 各クイズの画面を非表示にする
    /// </summary>
    void ResetQuizPanel()
    {
        m_fourChoicesQuizPanel.SetActive(false); //4択のクイズ画面が出ていたら非表示にする
        //ここに追加で他のクイズのパネルを非表示にするコードを書いてください
        m_AnaumeQuizPanel.SetActive(false);
    }

    /// <summary>
    /// 出題するクイズの種類を設定する
    /// </summary>
    /// <param name="type"></param>
    void SetQuizType(QuizTypes type)
    {
        switch (type)
        {
            case QuizTypes.FourChoice:
                m_minRangeNum = 0;
                m_maxRangeNum = 1;
                break;
            case QuizTypes.Anaume:
                m_minRangeNum = 1;
                m_maxRangeNum = 2;
                break;
            case QuizTypes.FourChoiceAndAnaume:
                m_minRangeNum = 0;
                m_maxRangeNum = 2;
                break;
            default:
                Debug.LogError("値が正しくありません");
                break;
        }
    }

    /// <summary>
    /// 判定の結果を表示する
    /// </summary>
    /// <param name="correct"> 判定 </param>
    void ShowJudge(bool correct)
    {
        if (m_correctAnswer.Length > 19)
        {
            m_judgePanelTexts[1].text = $"正しい答え\n<size=35>【{m_correctAnswer}】</size>";
        }
        else if (m_correctAnswer.Length > 14)
        {
            m_judgePanelTexts[1].text = $"正しい答え\n<size=45>【{m_correctAnswer}】</size>";
        }
        else if (m_correctAnswer.Length > 7)
        {
            m_judgePanelTexts[1].text = $"正しい答え\n<size=55>【{m_correctAnswer}】</size>";
        }
        else
        {
            m_judgePanelTexts[1].text = $"正しい答え\n【{m_correctAnswer}】";
        }

        //正解
        if (correct)
        {
            m_JudgePanel.SetActive(true);
            //m_judgeImages[0].enabled = true;
            m_judgePanelTexts[0].text = "<color=#FB3535>正解！</color>";

            if (m_playerAnswer.Length > 19)
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n<size=35>【{m_playerAnswer}】</size>";
            }
            else if (m_playerAnswer.Length > 14)
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n<size=45>【{m_playerAnswer}】</size>";
            }
            else if (m_playerAnswer.Length > 7)
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n<size=55>【{m_playerAnswer}】</size>";
            }
            else
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n【{m_playerAnswer}】";
            }
            m_playerImage.sprite = m_playeData.PlayerImage[2];
            m_historicalFiguresImage.sprite = m_historicalFiguresData.CharacterImages[1];
            m_playerChat.text = m_playeData.CorrectChat;
            m_historicalFiguresChat.text = m_historicalFiguresData.CorrectChat;
            m_quizResultUIAnims[CurrentTurnNum].Play("Correct");
            SoundManager.Instance.PlaySe("SE_correct");
            FixPlayVoice(VoiceType.Player, "voice004");
        }
        //不正解
        else
        {
            m_JudgePanel.SetActive(true);
            //m_judgeImages[1].enabled = true;
            m_judgePanelTexts[0].text = "<color=#588AFF>不正解…</color>";
            if (m_playerAnswer.Length > 19)
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n<size=35>【{m_playerAnswer}】</size>";
            }
            else if (m_playerAnswer.Length > 14)
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n<size=45>【{m_playerAnswer}】</size>";
            }
            else if (m_playerAnswer.Length > 7)
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n<size=55>【{m_playerAnswer}】</size>";
            }
            else
            {
                m_judgePanelTexts[2].text = $"{DataManager.Instance.PlayerData.PlayerName}の解答\n【{m_playerAnswer}】";
            }

            m_playerImage.sprite = m_playeData.PlayerImage[3];
            m_historicalFiguresImage.sprite = m_historicalFiguresData.CharacterImages[2];
            m_playerChat.text = m_playeData.IncorrectChat;
            m_historicalFiguresChat.text = m_historicalFiguresData.IncorrectChat;
            m_quizResultUIAnims[CurrentTurnNum].Play("InCorrect");
            SoundManager.Instance.PlaySe("SE_incorrect");
            FixPlayVoice(VoiceType.Player, "voice005");
            
            //デバッグモードでは体力が減らない
            if (!m_isDebugMode)
            {
                HPController.Instance.CurrentHP--;
            }
        }
        m_questionResults[CurrentTurnNum] = correct;
        EventManager.OnEvent(Events.QuizEnd);
        HPCheck();
    }

    /// <summary>
    /// Scene開始時にプレイヤーと偉人の画像データをセットする
    /// </summary>
    /// <param name="player"> プレイヤーデータ </param>
    /// <param name="historicalFigures"> 偉人データ </param>
    void SetCharacterPanel(PlayerData player, CharacterData historicalFigures)
    {
        //デフォルトポーズの画像をセットする
        m_playerImage.sprite = player.PlayerImage[0];
        m_historicalFiguresImage.sprite = historicalFigures.CharacterImages[0];

        //各時代の人物を名前で判別して表示するキャラクターを変更する処理を追加する必要あり
    }

    void HPCheck()
    {
        if(HPController.Instance.CurrentHP <= 0)
        {
            Debug.Log("ゲームオーバー");
            StopAllCoroutines();
            StartCoroutine(Gameover());
        }
    }

    void FixPlayVoice(VoiceType type, string id)
    {
        var s = id.Replace("player_", "voice").Replace("Koma_", "voice");
        SoundManager.Instance.PlayVoice(type, s);
    }

    void SetBackground(PeriodTypes period)
    {
        switch (period)
        {
            case PeriodTypes.Jomon_Yayoi:
                m_background.sprite = m_periodBackgrounds[0];
                break;
            case PeriodTypes.Asuka_Nara:
                m_background.sprite = m_periodBackgrounds[1];
                break;
            case PeriodTypes.Heian:
                m_background.sprite = m_periodBackgrounds[2];
                break;
            case PeriodTypes.Kamakura:
                m_background.sprite = m_periodBackgrounds[3];
                break;
            case PeriodTypes.Momoyama:
                m_background.sprite = m_periodBackgrounds[4];
                break;
            case PeriodTypes.Edo:
                m_background.sprite = m_periodBackgrounds[5];
                break;
            default:
                Debug.LogError("時代が設定されていません");
                break;
        }
    }

    /// <summary>
    /// 現在のクイズの正答率を計算する
    /// </summary>
    /// <param name="currentQuestionNum"> 現在のクイズ数 </param>
    /// <param name="correctNum"> 正解した数 </param>
    /// <returns></returns>
    float CalculateCorrectAnswerRate(float currentQuestionNum, float correctNum)
    {
        return (correctNum / currentQuestionNum) * 100;
    }
    #endregion

    /// <summary>
    /// クイズをやり直す。ボタン用
    /// </summary>
    public void Retry()
    {
        LoadSceneManager.AnyLoadScene("QuizPart");
    }

    /// <summary>
    /// 時代選択画面に戻る。ボタン用
    /// </summary>
    public void ReturnPeriodSelect()
    {
        LoadSceneManager.AnyLoadScene("PeriodSelect");
    }

    /// <summary>
    /// チュートリアル画面を表示する
    /// </summary>
    void OnTutorialPanel()
    {
        SoundManager.Instance.PlaySe("SE_touch");
        m_tutorialPanel.SetActive(true);
        m_guidePanel.SetActive(false);
        m_tutorialGuideText.text = "チュートリアル";
    }

    /// <summary>
    /// チュートリアル画面を閉じる
    /// </summary>
    public void OffTutorialPanel()
    {
        m_tutorialSelectPanel.transform.DOScale(Vector3.zero, 0.25f)
                             .OnComplete(() => 
                             {
                                 m_tutorialSelectPanel.SetActive(false);
                                 StartCoroutine(QuizStart());
                             });
    }
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
