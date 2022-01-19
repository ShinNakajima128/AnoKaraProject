using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// HPの管理を行うScript
/// </summary>
public class HPController : MonoBehaviour
{
    [SerializeField]
    GameObject m_hpObject = default;

    [SerializeField]
    int m_maxHP = 5;

    bool m_init = false;
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
        //HPを指定の数だけ生成
        if (!m_init)
        {
            m_currentHPObjects = new GameObject[CurrentHP];

            for (int i = 0; i < m_currentHPObjects.Length; i++)
            {
                m_currentHPObjects[i] = Instantiate(m_hpObject, transform);
            }
            Debug.Log(m_currentHPObjects.Length);
            m_init = true;
        }

        //体力が5以下かつ、現在のクイズで不正解だった場合
        if (CurrentHP < 5 && m_currentHPObjects[CurrentHP].activeSelf)
        {
            var hpObj = m_currentHPObjects[CurrentHP];
            hpObj.transform
                 .DOScale(new Vector3(2f, 2f, 2f), 0.5f)
                 .OnComplete(() =>
                 {
                     SoundManager.Instance.PlaySe("SE_popup");
                     hpObj.transform
                          .DOScale(Vector3.zero, 0.15f)
                          .OnComplete(() =>
                          {
                              hpObj.gameObject.SetActive(false);
                          });
                 });
        }
    }
}
