using System.Linq;
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

    /// <summary>リザルトアイコン</summary>
    [SerializeField]
    ResultIconMove[] m_moveIconMove;

    /// <summary>クリアアイコンを表示するタイマー</summary>
    [SerializeField]
    float m_iconTimer = 2f;

    /// <summary>
    /// 正解数
    /// クイズマネージャーが正解数を持っているので参照する
    /// </summary>
    [SerializeField]
    int m_ansCount = 7;

    /// <summary>
    /// 残り体力
    /// クイズマネージャーが正解数を持っているので参照する
    /// </summary>
    [SerializeField]
    int m_hp = 5;

    void Start()
    {
        m_nextUiButton.interactable = false;
        if (!m_isDebug)
        {
            m_ansCount = QuizManager.CorrectAnswersNum;
            m_hp = QuizManager.RemainingHP;
        }
        QuizResult();
    }

    /// <summary>
    /// 正解数に応じて、表示するUIを変える
    /// </summary>
    public void QuizResult()
    {
        if (m_hp == 5)
        {
            QuizResultUiSet(2);
            StartCoroutine(SetIcon(2, m_iconTimer));
            FlagOpen();
            DataManager.Instance.UpdateAchieve(StageQuizAchieveStates.Three);
        }
        else if (m_hp >= 3)
        {
            QuizResultUiSet(0);
            StartCoroutine(SetIcon(1, m_iconTimer));
            FlagOpen();
            DataManager.Instance.UpdateAchieve(StageQuizAchieveStates.Two);
        }
        else if (m_hp >= 1)
        {
            QuizResultUiSet(0);
            StartCoroutine(SetIcon(0, m_iconTimer));
            DataManager.Instance.UpdateAchieve(StageQuizAchieveStates.One);
        }
        else if (m_hp < 1)
        {
            QuizResultUiSet(3);
            DataManager.Instance.UpdateAchieve(StageQuizAchieveStates.None);
            Debug.Log("アチーブ：なし");
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
        DataManager.Instance.FlagOpen((int)GameManager.Instance.CurrentPeriod, GameManager.Instance.CurrentStageId);
    }

    /// <summary>
    /// 星を表示する
    /// </summary>
    /// <param name="index">表示する星の数</param>
    /// <param name="timer">表示するスピード</param>
    /// <returns></returns>
    IEnumerator SetIcon(int index, float timer)
    {
        yield return new WaitForSeconds(timer);
        for (int i = 0; i <= index; i++)
        {
            m_moveIconMove[i].IconSet();
            yield return new WaitForSeconds(timer);
        }
        m_nextUiButton.interactable = true;
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
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// 次のシーンへ
    /// ボタンに設定する
    /// </summary>
    public void NextScene(string scene)
    {
        if (GameManager.Instance.CurrentPeriod == MasterData.PeriodTypes.Edo && GameManager.Instance.CurrentStageId == 3)
        {
            LoadSceneManager.AnyLoadScene("Ending");
        }
        else
        {
            LoadSceneManager.AnyLoadScene(scene, () =>
            {
                GameManager.Instance.IsAfterQuized = true;
            });
        }
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// リトライ
    /// リトライボタンに設定する
    /// </summary>
    public void QuizRetry()
    {
        LoadSceneManager.LoadBeforeScene();
        SoundManager.Instance.PlaySe("SE_touch");
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