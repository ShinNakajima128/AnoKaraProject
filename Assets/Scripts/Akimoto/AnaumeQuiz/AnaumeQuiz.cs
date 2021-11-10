using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterData;

public class AnaumeQuiz : MonoBehaviour
{
    /// <summary>データベース</summary>
    [SerializeField]
    AnaumeQuizData m_database;

    /// <summary>問題文テキスト</summary>
    [SerializeField]
    Text m_questionViewText;

    /// <summary>ドロップされるオブジェクトの場所</summary>
    [SerializeField]
    Transform m_dropPos;

    /// <summary>ドラッグするオブジェクトの場所</summary>
    [SerializeField]
    Transform m_dragPos;

    /// <summary>ドロップされるプレハブ</summary>
    [SerializeField]
    GameObject m_dropPrefab;

    /// <summary>ドラッグするプレハブ</summary>
    [SerializeField]
    GameObject m_dragPrefab;

    private int m_currentNum = default;

    private List<DropText> m_dropLists = new List<DropText>();

    private AnaumeQuizDatabase m_data;

    public static AnaumeQuiz Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //var currentPeriod = GameManager.Instance.CurrentPeriod;

        //DataManagerに穴埋め問題のデータ取れるようになったらアンコメントする
        //var quizDatas = DataManager.Instance.AnaumeQuizDatas; //穴埋めデータ取得

        m_data = m_database.AnaumeQuizDatabases[0];
        m_questionViewText.text = m_data.Question;
        Create();
    }

    public IEnumerator OnAnaumeQuizQuestion(GameObject panel, Text question)
    {
        if (m_currentNum >= m_database.AnaumeQuizDatabases.Length)
        {
            yield break;
        }
    }

    /// <summary>
    /// 動かせるテキストとドロップされるテキストの生成
    /// </summary>
    private void Create()
    {
        for (int i = 0; i < m_data.Answer.Length; i++) //ドロップされる文字の配置
        {
            GameObject obj = Instantiate(m_dropPrefab);
            obj.transform.SetParent(m_dropPos);
            DropText dropText = obj.GetComponent<DropText>();
            dropText.Text = m_data.Answer[i].ToString();
            m_dropLists.Add(dropText);
            //dropText.GameManager = this;
        }
        for (int i = 0; i < m_data.Dragtext.Length; i++) //移動できる文字の配置
        {
            GameObject obj = Instantiate(m_dragPrefab, m_dragPos);
            obj.transform.SetParent(m_dragPos);
            DragText dragText = obj.GetComponent<DragText>();
            dragText.Text = m_data.Dragtext[i].ToString();
        }
    }

    /// <summary>
    /// 成功判定
    /// </summary>
    public void CrearCheck()
    {
        string t = default;
        foreach (var item in m_dropLists)
        {
            t += item.Text;
        }
        Debug.Log("回答" + t);
        if (m_data.Answer == t) Debug.Log("正解");
        else Debug.Log("ばーか");
    }
}
