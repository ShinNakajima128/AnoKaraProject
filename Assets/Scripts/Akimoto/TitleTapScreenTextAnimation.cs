using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTapScreenTextAnimation : MonoBehaviour
{
    Animator m_anim;

    private void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    public void OnAnimation()
    {
        m_anim.Play("FadeOut");
    }

    public void FinishAnimation()
    {
        TitleManager.Instance.IsAnim = true;
    }
}
