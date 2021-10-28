using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Animator animator;
    private void Start()
    {
        //animator = GetComponent<Animator>();
    }
    public void backButton()
    {
        Debug.Log("push");
        SceneManager.LoadScene("StudyPart");
    }
    public void pushStage1()
    {
        animator.SetFloat("Speed", 1);
        panel.SetActive(true);
    }
    public void PopupCroze()
    {
        animator.SetFloat("Speed", -1);
    }

}
