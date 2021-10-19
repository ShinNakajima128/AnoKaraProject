using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 会話マネージャー
/// </summary>
public class DialogManager : MonoBehaviour
{
    /// <summary>会話システムのオブジェクト</summary>
    [SerializeField]
    GameObject m_dialogDisplay;

    /// <summary>会話データ</summary>
    [SerializeField]
    DialogDataBase[] m_dialogData;

    /// <summary>背景のイメージ</summary>
    [SerializeField]
    Image m_backGroundImage;

    /// <summary>キャラクターポジションのイメージ</summary>
    [SerializeField]
    Image[] m_charcterPositionImage;

    /// <summary>名前のテキスト</summary>
    [SerializeField]
    Text m_dialogName;

    /// <summary>会話のテキスト</summary>
    [SerializeField]
    Text m_dialogText;

    /// <summary>テキストの再生中かどうか</summary>
    bool m_isSendingText = false;

    /// <summary>ダイアログの再生番号</summary>
    int m_dialogCount = 0;

    /// <summary>ダイアログの開始インデックス</summary>
    int m_dialogStartIndex = 0;

    /// <summary>ダイアログの終了のインデックス</summary>
    int m_dialogEndIndex = 0;

    private void Start()
    {
        StartCoroutine(SendingDialogText(m_dialogData[m_dialogCount],
                m_dialogData[m_dialogCount].dialogSendingSpeed));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendDialogText();
        }

        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began)    //タップした瞬間
        //    {
        //        SendDialogText();
        //    }
        //}
    }

    /// <summary>
    /// テキストの再生
    /// </summary>
    /// <param name="data">テキストデータ</param>
    /// <param name="speed">再生スピード</param>
    IEnumerator SendingDialogText(DialogDataBase data, float speed)
    {
        //再生中の文字カウント
        int sendingTextCount = 0;
        //タイマー
        float timer = 999;

        m_isSendingText = true;
        RefreshText(m_dialogName, m_dialogText);
        SetDialogSprite(data);
        m_dialogName.text = data.dialogName;
        
        while (sendingTextCount < data.dialogText.Length)
        {
            timer += Time.deltaTime;

            if (!m_isSendingText)   //スキップ時の処理
            {
                m_dialogText.text = data.dialogText;
                break;
            }

            if (timer > speed)      //通常の文字送りの処理
            {
                m_dialogText.text += data.dialogText[sendingTextCount];
                timer = 0;
                sendingTextCount++;
            }
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    /// <summary>
    /// 会話を進める
    /// </summary>
    void SendDialogText()
    {
        if (m_isSendingText)
        {
            //スキップする
            m_isSendingText = false;
        }
        else
        {
            m_dialogCount++;
            //次の会話データを読み込む。次がなければUIを落とす
            if (m_dialogCount < m_dialogData.Length)
            {
                StartCoroutine(SendingDialogText(m_dialogData[m_dialogCount],
                    m_dialogData[m_dialogCount].dialogSendingSpeed));
            }
            else
            {
                m_dialogDisplay.SetActive(false);
            }
        }
    }

    /// <summary>
    ///　背景、キャラクターの画像をセットする
    /// </summary>
    /// <param name="data">会話データ</param>
    void SetDialogSprite(DialogDataBase data)
    {
        m_backGroundImage.sprite = data.backGroundImage;   
        m_charcterPositionImage[0].sprite = data.charctorLeftImage;
        m_charcterPositionImage[1].sprite = data.charctorCenterImage;
        m_charcterPositionImage[2].sprite = data.charctorRightImage;
    }

    /// <summary>
    /// 各テキストをリセットする
    /// </summary>
    /// <param name="name">名前のテキスト</param>
    /// <param name="dialog">会話のテキスト</param>
    void RefreshText(Text name, Text dialog)
    {
        name.text = "";
        dialog.text = "";
    }

    /// <summary>
    /// 会話を開始する
    /// </summary>
    /// <param name="data">ダイアログデータ</param>
    /// <param name="startIndex">開始する番号</param>
    /// <param name="endIndex">終了する番号</param>
    void StartDialog(DialogDataBase data, int startIndex, int endIndex)
    {
        m_dialogDisplay.SetActive(true);
        m_dialogCount = startIndex;
    }

    /// <summary>
    /// 会話を終了する
    /// </summary>
    void EndDialog()
    {
        m_dialogDisplay.SetActive(false);
    }
}
