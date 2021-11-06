using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterData;

public class FourChoicesQuizManager : MonoBehaviour
{
    /// <summary> 4択クイズのデータ </summary>
    FourChoicesQuiz[] m_fourChoicesDatas = default;
    /// <summary> 現在の問題の番号 </summary>
    int m_currentNum = 0;

    public static FourChoicesQuizManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        var currentPeriod = GameManager.Instance.CurrentPeriod; //現在の時代を取得
        var quizDatas = DataManager.Instance.FourChoicesQuizDatas; //4択問題のデータを取得

        //取得した時代のクイズデータを取得する
        for (int i = 0; i < quizDatas.Length; i++)
        {
            if (quizDatas[i].PeriodType == currentPeriod)
            {
                m_fourChoicesDatas = quizDatas[i].FourChoicesQuizzes;
                break;
            }
        }
        //ランダムで問題を並び変える
        RandomlySorted(m_fourChoicesDatas);
    }

    /// <summary>
    /// 4択クイズを出題する
    /// </summary>
    /// <param name="panel"> 4択クイズ画面 </param>
    /// <param name="question"> 問題文を表示するText </param>
    /// <param name="c1"> 選択肢1を表示するText </param>
    /// <param name="c2"> 選択肢2を表示するText </param>
    /// <param name="c3"> 選択肢3を表示するText </param>
    /// <param name="c4"> 選択肢4を表示するText </param>
    /// <returns></returns>
    public IEnumerator OnFourQuizQuestion(GameObject panel,Text question, Text c1, Text c2, Text c3, Text c4)
    {
        //既に4択クイズの問題を全て出し終えていたら終了して他のクイズを出す
        if (m_currentNum >= m_fourChoicesDatas.Length)
        {
            yield break;
        }
        else
        {
            //クイズ画面を表示する
            panel.SetActive(true);
            //各テキストを更新する
            question.text = m_fourChoicesDatas[m_currentNum].Question;
            c1.text = m_fourChoicesDatas[m_currentNum].Choices1;
            c2.text = m_fourChoicesDatas[m_currentNum].Choices2;
            c3.text = m_fourChoicesDatas[m_currentNum].Choices3;
            c4.text = m_fourChoicesDatas[m_currentNum].Choices4;
            QuizManager.Instance.CorrectAnswer = m_fourChoicesDatas[m_currentNum].Answer;
        }
        yield return QuizManager.Instance.TimeLimit();

        //時間切れだった場合
        if (!QuizManager.Instance.IsAnswered)
        {
            QuizManager.Instance.IsAnswered = true;
            QuizManager.Instance.FourChoicesQuizSelectAnswer(0);    //解答無し
        }

        yield return new WaitUntil(() => QuizManager.Instance.IsAnswered);

        yield return QuizManager.Instance.Judge();
        m_currentNum++;
    }

    /// <summary>
    /// 問題の順番をランダムに並べ替える
    /// </summary>
    void RandomlySorted(FourChoicesQuiz[] quizzes)
    {
        for (int i = 0; i < quizzes.Length; i++)
        {
            int random = Random.Range(0, quizzes.Length);
            var temp = m_fourChoicesDatas[i];
            m_fourChoicesDatas[i] = m_fourChoicesDatas[random];
            m_fourChoicesDatas[random] = temp;
        }
    }
}
