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
    Text m_answerCountText;

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
    /// 正解数
    /// クイズマネージャーが正解数を持っているので参照する
    /// </summary>
    [SerializeField]
    int m_ansCount = 7;

    /// <summary>リザルトのイラストデータ</summary>
    [SerializeField]
    Sprite[] m_resultSprite;

    void Start()
    {
        QuizResult(m_ansCount);
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
        else if (count >= 7)
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
        m_answerCountText.text = m_ansCount.ToString();
        m_resultText.text = "失敗…";
        m_resultComment.fontSize = 60;
        m_resultComment.text = "もう一度挑戦してみよう";
        m_charactorImage.sprite = m_resultSprite[0];
    }

    /// <summary>
    /// クイズに成功した時の処理
    /// </summary>
    void QuizSuccess()
    {
        m_answerCountText.text = m_ansCount.ToString();
        m_resultText.text = "成功!!";
        m_resultComment.fontSize = 55;
        m_resultComment.text = "間違えた問題を確認しよう!";
        m_charactorImage.sprite = m_resultSprite[1];
    }

    /// <summary>
    /// クイズに完全成功した時の処理
    /// </summary>
    void QuizComplete()
    {
        m_answerCountText.text = m_ansCount.ToString();
        m_resultText.text = "大成功!!";
        m_resultComment.text = "全問正解！すごい！";
        m_charactorImage.sprite = m_resultSprite[2];
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