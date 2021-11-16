using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterData;

public class LineConnectionQuizManager : MonoBehaviour
{
    [SerializeField]
    Text m_viewQuestion;

    /// <summary>線繋ぎ問題のデータベース</summary>
    [SerializeField]
    LineConnectionQuizDatabase[] m_database;

    [SerializeField]
    List<string> m_years = new List<string>();
    [SerializeField]
    List<string> m_events = new List<string>();

    private int m_currentNum = 0;

    /// <summary>ゲームパネル</summary>
    [SerializeField]
    GameObject m_gamePanel;

    /// <summary>左のボタンの親</summary>
    [SerializeField]
    GameObject m_LButtonParent;

    /// <summary>左のボタンのプレハブ</summary>
    [SerializeField]
    GameObject m_lbuttonPrefab;

    /// <summary>左ボタン達</summary>
    List<LineConnectButton> m_lbuttons = new List<LineConnectButton>();

    /// <summary>右のボタンの親</summary>
    [SerializeField]
    GameObject m_RButtonParent;

    /// <summary>右のボタンのプレハブ</summary>
    [SerializeField]
    GameObject m_rbuttonPrefab;

    /// <summary>右ボタン達</summary>
    List<LineConnectButton> m_rbuttons = new List<LineConnectButton>();

    /// <summary>線となるオブジェクト</summary>
    [SerializeField]
    GameObject m_lineObj;

    /// <summary>ボタン入力フラグ</summary>
    bool m_isConnected = false;

    /// <summary>座標保存用</summary>
    Vector3 m_startPos;
    Vector3 m_endPos;
    //選択されたボタン保存用
    LineConnectButton m_leftButton;
    LineConnectButton m_rightButton;
    //プレイヤーの答え保存用
    List<string> m_results = new List<string>();
    //正しい答え
    private string m_correctAnswer;

    /// <summary>配置したオブジェクト削除用</summary>
    List<GameObject> m_deleteObjs = new List<GameObject>();

    public bool IsConnected { get => m_isConnected; set => m_isConnected = value; }
    public static LineConnectionQuizManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var currentPeriod = GameManager.Instance.CurrentPeriod;
        Debug.Log(currentPeriod);
        var quizDatas = DataManager.Instance.LineConnectionQuizDatas; //線繋ぎクイズのデータ取得

        for (int i = 0; i < quizDatas.Length; i++)
        {
            if (quizDatas[i].PeriodType == currentPeriod)
            {
                m_database = quizDatas[i].LineConnectQuizDatabases;
                break;
            }
        }
        RandomlySorted(m_database);
        StartCoroutine(OnLineQuizQuestion(m_gamePanel, m_viewQuestion));
        //Setup();
        //Create();
    }

    /// <summary>
    /// 値の初期化と設定
    /// </summary>
    private void Setup()
    {
        m_startPos = Vector3.zero;
        m_endPos = Vector3.zero;
        m_isConnected = false;
        m_years.Clear();
        m_events.Clear();
        m_lbuttons.Clear();
        m_rbuttons.Clear();
        m_leftButton = null;
        m_rightButton = null;
        for (int i = 0; i < m_database[m_currentNum].YearsArray.Length; i++)
        {
            //m_years.Add(m_database[m_currentNum].YearsArray[i]); //この二つはテスト用
            //m_events.Add(m_database[m_currentNum].EventsArray[i]);
            m_correctAnswer += m_database[m_currentNum].YearsArray[i] + m_database[m_currentNum].EventsArray[i];
        }
        //データの並び替え
        List<string> yearList = new List<string>();
        for (int i = 0; i < m_database[m_currentNum].YearsArray.Length; i++)
        {
            yearList.Add(m_database[m_currentNum].YearsArray[i]);
        }
        for (int i = 0; i < yearList.Count; i++)
        {
            int random = Random.Range(0, yearList.Count);
            string temp = yearList[i];
            yearList[i] = yearList[random];
            yearList[random] = temp;
        }
        List<string> eventList = new List<string>();
        for (int i = 0; i < m_database[m_currentNum].EventsArray.Length; i++)
        {
            eventList.Add(m_database[m_currentNum].EventsArray[i]);
        }
        for (int i = 0; i < eventList.Count; i++)
        {
            int random = Random.Range(0, eventList.Count);
            string temp = eventList[i];
            eventList[i] = eventList[random];
            eventList[random] = temp;
        }
        for (int i = 0; i < yearList.Count; i++)
        {
            m_years.Add(yearList[i]);
            m_events.Add(eventList[i]);
        }
    }

    private IEnumerator OnLineQuizQuestion(GameObject panel, Text text)
    {
        if (m_currentNum >= m_database.Length)
        {
            yield break;
        }
        else
        {
            QuizManager.Instance.QuizDataUpdated = true;
            panel.SetActive(true);
            //問題画面の表示
            m_viewQuestion.text = m_database[m_currentNum].Question;
            Setup();
            Create();
            //QuizManager.Instance.CorrectAnswer = m_database[m_currentNum].YearsArray[]
        }
    }

    private void RandomlySorted(LineConnectionQuizDatabase[] quizzes)
    {
        for (int i = 0; i < quizzes.Length; i++)
        {
            int random = Random.Range(0, quizzes.Length);
            var temp = m_database[i];
            m_database[i] = m_database[random];
            m_database[random] = temp;
        }
    }

    private void Create()
    {
        for (int i = 0; i < m_years.Count; i++) //左側のボタン(年号側)の設定
        {
            GameObject obj = Instantiate(m_lbuttonPrefab);
            obj.transform.SetParent(m_LButtonParent.transform, false);
            m_deleteObjs.Add(obj);
            LineConnectButton line = obj.GetComponent<LineConnectButton>();
            line.m_side = 0;
            line.m_text = m_years[i];
            line.SetText();
            m_lbuttons.Add(line);
        }
        for (int i = 0; i < m_events.Count; i++) //右側のボタン(出来事)の設定
        {
            GameObject obj = Instantiate(m_rbuttonPrefab);
            obj.transform.SetParent(m_RButtonParent.transform, false);
            m_deleteObjs[i] = obj;
            LineConnectButton line = obj.GetComponent<LineConnectButton>();
            line.m_side = 1;
            line.m_text = m_events[i];
            line.SetText();
            m_rbuttons.Add(line);
        }
    }

    /// <summary>
    /// 生成したオブジェクトの破棄
    /// </summary>
    private void Delete()
    {
        foreach (var item in m_deleteObjs)
        {
            Destroy(item);
        }
    }
    
    public void StartLine(Vector3 pos, LineConnectButton button)
    {
        m_startPos = pos;
        if (button.m_side == 0) { m_leftButton = button; }
        else m_rightButton = button;
    }

    public void EndLine(Vector3 pos, LineConnectButton button)
    {
        m_endPos = pos;
        if (button.m_side == 0) { m_leftButton = button; }
        else m_rightButton = button;
    }

    /// <summary>
    /// ２点間の線を引く
    /// </summary>
    public void LineCast()
    {
        if (m_leftButton == null || m_rightButton == null)
        {
            m_leftButton = null;
            m_rightButton = null;
            return;
        }
        if ((m_leftButton.m_side == m_rightButton.m_side) || m_leftButton.m_isConnect || m_rightButton.m_isConnect)
        {
            m_leftButton = null;
            m_rightButton = null;
            return;
        }
        m_leftButton.m_isConnect = true;
        m_rightButton.m_isConnect = true;
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

        m_results.Add(m_leftButton.m_text + m_rightButton.m_text);
        m_leftButton = null;
        m_rightButton = null;
    }

    public void CrearCheck()
    {
        for (int i = 0; i < m_years.Count; i++)
        {
            for (int n = 0; n <= m_years.Count; n++)
            {
                if (n == m_years.Count)
                {
                    Debug.Log("不正解");
                    return;
                }
                if (m_results[i] == m_database[m_currentNum].YearsArray[n] + m_database[m_currentNum].EventsArray[n])
                {
                    break;
                }
            }
        }
        Debug.Log("正解");
    }
}