using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Option : MonoBehaviour
{
    [SerializeField]
    float m_fadeTime = 1;

    /// <summary>名前入力欄</summary>
    [SerializeField]
    GameObject m_nameInput = default;

    [SerializeField]
    Slider m_bgmSlider = default;

    [SerializeField]
    Slider m_seSlider = default;

    [SerializeField]
    Slider m_voiceSlider = default;

    private void Start()
    {
        //GetComponent<Canvas>().enabled = false;
        //SoundManager.Instance.SettingVolume();
        SetSliderVolume();
        m_nameInput.SetActive(false);
    }

    public void SetSliderVolume()
    {
        m_bgmSlider.value = SoundManager.Instance.BgmVolume * 10;
        m_seSlider.value = SoundManager.Instance.SeVolume * 10;
        m_voiceSlider.value = SoundManager.Instance.VoiceVolume * 10;
    }

    public void OnClick(bool isActive)
    {
        if (isActive)
        {
            m_nameInput.SetActive(true);
            m_nameInput.transform.DOScale(new Vector3(1, 1, 1), m_fadeTime);
        }
        else
        {
            m_nameInput.SetActive(false);
            m_nameInput.transform.DOScale(new Vector3(0, 0, 0), m_fadeTime);
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
                SoundManager.Instance.BgmVolume = m_seSlider.value / 10;
                break;
            case ChangeType.Voice:
                SoundManager.Instance.BgmVolume = m_voiceSlider.value / 10;
                break;
        }
        SoundManager.Instance.SettingVolume();
    }
}


public enum ChangeType { Bgm, Se, Voice }