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
    DialogData[] m_dialogData;

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
    }

    /// <summary>
    /// テキストの再生
    /// </summary>
    /// <param name="data">テキストデータ</param>
    /// <param name="speed">再生スピード</param>
    IEnumerator SendingDialogText(DialogData data, float speed)
    {
        RefreshText(m_dialogName, m_dialogText);
        m_isSendingText = true;
        SetCharcterSprite(data);
        m_dialogName.text = data.dialogName;
        int i = 0;
        float timer = 999;

        while (i < data.dialogText.Length)
        {
            timer += Time.deltaTime;

            if (!m_isSendingText)   //スキップ時の処理
            {
                m_dialogText.text = data.dialogText;
                break;
            }

            if (timer > speed)      //通常の文字送りの処理
            {
                m_dialogText.text += data.dialogText[i];
                timer = 0;
                i++;
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
            //次の会話データを読み込む
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
    /// キャラクターを各ポジションに配置する
    /// </summary>
    /// <param name="data">会話データ</param>
    void SetCharcterSprite(DialogData data)
    {
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
}
