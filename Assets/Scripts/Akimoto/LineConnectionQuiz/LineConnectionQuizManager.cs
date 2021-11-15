using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineConnectionQuizManager : MonoBehaviour
{
    /// <summary>問題文</summary>
    [SerializeField]
    private string m_question;

    [SerializeField]
    private Text m_viewQuestion;

    /// <summary>選択肢</summary>
    [SerializeField]
    private string m_choices;

    [SerializeField]
    private string[] m_periods = new string[4];
    [SerializeField]
    private string[] m_dekigoto = new string[4];

    /// <summary>ゲームパネル</summary>
    [SerializeField]
    GameObject m_gamePanel;

    /// <summary>左のボタンの親</summary>
    [SerializeField]
    GameObject m_LButtonParent;

    /// <summary>左ボタン達</summary>
    List<LineConnectButton> m_LButtons = new List<LineConnectButton>();

    /// <summary>右のボタンの親</summary>
    [SerializeField]
    GameObject m_RButtonParent;

    /// <summary>右ボタン達</summary>
    List<LineConnectButton> m_RButtons = new List<LineConnectButton>();

    /// <summary>線となるオブジェクト</summary>
    [SerializeField]
    GameObject m_lineObj;
    //private RectTransform m_lineRect;

    /// <summary>ボタン入力フラグ</summary>
    private bool m_isConnected = false;

    /// <summary>座標保存用</summary>
    private Vector3 m_startPos;
    private Vector3 m_endPos;
    //Side番号保存用
    private int m_startNum;
    private int m_endNum;

    /// <summary>配置したオブジェクト削除用</summary>
    private List<GameObject> m_deleteObjs = new List<GameObject>();

    public bool IsConnected { get => m_isConnected; set => m_isConnected = value; }
    public static LineConnectionQuizManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Setup();
        for (int i = 0; i < m_LButtonParent.transform.childCount; i++) //左側のボタン(年号側)の設定
        {
            LineConnectButton line = m_LButtonParent.transform.GetChild(i).GetComponent<LineConnectButton>();
            line.Side = 0;
            line.m_text = m_periods[i];
            line.SetText();
            m_LButtons.Add(line);
        }
        for (int i = 0; i < m_RButtonParent.transform.childCount; i++) //右側のボタン(出来事)の設定
        {
            LineConnectButton line = m_RButtonParent.transform.GetChild(i).GetComponent<LineConnectButton>();
            line.Side = 1;
            line.m_text = m_dekigoto[i];
            line.SetText();
            m_RButtons.Add(line);
        }
    }

    private void Setup()
    {
        m_startPos = Vector3.zero;
        m_endPos = Vector3.zero;
        m_isConnected = false;
    }

    private IEnumerator OnLineQuizQuestion(GameObject panel, Text text)
    {
        yield return null;
    }

    public void StartLine(Vector3 startPos, int startNum)
    {
        m_startPos = startPos;
        m_startNum = startNum;
    }

    public void EndLine(Vector3 endPos, int endNum)
    {
        m_endPos = endPos;
        m_endNum = endNum;
    }

    /// <summary>
    /// ２点間の線を引く
    /// </summary>
    public void LineCast()
    {
        if (m_startNum == m_endNum) return;
        float x = m_endPos.x - m_startPos.x;
        float y = m_endPos.y - m_startPos.y;

        GameObject line = Instantiate(m_lineObj);
        line.transform.SetParent(m_gamePanel.transform);
        // 終点となるImageの方向に向かせる
        float angle = Mathf.Atan2(x, y) * Mathf.Rad2Deg;
        line.transform.rotation = Quaternion.Euler(0, 0, -angle - 90);

        // 中点の計算
        float mX = (m_startPos.x + m_endPos.x) / 2;
        float mY = (m_startPos.y + m_endPos.y) / 2;
        line.transform.position = new Vector2(mX, mY);
        float distance = Vector2.Distance(m_startPos, m_endPos);
        RectTransform rect = line.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(distance, rect.rect.height);
    }

    public void CrearCheck()
    {
        for (int i = 0; i < 4; i++)
        {
            string t = m_LButtons[i].m_text + m_RButtons[i].m_text;
            Debug.Log(t);
        }
    }
}