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

    public AudioClip[] BGM_Clips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
