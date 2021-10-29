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
    }
    public void BackButton()
    {
        Debug.Log("push");
        SceneManager.LoadScene("StudyPart");
    }
    public void PushStage1()
    {
        m_animator.SetFloat("Speed", 1);
        m_panel.SetActive(true);
    }
    public void PopupClose()
    {
        m_animator.SetFloat("Speed", -1);
    }

}
