using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackButton : MonoBehaviour
{
    [SerializeField]
    string m_returnSceneName = default;

    [SerializeField]
    string m_loadSceneName = default;

    [SerializeField]
    GameObject m_backButton = default;

    [SerializeField]
    GameObject m_confirmPanel = default;

    [SerializeField]
    GameObject m_settingPanel = default;

    [SerializeField]
    GameObject m_backPanel = default;

    [SerializeField]
    Text m_confirmText = default;

    [SerializeField]
    GameObject m_allPanel = default;

    bool m_isOpened = false;

    private void Start()
    {
        m_confirmPanel.SetActive(false);
        m_backPanel.SetActive(false);
        EventManager.ListenEvents(Events.BeginDialog, OffPanel);
        EventManager.ListenEvents(Events.FinishDialog, OnPanel);
    }

    public void OnConfirmPanel()
    {
        if (!m_isOpened)
        {
            m_backPanel.SetActive(true);
            m_settingPanel.SetActive(false);
            m_backButton.SetActive(false);
            m_confirmPanel.SetActive(true);
            m_confirmPanel.transform.localScale = Vector3.zero;
            m_confirmPanel.transform.DOScale(Vector3.one, 0.2f);
            m_confirmText.text = $"{m_returnSceneName}にもどりますか？";
            m_isOpened = true;
        }
        else
        {
            m_confirmPanel.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => 
            {
                m_backPanel.SetActive(false);
                m_settingPanel.SetActive(true);
                m_confirmPanel.SetActive(false);
                m_backButton.SetActive(true);
                m_isOpened = false;
            });
        }
    }
    
    /// <summary>
    /// 指定したSceneに戻る
    /// </summary>
    public void BackScene()
    {
        LoadSceneManager.AnyLoadScene(m_loadSceneName);
    }

    void OnPanel()
    {
        m_allPanel.SetActive(true);
    }

    void OffPanel()
    {
        m_allPanel.SetActive(false);
    }
}
