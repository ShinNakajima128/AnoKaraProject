using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリルの機能を持つクラス
/// </summary>
public class Tutorial : MonoBehaviour
{
    [SerializeField]
    Image m_tutorialImage = default;

    [SerializeField]
    Button[] m_progressButtons = default;

    [SerializeField]
    Sprite[] m_tutorialSprites = default;

    [SerializeField]
    Text m_nextButtonText = default;

    int m_currentIndex = 0;

    private void OnEnable()
    {
        m_tutorialImage.sprite = m_tutorialSprites[0];
        m_currentIndex = 0;
        m_progressButtons[1].gameObject.SetActive(false);
    }

    public void NextPanel()
    {
        if (m_currentIndex >= m_tutorialSprites.Length - 1)
        {
            QuizManager.Instance.OffTutorialPanel();
            SoundManager.Instance.PlaySe("SE_touch");
            return;
        }
        else if (m_currentIndex == m_tutorialSprites.Length - 2)
        {
            m_nextButtonText.text = "終了";
        }
        m_currentIndex++;
        m_tutorialImage.sprite = m_tutorialSprites[m_currentIndex];

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
        SoundManager.Instance.PlaySe("SE_touch");
    }
}
