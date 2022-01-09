using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create PlayerData")]

public class PlayerData : ScriptableObject
{
    [Header("プレイヤーの名前")]
    [SerializeField]
    string m_playerName = default;

    [Header("プレイヤーの性別")]
    [SerializeField]
    GenderType m_playerGender = default;

    [Header("ステージの開放フラグ")]
    [SerializeField]
    GameManager.ClearFlagArray[] m_clearFlag = default;

    [Header("各時代、各ステージのクイズクリア状況")]
    [SerializeField]
    StageAchieves[] m_stageAchieves = new StageAchieves[6];

    [Header("プレイヤーのクイズパートの考え中・正解・不正解のセリフ")]
    [SerializeField]
    string m_thinkingChat = default;
    [SerializeField]
    string m_correctChat = default;

    [SerializeField]
    string m_incorrectChat = default;

    [Header("男の子の画像データ")]
    [SerializeField]
    Sprite[] m_boyImages = default;

    [Header("女の子の画像データ")]
    [SerializeField]
    Sprite[] m_girlImages = default;

    public string PlayerName { get => m_playerName; set => m_playerName = value; }
    public GenderType PlayerGender { get => m_playerGender; set => m_playerGender = value; }
    public GameManager.ClearFlagArray[] ClearFlags { get => m_clearFlag; set => m_clearFlag = value; }
    public StageAchieves[] StageAchieves { get => m_stageAchieves; set => m_stageAchieves = value; }

    /// <summary> 現在のプレイヤーの性別に合わせた画像データを取得する </summary>
    public Sprite[] PlayerImage 
    {
        get
        {
            Sprite[] images = m_playerGender == GenderType.Boy ? BoyImages : GirlImages;
            return images;
        }
    }

    /// <summary> 男の子の画像データを取得する </summary>
    public Sprite[] BoyImages => m_boyImages;
    /// <summary> 女の子の画像データを取得する </summary>
    public Sprite[] GirlImages => m_girlImages;
    public string ThinkingChat => m_thinkingChat;
    public string CorrectChat => m_correctChat;
    public string IncorrectChat => m_incorrectChat;
}
