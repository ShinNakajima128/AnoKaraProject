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
    Sprite[] m_resultSprite = new Sprite[3];

    /// <summary>成功失敗のテキストデータ</summary>
    [SerializeField]
    string[] m_resultTextData = new string[3];

    /// <summary>コメントデータ</summary>
    [SerializeField]
    string[] m_resultCommentData = new string[3];

    /// <summary>
    /// コメントデータのフォントサイズ
    /// 変更がなければ　0　のままにしておく
    /// </summary>
    [SerializeField]
    int[] m_resultCommentFontSize = new int[3];

    void Start()
    {
        QuizResult(m_ansCount);
    }

    /// <summary>
    /// 正解数に応じて、表示するUIを変える
    /// </summary>
    /// <param name="count">正解数</param>
    public void QuizResult(int count)
    {
        if (count == 10)
        {
            QuizResultUiSet(0);
        }
        else if (count >= 7)
        {
            QuizResultUiSet(1);
        }
        else if (count < 7)
        {
            QuizResultUiSet(2);
        }
    }

    /// <summary>
    /// 正解数に応じて結果をUIに表示する
    /// </summary>
    /// <param name="index">成功レベル</param>
    void QuizResultUiSet(int index)
    {
        m_answerCountText.text = m_ansCount.ToString();
        m_resultText.text = m_resultTextData[index];
        m_resultComment.text = m_resultCommentData[index];
        if (m_resultCommentFontSize[index] != 0)
        {
            m_resultComment.fontSize = m_resultCommentFontSize[index];
        }
        m_charactorImage.sprite = m_resultSprite[index];
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
        Debug.Log("リトライ");
    }

    /// <summary>
    /// 問題解説
    /// 問題解説ボタンに設定する
    /// </summary>
    public void QuizReview()
    {
        Debug.Log("問題解説");
    }
}