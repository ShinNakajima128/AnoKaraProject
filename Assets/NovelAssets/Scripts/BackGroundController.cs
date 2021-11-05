using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BackGroundController : MonoBehaviour
{
    [SerializeField]
    Image[] m_backgrounds = default;

    [SerializeField]
    Sprite[] m_backgroundTypes = default;

    int m_currentBackground = 0;
    Animator m_anim = default;
    public static Action BackgroundAnim = default;

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    public void Setup(int backgroundType)
    {
        m_backgrounds[0].sprite = m_backgroundTypes[backgroundType];
    }

    public void FadeIn(int backgroundType)
    {
        m_backgrounds[1].sprite = m_backgroundTypes[backgroundType];
        m_anim.Play("BackgroundFadeIn");
    }

    public void Crossfade(int backgroundType)
    {
        if (m_currentBackground == 0)
        {
            m_backgrounds[1].sprite = m_backgroundTypes[backgroundType];
            m_anim.Play("BackgroundAnimation1");
            m_currentBackground = 1;
        }
        else
        {
            m_backgrounds[0].sprite = m_backgroundTypes[backgroundType];
            m_anim.Play("BackgroundAnimation2");
            m_currentBackground = 0;
        }
    }

    public void OnAnimationFinish()
    {
        BackgroundAnim?.Invoke();
    }
}
