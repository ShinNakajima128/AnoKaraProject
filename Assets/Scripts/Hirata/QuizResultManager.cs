using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>クイズのリザルトマネージャー</summary>
public class QuizResultManager : MonoBehaviour
{
    /// <summary>デバッグモード</summary>
    [SerializeField]
    bool m_isDebug = false;

    /// <summary>正解数を表示するテキスト</summary>
    [SerializeField]
    Text m_answerCountText;

    /// <summary>キャラクターのイメージ</summary>
    [SerializeField]
    Image m_charactorImage;

    /// <summary>次へのUIを表示するボタン</summary>
    [SerializeField]
    Button m_nextUiButton;

    /// <summary>次へのUI群</summary>
    [SerializeField]
    GameObject m_nextUi;

    /// <summary>リザルトの星</summary>
    [SerializeField]
    GameObject[] m_stars;

    /// <summary>星を表示するタイマー</summary>
    [SerializeField]
    float m_starTimer = 2f;

    /// <summary>
    /// 正解数
    /// クイズマネージャーが正解数を持っているので参照する
    /// </summary>
    [SerializeField]
    int m_ansCount = 7;

    void Start()
    {
        if (!m_isDebug)
        {
            m_ansCount = QuizManager.CorrectAnswersNum;
        }
        QuizResult();
    }

    /// <summary>
    /// 正解数に応じて、表示するUIを変える
    /// </summary>
    public void QuizResult()
    {
        if (m_ansCount == 10)
        {
            QuizResultUiSet(0);
            StartCoroutine(SetStar(2, m_starTimer));
            FlagOpen();
        }
        else if (m_ansCount >= 7)
        {
            QuizResultUiSet(2);
            StartCoroutine(SetStar(1, m_starTimer));
            FlagOpen();
        }
        else if (m_ansCount >= 3)
        {
            QuizResultUiSet(3);
            StartCoroutine(SetStar(0, m_starTimer));
        }
        else if (m_ansCount < 3)
        {
            QuizResultUiSet(3);
        }
    }

    /// <summary>
    /// 正解数に応じて結果をUIに表示する
    /// </summary>
    /// <param name="index">成功レベル</param>
    void QuizResultUiSet(int index)
    {
        m_answerCountText.text = m_ansCount.ToString() + " / 10 問";
        m_charactorImage.sprite = DataManager.Instance.PlayerData.PlayerImage[index];
    }

    /// <summary>
    /// クリアフラグを開ける
    /// </summary>
    void FlagOpen()
    {
        Debug.Log((int)GameManager.Instance.CurrentPeriod);
        Debug.Log((int)GameManager.Instance.CurrentStageId);
        DataManager.Instance.FlagOpen((int)GameManager.Instance.CurrentPeriod,(int)GameManager.Instance.CurrentStageId);
    }

    /// <summary>
    /// 星を表示する
    /// </summary>
    /// <param name="index">表示する星の数</param>
    /// <param name="timer">表示するスピード</param>
    /// <returns></returns>
    IEnumerator SetStar(int index, float timer)
    {
        yield return new WaitForSeconds(timer);
        for (int i = 0; i <= index; i++)
        {
            m_stars[i].SetActive(true);
            yield return new WaitForSeconds(timer);
        }
        yield break;
    }

    /// <summary>
    /// 次へのUIを表示する
    /// ボタンに設定する
    /// </summary>
    public void NextUiOn()
    {
        m_nextUiButton.interactable = false;
        m_nextUi.SetActive(true);
    }

    /// <summary>
    /// 次のシーンへ
    /// ボタンに設定する
    /// </summary>
    public void NextScene(string scene)
    {
        LoadSceneManager.AnyLoadScene(scene, () =>
        {
            LoadSceneManager.FadeOutPanel();
        });
    }

    /// <summary>
    /// リトライ
    /// リトライボタンに設定する
    /// </summary>
    public void QuizRetry()
    {
        LoadSceneManager.LoadBeforeScene();
    }

    /// <summary>
    /// 問題解説
    /// 問題解説ボタンに設定する
    /// </summary>
    public void QuizReview()
    {
        Debug.Log("問題解説");
    }
}