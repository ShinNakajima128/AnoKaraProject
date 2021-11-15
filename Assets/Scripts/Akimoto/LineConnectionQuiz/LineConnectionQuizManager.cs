using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LCQSide { Left, Right }
public class LineConnectionQuizManager : MonoBehaviour
{
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
    private RectTransform m_lineRect;

    private string m_question;

    /// <summary>ボタン入力フラグ</summary>
    private bool m_isConnected = false;

    /// <summary>座標保存用</summary>
    private Vector3 m_startPos;
    private Vector3 m_endPos;
    //[SerializeField] private Transform canvas;
    private List<GameObject> m_deleteObjs = new List<GameObject>();

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
        for (int i = 0; i < m_LButtonParent.transform.childCount; i++)
        {
            m_LButtons.Add(m_LButtonParent.transform.GetChild(i).GetComponent<LineConnectButton>());
        }
        for (int i = 0; i < m_RButtonParent.transform.childCount; i++)
        {
            m_RButtons.Add(m_RButtonParent.transform.GetChild(i).GetComponent<LineConnectButton>());
        }
    }

    private IEnumerator OnLineQuizQuestion(GameObject panel, Text text)
    {
        yield return null;
    }

    //public void SetTransform(int index, RectTransform transform)
    //{
    //    m_transforms[index] = transform;
    //}

    //public RectTransform GetTransform(int index) { return m_transforms[index]; }

    public void StartLine(Vector3 startPos)
    {
        m_startPos = startPos;
        //GameObject image = Instantiate(_startImage, _startPos, Quaternion.identity).gameObject;
        //image.transform.SetParent(_targetCanvas.transform);

        //m_lineRect = Instantiate(m_lineObj, m_startPos, Quaternion.identity).GetComponent<RectTransform>();
        //_line = _lineObj.GetComponent<RectTransform>();
        //_lineObj.transform.SetParent(_targetCanvas.transform);
    }

    public void EndLine(Vector3 endPos)
    {
        m_endPos = endPos;
        //GameObject image = Instantiate(_endImage, _endPos, Quaternion.identity).gameObject;
        //image.transform.SetParent(_targetCanvas.transform);
    }

    /// <summary>
    /// ２点間の線を引く
    /// </summary>
    public void LineCast()
    {
        //float diffX = mouse.x - _startPos.x;
        //float diffY = mouse.y - _startPos.y;
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

    /*
    // 対象となる Canvas
    [SerializeField]
    Canvas _targetCanvas;
    // 始点となる Image
    [SerializeField]
    Image _startImage;
    // 終点となる Image
    [SerializeField]
    Image _endImage;
    // 描画するためのラインとなる Image
    [SerializeField]
    Image _lineImage;

    RectTransform _line;
    GameObject _lineObj;

    Vector2 _startPos = Vector2.zero;
    Vector2 _endPos = Vector2.zero;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) SetStartPoint();
        if (Input.GetMouseButton(0)) Drow();
        if (Input.GetMouseButtonUp(0)) SetEndPoint();
    }

    void SetStartPoint()
    {
        _startPos = Input.mousePosition;
        GameObject image = Instantiate(_startImage, _startPos, Quaternion.identity).gameObject;
        image.transform.SetParent(_targetCanvas.transform);

        _lineObj = Instantiate(_lineImage, _startPos, Quaternion.identity).gameObject;
        _line = _lineObj.GetComponent<RectTransform>();
        _lineObj.transform.SetParent(_targetCanvas.transform);
    }

    void Drow()
    {
        Vector2 mouse = Input.mousePosition;

        float diffX = mouse.x - _startPos.x;
        float diffY = mouse.y - _startPos.y;

        // 終点となるImageの方向に向かせる
        float angle = Mathf.Atan2(diffY, diffX) * Mathf.Rad2Deg;
        _lineObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 中点の計算
        float mX = (_startPos.x + mouse.x) / 2;
        float mY = (_startPos.y + mouse.y) / 2;
        _lineObj.transform.position = new Vector2(mX, mY);
        float distance = Vector2.Distance(_startPos, mouse);
        _line.sizeDelta = new Vector2(distance, _line.rect.height);
    }

    void SetEndPoint()
    {
        _endPos = Input.mousePosition;
        GameObject image = Instantiate(_endImage, _endPos, Quaternion.identity).gameObject;
        image.transform.SetParent(_targetCanvas.transform);
    }
     */
}