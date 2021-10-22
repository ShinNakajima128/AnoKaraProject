﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 会話データのScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/Create DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogDataBase> m_dialogData = new List<DialogDataBase>();
}

[System.Serializable]
public class DialogDataBase
{
    /// <summary>背景の画像</summary>
    [SerializeField]
    Sprite m_backGroundImage;

    /// <summary>背景の有無</summary>
    [SerializeField]
    bool m_isBackGround = false;

    /// <summary>左のキャラクターの画像</summary>
    [SerializeField]
    Sprite m_charcterLeftImage;

    /// <summary>中央のキャラクターの画像</summary>
    [SerializeField]
    Sprite m_charcterCenterImage;

    /// <summary>右キャラクターの画像</summary>
    [SerializeField]
    Sprite m_charcterRightImage;

    /// <summary>名前</summary>
    [SerializeField]
    string m_dialogName;

    /// <summary>会話テキスト</summary>
    [SerializeField, TextArea(0, 10)]
    string m_dialogText;

    public Sprite backGroundImage => m_backGroundImage;

    public bool isBackGroundImage => m_isBackGround;

    public Sprite charctorRightImage => m_charcterRightImage;

    public Sprite charctorCenterImage => m_charcterCenterImage;

    public Sprite charctorLeftImage => m_charcterLeftImage;

    public string dialogName => m_dialogName;

    public string dialogText => m_dialogText;
}