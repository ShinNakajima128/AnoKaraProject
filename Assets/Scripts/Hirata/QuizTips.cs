using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>クイズのヒント</summary>
public class QuizTips : MonoBehaviour
{
    /// <summary>UI</summary>
    [SerializeField]
    GameObject m_tipsUi;

    /// <summary>表示テキスト</summary>
    [SerializeField]
    Text m_tipsText;

    /// <summary>データ</summary>
    string m_tips;

    /// <summary>表示するのに必要な回数</summary>
    [SerializeField]
    int m_tipsActiveCount = 5;

    /// <summary>連打UI</summary>
    [SerializeField]
    GameObject m_mashUi;

    /// <summary>連打キャンセル時間</summary>
    [SerializeField]
    float m_mashCancelTime = 1.5f;

    /// <summary>連打カウント</summary>
    int m_mashCount = 0;

    /// <summary>連打中か否か</summary>
    bool m_isMash = false;

    /// <summary>クイズ中にヒントを表示したか否か</summary>
    bool m_isTips = false;

    /// <summary>ヒントを表示中か否か</summary>
    bool m_isTipsActive = true;

    /// <summary>ヒントチュートリアルが表示されているかどうか</summary>
    bool m_isTutorialActive = false;

    /// <summary>ヒントのチュートリアルの表示時間</summary>
    [SerializeField]
    float m_tutorialTipsTimer = 5f;

    Vector2 m_defPos = default;

    [SerializeField]
    float m_moveDuration = 0.1f;

    [SerializeField]
    float m_moveValue = 0.1f;

    private void Start()
    {
        if (GameManager.Instance.CurrentPeriod == MasterData.PeriodTypes.Jomon_Yayoi)
        {
            EventManager.ListenEvents(Events.QuizStart, GetTips);
        }
        m_defPos = transform.position;
    }

    /// <summary>
    /// チュートリアルのヒントをアクティブ化する
    /// </summary>
    public void TutorialTipsActive()
    {
        StartCoroutine(TutorialTips());
    }

    /// <summary>
    /// ヒントのチュートリアルを表示する
    /// </summary>
    /// <returns></returns>
    IEnumerator TutorialTips()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            if (!m_isTutorialActive)
            {
                m_isTutorialActive = true;
                m_tipsUi.SetActive(true);
                m_tipsText.text = "僕を連打すると1度だけヒントがもらえるよ";
            }

            if (timer > m_tutorialTipsTimer || m_isMash)
            {
                m_tipsUi.SetActive(false);
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// ヒントボタン
    /// </summary>
    public void Tips()
    {
        if (!m_isTips)
        {
            if (!m_isMash)
            {
                m_isMash = true;
                StartCoroutine(MashTips());
            }
            m_mashCount++;
            transform.position = m_defPos;
            transform.DOMoveY(m_moveValue, m_moveDuration);
            SoundManager.Instance.PlaySe("SE_touch");
        }
    }

    /// <summary>
    /// 連打受付処理
    /// </summary>
    IEnumerator MashTips()
    {
        float time = 0;
        m_mashUi.SetActive(true);
        while (m_isMash)
        {
            time += Time.deltaTime;

            if (time > m_mashCancelTime)
            {
                Reset();
                yield break;
            }

            if (m_mashCount >= m_tipsActiveCount)
            {
                ActiveTips();
                Reset();
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }

        void Reset()
        {
            m_isMash = false;
            m_mashUi.SetActive(false);
            m_mashCount = 0;
        }
    }

    /// <summary>
    /// ヒントを表示する
    /// </summary>
    void ActiveTips()
    {
        GetTips();
        m_isTips = true;
        m_tipsUi.SetActive(true);
        SoundManager.Instance.PlaySe("SE_popup");
        StartCoroutine(CloseTips());
    }

    /// <summary>
    /// UIを閉じるまで待機する
    /// </summary>
    IEnumerator CloseTips()
    {
        while (m_isTipsActive)
        {
            yield return new WaitForEndOfFrame();
        }
        m_tipsUi.SetActive(false);
        SoundManager.Instance.PlaySe("SE_touch");
        yield break;
    }

    /// <summary>
    /// ヒントの情報を取得する
    /// </summary>
    void GetTips()
    {
        m_tips = QuizManager.Instance.CurrentQuizTips;
        m_tipsText.text = m_tips;
    }

    /// <summary>
    /// ヒントを閉じる
    /// </summary>
    public void Close()
    {
        m_isTipsActive = false;
    }
}