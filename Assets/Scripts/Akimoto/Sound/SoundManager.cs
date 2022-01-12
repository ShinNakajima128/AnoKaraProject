using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoiceType
{
    SelectBoy,
    SelectGirl,
    Player,
    Koma
}

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    string m_bgmName = default;

    [SerializeField]
    float m_switchSpeed = 1f;

    CriAtomSource m_bgmSource = default;
    CriAtomSource m_seSource = default;
    CriAtomSource m_voiceBoySource = default;
    CriAtomSource m_voiceGirlSource = default;
    CriAtomSource m_voiceKomaSource = default;
    static float m_bgmVolume = 0.5f;
    static float m_seVolume = 0.5f;
    static float m_voiceVolume = 0.5f;

    public static SoundManager Instance;
    public string BgmName { get => m_bgmName; set => m_bgmName = value; }
    public float SwitchSpeed { set => m_switchSpeed = value; }
    public float BgmVolume { get => m_bgmVolume; set => m_bgmVolume = value; }
    public float SeVolume { get => m_seVolume; set => m_seVolume = value; }
    public float VoiceVolume { get => m_voiceVolume; set => m_voiceVolume = value; }

    private void Awake()
    {
        Instance = this;
        Setup();
    }

    /// <summary>
    /// BGMを再生する
    /// </summary>
    /// <param name="name"> BGM名 </param>
    public void PlayBgm(string name)
    {
        m_bgmSource.loop = true;
        if (m_bgmSource.status == CriAtomSource.Status.Stop)
        {
            m_bgmSource.Play(name);
        }
        else
        {
            StartCoroutine(SwitchBGM(name));
        }
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    /// <param name="name"> SE名 </param>
    public void PlaySe(string name)
    {
        m_seSource.cueName = name;
        m_seSource.Play();
    }

    /// <summary>
    /// 指定したキャラクターのボイスを再生する
    /// </summary>
    /// <param name="voiceType"> キャラクターの種類 </param>
    /// <param name="name">  </param>
    public void PlayVoice(VoiceType voiceType, string name)
    {
        switch (voiceType)
        {
            case VoiceType.SelectBoy:
                m_voiceBoySource.cueName = name;
                m_voiceBoySource.volume = m_voiceVolume;
                m_voiceBoySource.Play();
                break;
            case VoiceType.SelectGirl:
                m_voiceGirlSource.cueName = name;
                m_voiceGirlSource.volume = m_voiceVolume;
                m_voiceGirlSource.Play();
                break;
            case VoiceType.Player:
                if(DataManager.Instance.PlayerData.PlayerGender == GenderType.Boy)
                {
                    m_voiceBoySource.cueName = name;
                    m_voiceBoySource.volume = m_voiceVolume;
                    m_voiceBoySource.Play();
                }
                else
                {
                    m_voiceGirlSource.cueName = name;
                    m_voiceGirlSource.volume = m_voiceVolume;
                    m_voiceGirlSource.Play();
                }
                break;
            case VoiceType.Koma:
                m_voiceKomaSource.cueName = name;
                m_voiceKomaSource.volume = m_voiceVolume;
                m_voiceKomaSource.Play();
                break;
        }
    }

    void Setup()
    {
        m_bgmSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<CriAtomSource>();
        m_seSource = GameObject.FindGameObjectWithTag("SE").GetComponent<CriAtomSource>();
        m_voiceBoySource = GameObject.FindGameObjectWithTag("VOICE_Boy").GetComponent<CriAtomSource>();
        m_voiceGirlSource = GameObject.FindGameObjectWithTag("VOICE_Girl").GetComponent<CriAtomSource>();
        m_voiceKomaSource = GameObject.FindGameObjectWithTag("VOICE_Koma").GetComponent<CriAtomSource>();
        SettingVolume();
    }

    public void SettingVolume()
    {
        m_bgmSource.volume = m_bgmVolume;
        m_seSource.volume = m_seVolume;
        m_voiceBoySource.volume = m_voiceVolume;
        m_voiceGirlSource.volume = m_voiceVolume;
        m_voiceKomaSource.volume = m_voiceVolume;
    }

    IEnumerator SwitchBGM(string bgmName)
    {
        while (m_bgmSource.volume > 0)
        {
            m_bgmSource.volume -= Time.deltaTime * m_switchSpeed;
            yield return null;
        }
        m_bgmSource.Stop();
        m_bgmSource.cueName = bgmName;
        m_bgmSource.Play();
        while (m_bgmSource.volume < 1)
        {
            m_bgmSource.volume += Time.deltaTime * m_switchSpeed;
            yield return null;
        }
    }
}
