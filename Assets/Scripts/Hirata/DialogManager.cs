﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 会話マネージャー
/// </summary>
public class DialogManager : MonoBehaviour
{
    /// <summary>デバッグモード</summary>
    [SerializeField]
    bool m_isDebug = false;

    /// <summary>オート再生か否か</summary>
    [SerializeField]
    bool m_isAuto = false;

    /// <summary>オートの時、次の会話ログ再生までの時間</summary>
    [SerializeField]
    float m_autoSendingTime = 2f;

    /// <summary>
    /// 各再生スピード
    /// </summary>
    public enum SendingSpeed
    {
        /// <summary>遅い</summary>
        Low,
        /// <summary>普通</summary>
        Middle,
        /// <summary>早い</summary>
        High,
    }

    /// <summary>再生スピードのモード</summary>
    public SendingSpeed m_sendingSpeed = SendingSpeed.Middle;

    /// <summary>遅い時の再生スピード</summary>
    [SerializeField]
    float m_sendingLowSpeed = 1f;

    /// <summary>普通の時の再生スピード</summary>
    [SerializeField]
    float m_sendingMiddleSpeed = 0.5f;

    /// <summary>早い時の再生スピード</summary>
    [SerializeField]
    float m_sendingHighSpeed = 0.01f;

    /// <summary>会話システムのオブジェクト</summary>
    [SerializeField]
    GameObject m_dialogDisplay;

    /// <summary>会話データ</summary>
    [SerializeField]
    DialogData m_dialogData;

    /// <summary>各会話データの保存</summary>
    DialogDataBase m_dialogDataBase;

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

    /// <summary>会話ログのオブジェクト</summary>
    [SerializeField]
    GameObject m_dialogLogObject;

    /// <summary>会話ログのテキスト</summary>
    [SerializeField]
    Text m_dialogLogText;

    /// <summary>テキストの再生中かどうか</summary>
    bool m_isSendingText = false;

    /// <summary>会話ログがアクティブかどうか</summary>
    bool m_isActiveDiaLog = false;

    /// <summary>ダイアログの再生番号</summary>
    int m_dialogCount = 0;

    /// <summary>テキスト再生コルーチンの保存先</summary>
    IEnumerator m_saveSendingDialogText;

    /// <summary>オートコルーチンの保存先</summary>
    IEnumerator m_saveAutoSending;

    /// <summary>オート再生時のタイマーフラグ</summary>
    bool m_isAutoTimer = true;

    private void Start()
    {
        if (m_isDebug)
        {
            StartSendingDialogText();
        }
    }

    private void Update()
    {
        if (m_isActiveDiaLog)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendDialogText();
            }
        }

        //スマホ用(タップ)
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
    IEnumerator SendingDialogText(DialogDataBase data)
    {
        //再生中の文字カウント
        int sendingTextCount = 0;
        //タイマー
        float timer = 999;

        m_isSendingText = true;
        RefreshText(m_dialogName, m_dialogText);
        SetDialogSprite(data);
        m_dialogName.text = data.dialogName;
        AddDialogLog(data.dialogName, data.dialogText);
        //m_dialogLogText.text += data.dialogName + "\n";   ログも順次出力する時
        
        while (sendingTextCount < data.dialogText.Length)
        {
            timer += Time.deltaTime;

            if (!m_isSendingText)   //スキップ時の処理
            {
                m_dialogText.text = data.dialogText;
                //m_dialogLogText.text += data.dialogText;  ログも順次出力する時
                break;
            }

            ///通常文字送り
            if (m_sendingSpeed == SendingSpeed.Low)
            {
                if (timer > m_sendingLowSpeed)
                {
                    SendingText();
                }
            }
            else if (m_sendingSpeed == SendingSpeed.Middle)
            {
                if (timer > m_sendingMiddleSpeed)
                {
                    SendingText();
                }
            }
            else if (m_sendingSpeed == SendingSpeed.High)
            {
                if (timer > m_sendingHighSpeed)
                {
                    SendingText();
                }
            }
            yield return new WaitForEndOfFrame();
        }

        if (m_isAuto)
        {
            m_dialogCount++;
            if (m_dialogCount < m_dialogData.m_dialogData.Count)
            {
                StartCoroutine(AutoSending());
            }
        }
        yield break;

        ///会話データを出力する
        void SendingText()
        {
            m_dialogText.text += data.dialogText[sendingTextCount];
            //m_dialogLogText.text += data.dialogText[sendingTextCount]; ログも順次出力する時
            timer = 0;
            sendingTextCount++;
        }
    }

    /// <summary>
    /// 会話を進める
    /// </summary>
    void SendDialogText()
    {
        if (!m_isAuto)
        {
            if (m_isSendingText)
            {
                //スキップする
                m_isSendingText = false;
            }
            else
            {
                m_dialogCount++;
                //次の会話データを読み込む。次がなければ止まる
                if (m_dialogCount < m_dialogData.m_dialogData.Count)
                {
                    StartSendingDialogText();
                }
            }
        }
    }

    /// <summary>
    /// オートモード
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoSending()
    {
        float timer = 0;

        if (m_dialogCount >= m_dialogData.m_dialogData.Count)
        {
            yield break;
        }

        while (m_isAutoTimer)
        {
            timer += Time.deltaTime;

            if (timer > m_autoSendingTime)
            {
                if (m_dialogCount < m_dialogData.m_dialogData.Count)
                {
                    StartSendingDialogText();
                }
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }

    /// <summary>
    /// 会話ログにデータを追加する
    /// </summary>
    /// <param name="name">名前データ</param>
    /// <param name="text">会話データ</param>
    void AddDialogLog(string name, string text)
    {
        m_dialogLogText.text += name;
        m_dialogLogText.text += "\n";
        m_dialogLogText.text += text;
        
        for (int i = 0; i < 3; i++)
        {
            m_dialogLogText.text += "\n";
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
    /// 次の再生スピードを変える
    /// </summary>
    public void SetSendingSpeed()
    {
        switch (m_sendingSpeed)
        {
            case SendingSpeed.Low:
                {
                    m_sendingSpeed = SendingSpeed.Middle;
                }
                break;
            case SendingSpeed.Middle:
                {
                    m_sendingSpeed = SendingSpeed.High;
                }
                break;
            case SendingSpeed.High:
                {
                    m_sendingSpeed = SendingSpeed.Low;
                }
                break;
        }
    }

    /// <summary>
    /// 会話を再生する
    /// </summary>
    void StartSendingDialogText()
    {
        m_dialogDataBase = m_dialogData.m_dialogData[m_dialogCount];
        m_saveSendingDialogText = SendingDialogText(m_dialogDataBase);
        m_saveAutoSending = AutoSending();
        StartCoroutine(m_saveSendingDialogText);
    }

    /// <summary>
    /// 会話ログを起動する
    /// </summary>
    public void ActiveLog()
    {
        if (!m_isActiveDiaLog)
        {
            m_isSendingText = false;
            m_isActiveDiaLog = true;
            m_dialogLogObject.SetActive(true);
            StopCoroutine(m_saveSendingDialogText);
            if (m_isAuto)
            {
                m_isAutoTimer = false;
                StopCoroutine(m_saveAutoSending);
            }
        }
        else
        {
            m_isSendingText = true;
            m_isActiveDiaLog = false;
            m_dialogLogObject.SetActive(false);
            StartCoroutine(m_saveSendingDialogText);
            if (m_isAuto)
            {
                m_isAutoTimer = true;
                StartCoroutine(m_saveAutoSending);
            }
        }
    }

    /// <summary>
    /// 会話を開始する
    /// </summary>
    /// <param name="data">ダイアログデータ</param>
    public void StartDialog(DialogData data)
    {
        m_dialogDisplay.SetActive(true);
    }

    /// <summary>
    /// 会話を終了する
    /// </summary>
    void EndDialog()
    {
        m_dialogDisplay.SetActive(false);
    }
}
