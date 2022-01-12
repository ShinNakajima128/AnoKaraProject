using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Option : MonoBehaviour
{
    //[SerializeField]
    //float m_fadeTime = 1;

    [Header("名前変更後に表示するパネル")]
    [SerializeField]
    GameObject m_panel = default;

    [Header("名前変更後パネルの表示時間")]
    [SerializeField]
    float m_time = default;

    /// <summary>名前入力欄</summary>
    [SerializeField]
    GameObject m_nameInput = default;

    [SerializeField]
    GameObject m_optionObj = default;

    [SerializeField]
    Slider m_bgmSlider = default;

    [SerializeField]
    Image m_bgmSliderFill = default;

    [SerializeField]
    Slider m_seSlider = default;

    [SerializeField]
    Image m_seSliderFill = default;

    [SerializeField]
    Slider m_voiceSlider = default;

    [SerializeField]
    Image m_voiceSliderFill = default;

    private bool m_flag = false;

    private void Start()
    {
        SetSliderVolume();
        m_panel.SetActive(false);
        m_nameInput.SetActive(false);
        m_optionObj.SetActive(false);
    }

    public void SetSliderVolume()
    {
        m_bgmSlider.value = SoundManager.Instance.BgmVolume * 10;
        m_seSlider.value = SoundManager.Instance.SeVolume * 10;
        m_voiceSlider.value = SoundManager.Instance.VoiceVolume * 10;

        m_bgmSliderFill.fillAmount = SoundManager.Instance.BgmVolume;
        m_seSliderFill.fillAmount = SoundManager.Instance.SeVolume;
        m_voiceSliderFill.fillAmount = SoundManager.Instance.VoiceVolume;

    }

    public void RenameClick()
    {
        StartCoroutine("Rename");
    }

    IEnumerator Rename()
    {
        m_panel.SetActive(true);
        yield return new WaitForSeconds(m_time);
        m_panel.SetActive(false);
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
        SoundManager.Instance.PlaySe("SE_touch");
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
        SoundManager.Instance.PlaySe("SE_touch");
    }

    public void OnValueChanged(int changeType)
    {
        switch ((ChangeType)changeType)
        {
            case ChangeType.Bgm:
                SoundManager.Instance.BgmVolume = m_bgmSlider.value / 10;
                m_bgmSliderFill.fillAmount = m_bgmSlider.value / 10;
                break;
            case ChangeType.Se:
                SoundManager.Instance.SeVolume = m_seSlider.value / 10;
                m_seSliderFill.fillAmount = m_seSlider.value / 10;
                break;
            case ChangeType.Voice:
                SoundManager.Instance.VoiceVolume = m_voiceSlider.value / 10;
                m_voiceSliderFill.fillAmount = m_voiceSlider.value / 10;
                break;
        }

        SoundManager.Instance.SettingVolume();

        if (m_flag)
        {
            if (changeType == 1)
            {
                SoundManager.Instance.PlaySe("SE_touch");
            }
            else if (changeType == 2)
            {
                SoundManager.Instance.PlayVoice(VoiceType.Koma, "voice008");
            }
        } 
    }
}


public enum ChangeType { Bgm, Se, Voice }