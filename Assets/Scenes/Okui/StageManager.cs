using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class StageManager : MonoBehaviour
{
    /// <summary>非アクティブにしてるパネル</summary>
    [SerializeField]
    GameObject m_panel;

    /// <summary>UIのAnimator</summary>
    [SerializeField]
    Animator m_animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
        SoundManager.sound.PlayBGM(SoundManager.BGM_Type.BGM);

    }
    /// <summary>
    /// 戻るボタン
    /// </summary>
    public void BackButton()
    {
        Debug.Log("push");
        SceneManager.LoadScene("StudyPart");
    }
    /// <summary>
    /// stage１のボタン
    /// </summary>
    public void PushStage1()
    {
        m_animator.SetFloat("Speed", 1);
        m_panel.SetActive(true);
    }
    /// <summary>
    /// アクティブのパネルをAnimationから逆再生する関数
    /// </summary>
    public void PopupClose()
    {
        SoundManager.sound.PlayBGM(SoundManager.BGM_Type.BGM2);
        m_animator.SetFloat("Speed", -1);
    }

}
