using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGameManager : MonoBehaviour
{
    /// <summary>デバッグモード</summary>
    [SerializeField]
    bool m_isDebug = false;

    /// <summary>ステージのオブジェクト</summary>
    GameObject m_stageObj;

    /// <summary>ステージのプレファブ</summary>
    [SerializeField]
    GameObject m_stagePrefab;

    /// <summary>目標の画像</summary>
    [SerializeField]
    Sprite m_sprite;

    /// <summary>目標の画像置き場</summary>
    [SerializeField]
    SpriteRenderer m_targetSprite;

    /// <summary>ボールのオブジェクト</summary>
    [SerializeField]
    GameObject m_ball;

    /// <summary>ゲームクリアした時のUI</summary>
    [SerializeField]
    GameObject m_gameClearUi;

    /// <summary>ゲームオーバーした時のUI</summary>
    [SerializeField]
    GameObject m_gameOverUi;

    /// <summary>BallController</summary>
    BallController m_ballCon;

    /// <summary>ゲーム中か否か</summary>
    bool m_isGame = false;

    void Awake()
    {
        m_ballCon = m_ball.GetComponent<BallController>();
    }

    void Start()
    {
        SetObj(m_stagePrefab, m_sprite);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        ClearCheck();
    }

    /// <summary>
    /// クリアチェック
    /// </summary>
    void ClearCheck()
    {
        if (m_isGame)
        {
            if (m_stageObj.transform.childCount == 0)
            {
                m_ball.SetActive(false);
                GameClear();
            }
        }
    }

    /// <summary>
    /// ステージと画像を設定する
    /// ゲーム開始前に呼んでもらう
    /// </summary>
    public void SetObj(GameObject obj, Sprite sprite)
    {
        m_stageObj = Instantiate(obj);
        m_stageObj.transform.position = Vector3.zero;
        m_targetSprite.sprite = sprite;
    }

    /// <summary>
    /// ゲームを開始する関数
    /// SetObjを呼んでから、この関数を呼ぶ
    /// </summary>
    public void GameStart()
    {
        m_isGame = true;
        m_ballCon.StartPush();
    }

    /// <summary>リスタートする時の処理</summary>
    public void ReStart()
    {
        m_gameClearUi.SetActive(false);
        m_gameOverUi.SetActive(false);
        Debug.Log("リスタート");
        m_ball.SetActive(true);
        SetObj(m_stagePrefab, m_sprite);
    }

    /// <summary>クリアした時の処理</summary>
    void GameClear()
    {
        m_isGame = false;
        m_gameClearUi.SetActive(true);
        Debug.Log("ゲームクリア");
    }

    /// <summary>ゲームオーバーした時の処理</summary>
    void GameOver()
    {
        m_ball.gameObject.SetActive(false);
        m_ball.gameObject.transform.position = new Vector3(0, -3, 0);
        m_isGame = false;
        m_gameOverUi.SetActive(true);
        Debug.Log("ゲームオーバー");
        if (m_isDebug)
        {
            ReStart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            GameOver();
        }
    }
}
