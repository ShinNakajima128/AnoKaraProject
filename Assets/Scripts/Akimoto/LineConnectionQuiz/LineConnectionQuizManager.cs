using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Side { Left, Right }
public class LineConnectionQuizManager : MonoBehaviour
{
    /// <summary>ゲームパネル</summary>
    [SerializeField]
    GameObject m_gamePanel;

    /// <summary>左のボタン達</summary>
    [SerializeField]
    GameObject m_LButtons;

    /// <summary>右のボタン達</summary>
    [SerializeField]
    GameObject m_RButtons;

    /// <summary>ボタンを押した状態かどうか</summary>
    private bool m_isConnected = false;

    private RectTransform[] m_transforms = new RectTransform[2];

    /// <summary>自分</summary>
    private LineConnectionQuizManager m_lcqm;

    public bool IsConnected { get => m_isConnected; set => m_isConnected = value; }
    public static LineConnectionQuizManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    private IEnumerator OnLineQuizQuestion(GameObject panel, Text text)
    {
        yield return null;
    }

    public void SetTransform(int index, RectTransform transform)
    {
        m_transforms[index] = transform;
    }

    public RectTransform GetTransform(int index) { return m_transforms[index]; }

    /// <summary>
    /// ２点間の線を引く
    /// </summary>
    public void LineCast()
    {
        Vector3[] v = new Vector3[2];
        for (int i = 0; i < m_transforms.Length; i++)
        {
            v[i] = m_transforms[i].anchoredPosition;
        }
        GetComponent<LineRenderer>().SetPositions(v);
    }
}