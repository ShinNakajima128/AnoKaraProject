using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>クイズのリザルトマネージャー</summary>
public class QuizResultManager : MonoBehaviour
{
    /// <summary>成功失敗を表示するテキスト</summary>
    [SerializeField]
    Text m_resultText;

    /// <summary>正解数を表示するテキスト</summary>
    [SerializeField]
    Text m_ansCount;

    /// <summary>キャラクターのイメージ</summary>
    [SerializeField]
    Image m_charactorImage;

    /// <summary>コメントを表示するテキスト</summary>
    [SerializeField]
    Text m_resultComment;

    /// <summary>次へのUIを表示するボタン</summary>
    [SerializeField]
    Button m_nextUiButton;

    /// <summary>次へのUI群</summary>
    [SerializeField]
    GameObject m_nextUi;

    /// <summary>
    /// クイズマネージャーが正解数を持っています
    /// </summary>

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// クイズのリザルト
    /// </summary>
    /// <param name="count">正解数</param>
    public void QuizResult(int count)
    {
        if (count == 10)
        {
            QuizComplete();
        }
        else if (count <= 7)
        {
            QuizSuccess();
        }
        else if (count < 7)
        {
            QuizFailed();
        }
    }

    /// <summary>
    /// クイズに失敗した時の処理
    /// </summary>
    void QuizFailed()
    {

    }

    /// <summary>
    /// クイズに成功した時の処理
    /// </summary>
    void QuizSuccess()
    {

    }

    /// <summary>
    /// クイズに完全成功した時の処理
    /// </summary>
    void QuizComplete()
    {

    }

    /// <summary>
    /// 次へのUIを表示する
    /// ボタンに設定する
    /// </summary>
    public void NextUiOn()
    {
        m_nextUiButton.interactable = false;
        m_nextUi.SetActive(true);
    }

    /// <summary>
    /// 次のシーンへ
    /// ボタンに設定する
    /// </summary>
    public void NextScene(string scene)
    {
        LoadSceneManager.AnyLoadScene(scene, () =>
        {
            LoadSceneManager.FadeOutPanel();
        });
    }

    /// <summary>
    /// リトライ
    /// リトライボタンに設定する
    /// </summary>
    public void QuizRetry()
    {

    }

    /// <summary>
    /// 問題解説
    /// 問題解説ボタンに設定する
    /// </summary>
    public void QuizReview()
    {

    }
}