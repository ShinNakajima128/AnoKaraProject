using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : SingletonMonoBehaviour<SoundTest>
{
    CriAtomSource m_bgmSource = default;
    CriAtomSource m_seSource = default;
    CriAtomSource m_voiceBoySource = default;
    CriAtomSource m_voiceGirlSource = default;
    CriAtomSource m_voiceKomaSource = default;


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Setup();
        m_bgmSource.loop = true;
        m_bgmSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_seSource.Play("voice001");
        }
    }

    public void PlayBgm()
    {

    }

    public void PlaySe()
    {

    }

    public void PlayBoyVoice()
    {

    }
    public void PlayGirlVoice()
    {

    }
    public void PlayKomaVoice()
    {

    }

    void Setup()
    {
        m_bgmSource = GameObject.FindGameObjectWithTag("BGM").GetComponent<CriAtomSource>();
        m_seSource = GameObject.FindGameObjectWithTag("SE").GetComponent<CriAtomSource>();
        m_voiceBoySource = GameObject.FindGameObjectWithTag("VOICE_Boy").GetComponent<CriAtomSource>();
        m_voiceGirlSource = GameObject.FindGameObjectWithTag("VOICE_Girl").GetComponent<CriAtomSource>();
        m_voiceKomaSource = GameObject.FindGameObjectWithTag("VOICE_Koma").GetComponent<CriAtomSource>();
    }


}
