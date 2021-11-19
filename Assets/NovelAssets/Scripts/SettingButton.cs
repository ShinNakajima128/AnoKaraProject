using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 設定ボタンのアニメーションの管理をする
/// </summary>
public class SettingButton : MonoBehaviour
{
    /// <summary> ButtonのAnimator </summary>
    Animator m_anim = default;
    /// <summary> 設定ボタンがONかOFFか </summary>
    bool m_isActived = false;

    public bool IsActived { get => m_isActived; set => m_isActived = value; }

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 設定ボタンのアニメーションをする
    /// </summary>
    public void SettingButtonAnim()
    {
        //OFFだった場合はメニューを開く
        if (!m_isActived)
        {
            m_isActived = true;
        }
        //ONだった場合はメニューを閉じる
        else
        {
            m_isActived = false;
        }
        m_anim.Play(m_isActived ? "ON" : "OFF");
    }
}
