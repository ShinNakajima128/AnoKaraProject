using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGameManager : MonoBehaviour
{
    /// <summary>ステージのオブジェクト</summary>
    [SerializeField]
    Transform m_stageObj;

    /// <summary>ボールのオブジェクト</summary>
    [SerializeField]
    GameObject m_ball;

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
        GameStart();
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
            if (m_stageObj.childCount == 0)
            {
                GameClear();
            }
        }
    }

    /// <summary>
    /// BallControllerの開始する関数を呼ぶ
    /// </summary>
    void GameStart()
    {
        m_isGame = true;
        m_ballCon.StartPush();
    }

    /// <summary>リスタートする時の処理</summary>
    void ReStart()
    {
        Debug.Log("リスタート");
        m_ball.SetActive(true);
        GameStart();
    }

    /// <summary>クリアした時の処理</summary>
    void GameClear()
    {
        m_isGame = false;
        Debug.Log("ゲームクリア");
    }

    /// <summary>ゲームオーバーした時の処理</summary>
    void GameOver()
    {
        m_isGame = false;
        Debug.Log("ゲームオーバー");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            collision.gameObject.SetActive(false);
            collision.gameObject.transform.position = new Vector3(0, -3, 0);
            GameOver();
        }
    }
}
