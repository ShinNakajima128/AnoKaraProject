using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Option : MonoBehaviour
{
    //[SerializeField]
    //float m_fadeTime = 1;

    /// <summary>名前入力欄</summary>
    [SerializeField]
    GameObject m_nameInput = default;

    [SerializeField]
    GameObject m_optionObj = default;

    [SerializeField]
    Slider m_bgmSlider = default;

    [SerializeField]
    Slider m_seSlider = default;

    [SerializeField]
    Slider m_voiceSlider = default;

    private bool m_flag = false;

    private void Start()
    {
        SetSliderVolume();
        m_nameInput.SetActive(false);
        m_optionObj.SetActive(false);
    }

    public void SetSliderVolume()
    {
        m_bgmSlider.value = SoundManager.Instance.BgmVolume * 10;
        m_seSlider.value = SoundManager.Instance.SeVolume * 10;
        m_voiceSlider.value = SoundManager.Instance.VoiceVolume * 10;
    }

    public void OnNameInputClick(bool isActive)
    {
        if (isActive)
        {
            m_nameInput.SetActive(true);
            //m_nameInput.transform.localScale = Vector3.zero;
            //m_nameInput.transform.DOScale(Vector3.one, m_fadeTime);
        }
        else
        {
            //m_nameInput.transform.localScale = Vector3.one;
            //m_nameInput.transform.DOScale(Vector3.zero, m_fadeTime);
            m_nameInput.SetActive(false);
        }
    }

    public void OnSettingButtonClick()
    {
        if (m_flag)
        {
            m_optionObj.SetActive(false);
            m_flag = false;
        }
        else
        {
            m_optionObj.SetActive(true);
            m_flag = true;
        }
    }

    public void OnValueChanged(int changeType)
    {
        switch ((ChangeType)changeType)
        {
            case ChangeType.Bgm:
                SoundManager.Instance.BgmVolume = m_bgmSlider.value / 10;
                break;
            case ChangeType.Se:
                SoundManager.Instance.SeVolume = m_seSlider.value / 10;
                break;
            case ChangeType.Voice:
                SoundManager.Instance.VoiceVolume = m_voiceSlider.value / 10;
                break;
        }
        SoundManager.Instance.SettingVolume();
    }
}


public enum ChangeType { Bgm, Se, Voice }