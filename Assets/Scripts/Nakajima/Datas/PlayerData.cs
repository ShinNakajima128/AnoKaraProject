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

    [Header("ゲームの進行度")]
    [SerializeField]
    int m_progress = default;

    [Header("男の子の画像データ")]
    [SerializeField]
    Sprite[] m_boyImages = default;

    [Header("女の子の画像データ")]
    [SerializeField]
    Sprite[] m_girlImages = default;

    public string PlayerName { get => m_playerName; set => m_playerName = value; }
    public GenderType PlayerGender { get => m_playerGender; set => m_playerGender = value; }
    public int Progress { get => m_progress; set => m_progress = value; }

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
}
