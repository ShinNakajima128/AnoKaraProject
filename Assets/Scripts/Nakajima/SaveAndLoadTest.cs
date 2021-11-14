using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAndLoadTest : MonoBehaviour
{
    public static SaveAndLoadTest Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    

    
}