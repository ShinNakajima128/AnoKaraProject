using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HPの管理を行うScript
/// </summary>
public class HPController : MonoBehaviour
{
    [SerializeField]
    GameObject m_hpObject = default;

    [SerializeField]
    int m_maxHP = 5;

    GameObject[] m_currentHPObjects = default;
    public static HPController Instance { get; private set; } 
    public int CurrentHP { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentHP = m_maxHP;
        SetHP();
        EventManager.ListenEvents(Events.QuizEnd, SetHP);
    }

    void SetHP()
    {
        if (m_currentHPObjects != null)
        {
            for (int i = 0; i < m_currentHPObjects.Length; i++)
            {
                Destroy(m_currentHPObjects[i]);
            }
            m_currentHPObjects = null;
        }

        m_currentHPObjects = new GameObject[CurrentHP];
        
        for (int i = 0; i < m_currentHPObjects.Length; i++)
        {
            m_currentHPObjects[i] = Instantiate(m_hpObject, transform);
        }
    }
}
