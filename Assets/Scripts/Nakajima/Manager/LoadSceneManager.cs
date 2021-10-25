using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Scene遷移を管理するクラス
/// </summary>
public class LoadSceneManager : MonoBehaviour
{
    [SerializeField, Header("ロード時に要する時間")]
    float m_LoadTimer = 1.0f;

    [SerializeField, Header("フェードの速度")] 
    float m_fadeSpeedValue = 1.0f;

    [SerializeField, Header("フェード時に使用するImage")]
    Image m_fadeImage = default;
    
    [Header("デバッグ用")]
    [SerializeField]
    bool m_debugMode = false;
    /// <summary> フェードアウトのフラグ </summary>
    [SerializeField]
    bool m_isFadeOut = false;
    /// <summary> フェードインのフラグ </summary>
    [SerializeField]
    bool m_isFadeIn = false;
    #region private member
    /// <summary> 現在のScene </summary>
    static string m_currentScene = "";
    /// <summary> 現在のScene </summary>
    static string m_beforeScene = "";  
    /// <summary> フェードさせるImageのRGBa </summary>
    float m_red, m_green, m_blue, m_alfa;
    /// <summary> 現在稼働中のコルーチン </summary>
    Coroutine m_currentFade = default;
    #endregion 
    public static LoadSceneManager Instance { get; private set; }
    /// <summary> 現在のScene </summary>
    public static string CurrentScene => m_currentScene;
    /// <summary> 直前のScene </summary>
    public static string BeforeScene => m_beforeScene;

    void OnValidate()
    {
        if (m_debugMode)
        {
            if (m_isFadeIn)
            {
                m_currentFade = StartCoroutine(FadeIn());
            }
            else if (m_isFadeOut)
            {
                m_currentFade = StartCoroutine(FadeOut());
            }
        }
    }

    void Awake()
    {
        Instance = this;

        m_alfa = 1;
        SetAlfa();
    }

    void Start()
    {
        m_red = m_fadeImage.color.r;
        m_green = m_fadeImage.color.g;
        m_blue = m_fadeImage.color.b;
        m_alfa = m_fadeImage.color.a;

        m_currentScene = SceneManager.GetActiveScene().name;

        if (m_alfa > 0)
        {
            m_isFadeIn = true;
            m_currentFade = StartCoroutine(FadeIn());
        }
    }

    /// <summary>
    /// 任意のSceneへ遷移する
    /// </summary>
    /// <param name="name"> 遷移先のScene名 </param>
    /// <param name="callBack"> フェード後に呼ばれるコールバック </param>
    public void AnyLoadScene(string name, Action callBack = null)
    {
        m_isFadeOut = true;
        StartCoroutine(LoadScene(name, m_LoadTimer, callBack));
    }

    /// <summary>
    /// Sceneのリスタート。もう一度遊ぶ、やり直すといった機能を使いたい時に使用してください
    /// </summary>
    public void Restart()
    {
        m_isFadeOut = true;
        StartCoroutine(LoadScene(m_currentScene, m_LoadTimer));
    }

    /// <summary>
    /// 前のSceneに遷移する。ゲームを中断する時などに使用してください。
    /// </summary>
    public void LoadBeforeScene()
    {
        m_isFadeOut = true;
        StartCoroutine(LoadScene(m_beforeScene, m_LoadTimer));
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public void QuitGame()
    {
        m_isFadeOut = true;
        StartCoroutine(QuitScene(m_LoadTimer));
    }

    IEnumerator FadeOut(Action callback = null)
    {
        m_fadeImage.raycastTarget = true;
        while (m_isFadeOut)
        {
            if (m_alfa < 1)
            {
                m_alfa += m_fadeSpeedValue * Time.deltaTime;
                SetAlfa();
            }
            else
            {
                m_isFadeOut = false;
                m_fadeImage.raycastTarget = false;
                callback?.Invoke();
            }
            yield return null;
        }
    }

    IEnumerator FadeIn(Action callback = null)
    {
        m_fadeImage.raycastTarget = true;

        while (m_isFadeIn)
        {
            if (m_alfa > 0)
            {
                m_alfa -= m_fadeSpeedValue * Time.deltaTime;
                SetAlfa();
            }
            else
            {
                m_isFadeIn = false;
                m_fadeImage.raycastTarget = false;
                callback?.Invoke();
            }
            yield return null;
        }
    }
    
    public void FadeInPanel()
    {
        m_isFadeIn = true;
        m_currentFade = StartCoroutine(FadeIn());
    }

    public void FadeOutPanel()
    {
        m_isFadeOut = true;
        m_currentFade = StartCoroutine(FadeOut());
    }
    /// <summary>
    /// アルファ値をImageにセットする
    /// </summary>
    void SetAlfa()
    {
        m_fadeImage.color = new Color(m_red, m_green, m_blue, m_alfa);
    }

    IEnumerator LoadScene(string name, float timer, Action callBack = null)
    {
        m_currentFade = StartCoroutine(FadeOut());

        yield return new WaitForSeconds(timer);

        m_beforeScene = SceneManager.GetActiveScene().name; //前のScene名を保持している変数の値を現在のScene名に更新する
        callBack?.Invoke();
        SceneManager.LoadScene(name);
    }

    IEnumerator QuitScene(float timer)
    {
        m_currentFade = StartCoroutine(FadeIn());

        yield return new WaitForSeconds(timer);

        Application.Quit();
    }
}
