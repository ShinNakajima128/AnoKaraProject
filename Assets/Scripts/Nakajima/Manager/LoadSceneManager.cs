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
    [Header("ロード時に要する時間")]
    [SerializeField]
    float m_LoadTimer = 1.0f;

    [Header("フェード時に使用するImage")]
    [SerializeField]
    Image m_fadeImage = default;

    [Header("フェードの速度")]
    [SerializeField] 
    float m_fadeSpeedValue = 1.0f;

    [Header("デバッグ用")]
    [SerializeField]
    bool m_debugMode = false;

    /// <summary> フェードアウトのフラグ </summary>
    [SerializeField]
    bool m_isFadeOut = false;
    /// <summary> フェードインのフラグ </summary>
    [SerializeField]
    bool m_isFadeIn = false;

    /// <summary> 現在のScene </summary>
    static string m_currentScene = "";
    /// <summary> 現在のScene </summary>
    static string m_beforeScene = "";  
    /// <summary> フェードさせるImageのRGBa </summary>
    float m_red, m_green, m_blue, m_alfa;
    
    Coroutine m_currentFade = default;

    public static LoadSceneManager Instance { get; private set; }
    public static string CurrentScene => m_currentScene;
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

        if (m_alfa > 0)
        {
            m_isFadeIn = true;
            m_currentFade = StartCoroutine(FadeIn());
        }
    }

    /// <summary> 任意のSceneへ遷移する </summary>
    public void AnyLoadScene(string name, Action callBack = null)
    {
        m_isFadeOut = true;
        StartCoroutine(LoadScene(name, m_LoadTimer, callBack));
    }

    /// <summary>
    /// Sceneのリスタート
    /// </summary>
    public void Restart()
    {
        m_isFadeOut = true;
        StartCoroutine(LoadScene(m_currentScene, m_LoadTimer));
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
