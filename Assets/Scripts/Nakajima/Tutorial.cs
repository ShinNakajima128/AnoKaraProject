using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// チュートリルの機能を持つクラス
/// </summary>
public class Tutorial : MonoBehaviour
{
    /// <summary> 説明用のImage </summary>
    [SerializeField]
    Image m_tutorialImage = default;

    /// <summary> 画面を切り替えるボタン </summary>
    [SerializeField]
    Button[] m_progressButtons = default;

    /// <summary> 説明用の画像データ </summary>
    [SerializeField]
    Sprite[] m_tutorialSprites = default;

    /// <summary> 説明用の文章 </summary>
    [SerializeField]
    string[] m_tutorialMessage = default;

    /// <summary> 次に進むボタンのテキスト </summary>
    [SerializeField]
    Text m_nextButtonText = default;

    [SerializeField]
    Text m_tutorialText = default;

    int m_currentIndex = 0;

    private void OnEnable()
    {
        m_tutorialImage.sprite = m_tutorialSprites[0];
        m_tutorialText.text = m_tutorialMessage[0];
        m_currentIndex = 0;
        m_nextButtonText.text = "つぎへ";
        m_progressButtons[1].gameObject.SetActive(false);
    }

    public void NextPanel()
    {
        if (m_currentIndex >= m_tutorialSprites.Length - 1)
        {
            if (SceneManager.GetActiveScene().name == "QuizPart")
            {
                QuizManager.Instance.OffTutorialPanel();
            }
            else if (SceneManager.GetActiveScene().name == "PeriodSelect")
            {
                PeriodSelectManager.Instance.OffHelpPanel();
            }

            SoundManager.Instance.PlaySe("SE_touch");
            return;
        }
        else if (m_currentIndex == m_tutorialSprites.Length - 2)
        {
            m_nextButtonText.text = "終了";
        }
        m_currentIndex++;
        m_tutorialImage.sprite = m_tutorialSprites[m_currentIndex];
        m_tutorialText.text = m_tutorialMessage[m_currentIndex];

        //文字数に応じてフォントサイズ調整
        if (m_tutorialText.text.Length >= 35)
        {
            m_tutorialText.fontSize = 35;
        }
        else
        {
            m_tutorialText.fontSize = 50;
        }

        if (!m_progressButtons[1].gameObject.activeSelf)
        {
            m_progressButtons[1].gameObject.SetActive(true);
        }

        SoundManager.Instance.PlaySe("SE_touch");
    }

    public void BackPanel()
    {
        if (m_currentIndex == m_tutorialSprites.Length - 1)
        {
            m_nextButtonText.text = "つぎへ";
        }

        if (m_currentIndex <= 0)
        {
            return;
        }
        else if (m_currentIndex == 1)
        {
            m_progressButtons[1].gameObject.SetActive(false);
        }
        m_currentIndex--;
        m_tutorialImage.sprite = m_tutorialSprites[m_currentIndex];
        m_tutorialText.text = m_tutorialMessage[m_currentIndex];

        //文字数に応じてフォントサイズ調整
        if (m_tutorialText.text.Length >= 35)
        {
            m_tutorialText.fontSize = 35;
        }
        else
        {
            m_tutorialText.fontSize = 50;
        }

        SoundManager.Instance.PlaySe("SE_touch");
    }
}
