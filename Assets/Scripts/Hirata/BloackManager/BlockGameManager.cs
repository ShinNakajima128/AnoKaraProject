using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>ブロック崩しのゲームマネージャー</summary>
public class BlockGameManager : MonoBehaviour
{
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

    /// <summary>BallController</summary>
    BallController m_ballCon;

    /// <summary>ゲーム中か否か</summary>
    bool m_isGame = false;

    /// <summary>スタートボタン</summary>
    [SerializeField]
    Button m_startButton;

    /// <summary>シェイクボタン</summary>
    [SerializeField]
    Button m_shakeButton;

    /// <summary>シェイクボタンテキスト</summary>
    [SerializeField]
    Text m_shakeButtonText;

    /// <summary>シェイクボタンのクールタイム</summary>
    [SerializeField]
    float m_shakeCoolTime = 5;

    /// <summary>シェイクできるか否か</summary>
    bool m_isShake = true;

    void Awake()
    {
        m_ballCon = m_ball.GetComponent<BallController>();
    }

    void Start()
    {
        SetObj(m_stagePrefab, m_sprite);
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
    /// 次のシーン移動する
    /// </summary>
    public void SceneMove(string scene)
    {
        LoadSceneManager.AnyLoadScene(scene, () =>
        {
            LoadSceneManager.FadeOutPanel();
        });
    }

    /// <summary>
    /// ステージと画像を設定する
    /// ゲーム開始前に呼んでもらう
    /// </summary>
    public void SetObj(GameObject obj, Sprite sprite)
    {
        StartCoroutine(ButtonLock());
        m_stageObj = Instantiate(obj);
        m_stageObj.transform.position = Vector3.zero;
        m_targetSprite.sprite = sprite;
    }

    /// <summary>
    /// ゲームを開始する関数
    /// ボタンに設定
    /// </summary>
    public void GameStart()
    {
        m_isGame = true;
        m_startButton.interactable = false;
        m_ballCon.StartPush();
    }

    /// <summary>
    /// クリアした時の処理
    /// </summary>
    void GameClear()
    {
        m_isGame = false;
        m_startButton.interactable = false;
        StartCoroutine(ButtonLock());
        m_gameClearUi.SetActive(true);
    }

    /// <summary>
    /// ボールを下に落としてリスタートする時の処理
    /// </summary>
    void Restart()
    {
        m_ball.gameObject.SetActive(false);
        m_ball.gameObject.transform.position = new Vector3(0, -3, 0);
        m_ball.gameObject.SetActive(true);
        m_isGame = false;
        m_isShake = true;
        StartCoroutine(ButtonLock());
        m_startButton.interactable = true;
    }

    /// <summary>
    /// シェイクボタンのクールタイム
    /// シェイクボタンに設定する
    /// </summary>
    public void IsShake()
    {
        if (m_isShake)
        {
            m_isShake = false;
            m_shakeButton.interactable = false;
            StartCoroutine(ShakeTimer());
        }
    }

    /// <summary>
    /// シェイクボタンのクールダウンタイマー
    /// </summary>
    /// <returns></returns>
    IEnumerator ShakeTimer()
    {
        float timer = m_shakeCoolTime;
        while (!m_isShake)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                m_shakeButtonText.text = timer.ToString("f1");
            }
            else
            {
                m_isShake = true;
                m_shakeButtonText.text = "0";
            }
            yield return new WaitForEndOfFrame();
        }
        m_shakeButton.interactable = true;
        m_shakeButtonText.text = "Shake";
        yield break;
    }

    /// <summary>
    /// シェイクボタンにゲーム開始までロックをかける
    /// </summary>
    /// <returns></returns>
    IEnumerator ButtonLock()
    {
        while (!m_isGame)
        {
            m_shakeButton.interactable = false;
            yield return new WaitForEndOfFrame();
        }
        m_shakeButton.interactable = true;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Restart();
        }
    }
}
