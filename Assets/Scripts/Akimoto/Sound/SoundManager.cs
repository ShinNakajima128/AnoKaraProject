using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoiceType
{
    Boy,
    Giri,
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

    public static SoundManager Instance;
    public string Name { set => m_bgmName = value; }
    public float SwitchSpeed { set => m_switchSpeed = value; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Setup();
    }

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

    public void PlaySe(string name)
    {
        m_bgmSource.loop = false;
        m_seSource.cueName = name;
        m_seSource.Play();
    }

    public void PlayVoice(VoiceType voiceType, string name)
    {
        m_bgmSource.loop = false;
        switch (voiceType)
        {
            case VoiceType.Boy:
                m_voiceBoySource.cueName = name;
                m_voiceBoySource.Play();
                break;
            case VoiceType.Giri:
                m_voiceGirlSource.cueName = name;
                m_voiceGirlSource.Play();
                break;
            case VoiceType.Koma:
                m_voiceKomaSource.cueName = name;
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
