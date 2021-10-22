using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject panel;
    
   public void backButton()
    {
        Debug.Log("push");
        SceneManager.LoadScene("StudyPart");
    }
   public void pushStage1()
    {
        panel.SetActive(true);
    }
    
}
