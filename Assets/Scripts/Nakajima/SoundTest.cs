using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField]
    string m_bgmName = default;

    [SerializeField]
    float m_switchSpeed = 1.0f;

    CriAtomSource m_bgmSource = default;
    CriAtomSource m_seSource = default;
    CriAtomSource m_voiceBoySource = default;
    CriAtomSource m_voiceGirlSource = default;
    CriAtomSource m_voiceKomaSource = default;

    public static SoundTest Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Setup();
        PlayBgm(m_bgmName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            m_seSource.Play("voice001");
        }
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
