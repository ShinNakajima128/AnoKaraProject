using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest_Akmt : MonoBehaviour
{
    [SerializeField]
    string m_name;
    [SerializeField]
    int m_swtSpeed;

    void Start()
    {
        SoundManager.Instance.PlayBgm(m_name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //SoundManager.Instance.PlayVoice(VoiceType.Player, m_name);
        }
    }
}
