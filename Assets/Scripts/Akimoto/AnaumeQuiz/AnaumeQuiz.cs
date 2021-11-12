using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MasterData;

public class AnaumeQuiz : MonoBehaviour
{
    /// <summary>問題文テキスト</summary>
    [SerializeField]
    Text m_questionViewText;

    [SerializeField]
    GameObject m_gamePanel;

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

    /// <summary>現在何問目か</summary>
    private int m_currentNum = default;

    /// <summary>ドロップされるオブジェクト保存用
    /// 正解判定に使う</summary>
    private List<DropText> m_dropLists = new List<DropText>();

    /// <summary>生成したオブジェクトのリスト<br/>消す時に使う</summary>
    private List<GameObject> m_objList = new List<GameObject>();

    private AnaumeQuizDatabase[] m_anaumeQuizdatas = default;

    public static AnaumeQuiz Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        var currentPeriod = GameManager.Instance.CurrentPeriod;
        Debug.Log(currentPeriod);
        var quizDatas = DataManager.Instance.AnaumeQuizDatabases; //穴埋めデータ取得

        for (int i = 0; i < quizDatas.Length; i++)
        {
            if (quizDatas[i].PeriodType == currentPeriod)
            {
                m_anaumeQuizdatas = quizDatas[i].AnaumeQuizDatabases;
                break;
            }
        }
        RandomlySorted(m_anaumeQuizdatas);
        //StartCoroutine(OnAnaumeQuizQuestion(m_gamePanel, m_questionViewText)); //とりあえず
        //Create(); //とりあえず
    }

    public IEnumerator OnAnaumeQuizQuestion(GameObject panel, Text question)
    {
        if (m_currentNum >= m_anaumeQuizdatas.Length)
        {
            yield break;
        }
        else
        {
            QuizManager.Instance.QuizDataUpdated = true;
            panel.SetActive(true);
            //問題画面の表示
            question.text = m_anaumeQuizdatas[m_currentNum].Question;
            Create();
            QuizManager.Instance.CorrectAnswer = m_anaumeQuizdatas[m_currentNum].Answer;
        }
        yield return QuizManager.Instance.TimeLimit();

        if (!QuizManager.Instance.IsAnswered)
        {
            QuizManager.Instance.IsAnswered = true;
            QuizManager.Instance.PlayerAnswer = "";
        }
        yield return new WaitUntil(() => QuizManager.Instance.IsAnswered);

        yield return QuizManager.Instance.Judge();
        QuizManager.Instance.CurrentTurnNum++;
        m_currentNum++;
        Delete();
    }

    private void RandomlySorted(AnaumeQuizDatabase[] quizzes)
    {
        for (int i = 0; i < quizzes.Length; i++)
        {
            int random = Random.Range(0, quizzes.Length);
            var temp = m_anaumeQuizdatas[i];
            m_anaumeQuizdatas[i] = m_anaumeQuizdatas[random];
            m_anaumeQuizdatas[random] = temp;
        }
    }

    /// <summary>
    /// 動かせるテキストとドロップされるテキストの生成
    /// </summary>
    private void Create()
    {
        m_questionViewText.text = m_anaumeQuizdatas[m_currentNum].Question;
        for (int i = 0; i < m_anaumeQuizdatas[m_currentNum].Answer.Length; i++) //ドロップされる文字の配置
        {
            GameObject obj = Instantiate(m_dropPrefab);
            obj.transform.SetParent(m_dropPos);
            m_objList.Add(obj);
            DropText dropText = obj.GetComponent<DropText>();
            m_dropLists.Add(dropText);
        }
        //並び替え
        List<string> list = new List<string>();
        for (int i = 0; i < m_anaumeQuizdatas[m_currentNum].DragTexts.Length; i++)
        {
            list.Add(m_anaumeQuizdatas[m_currentNum].DragTexts[i]);
        }
        for (int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(0, list.Count);
            string temp = list[i];
            list[i] = list[random];
            list[random] = temp;
        }
        for (int i = 0; i < list.Count; i++) //移動できる文字の配置
        {
            GameObject obj = Instantiate(m_dragPrefab, m_dragPos);
            obj.transform.SetParent(m_dragPos);
            m_objList.Add(obj);
            DragText dragText = obj.GetComponent<DragText>();
            dragText.Text = list[i].ToString();
        }
    }

    /// <summary>
    /// 生成したオブジェクトの破棄
    /// </summary>
    private void Delete()
    {
        foreach (var item in m_objList)
        {
            Destroy(item);
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
        Debug.Log("回答:" + t);
        QuizManager.Instance.PlayerAnswer = t;
        QuizManager.Instance.IsAnswered = true;
        //if (m_anaumeQuizdatas[m_currentNum].Answer == t) Debug.Log("正解");
        //else Debug.Log("不正解");
        //Delete();
    }
}
