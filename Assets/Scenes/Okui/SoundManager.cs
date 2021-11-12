using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/// <summary>
/// 音源管理
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager sound;

    /// <summary>
    /// BGN管理
    /// </summary>
    public enum BGM_Type
    {
        BGM,
        BGM2,
        BGM3,
    }
    //SE管理
    public enum SE_Type
    {
        SE,
        SE2,
        SE3,
    }
    /// <summary>AudioClip</summary>
    /// 
    [SerializeField]
    AudioClip[] BGM_Clips;
    [SerializeField]
    AudioClip[] SE_cliip;
    [SerializeField]
    AudioMixerGroup m_mixer;

    /// <summary>AudioSource</summary>
    private AudioSource[] bgmSources = new AudioSource[2];
    private AudioSource[] seSources = new AudioSource[10];

    private int currentBgmIndex = 999;


    private void Awake()
    {
        if (sound == null)
        {
            sound = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        bgmSources[0] = gameObject.AddComponent<AudioSource>();
        bgmSources[0].outputAudioMixerGroup = m_mixer;

    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayBGM(BGM_Type bgmType,bool loopFlg=true)
    {

        int index = (int)bgmType;
        currentBgmIndex = index;

        if(index<0||BGM_Clips.Length<=0)
        {
            return;
        }
        //BGM再生開始
        bgmSources[0].clip= BGM_Clips[index];
        bgmSources[0].Play();
        
    }
}
