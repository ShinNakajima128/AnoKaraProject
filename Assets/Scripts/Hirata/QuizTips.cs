using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>クイズのヒント</summary>
public class QuizTips : MonoBehaviour
{
    /// <summary>UI</summary>
    [SerializeField]
    GameObject m_tipsObject;

    /// <summary>表示テキスト</summary>
    [SerializeField]
    Text m_tipsText;

    /// <summary>データ</summary>
    string m_tips;

    /// <summary>表示するのに必要な回数</summary>
    [SerializeField]
    int m_tipsActiveCount = 5;

    /// <summary>連打キャンセル時間</summary>
    [SerializeField]
    float m_mashCancelTime = 1.5f;

    /// <summary>連打カウント</summary>
    int m_mashCount = 0;

    /// <summary>連打中か否か</summary>
    bool m_isMash = false;

    /// <summary>クイズ中にヒントを表示したか否か</summary>
    bool m_isTips = false;

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
        }
    }

    /// <summary>
    /// 連打受付処理
    /// </summary>
    IEnumerator MashTips()
    {
        float time = 0;
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
        m_tipsObject.SetActive(true);
    }

    /// <summary>
    /// ヒントの情報を取得する
    /// </summary>
    void GetTips()
    {
        EventManager.ListenEvents(Events.QuizStart, () =>
        {
            m_tips = QuizManager.Instance.CurrentQuizTips;
        });
        m_tipsText.text = m_tips;
    }

    /// <summary>
    /// ヒントを閉じる
    /// </summary>
    public void CloseTips()
    {
        m_tipsObject.SetActive(false);
    }
}
