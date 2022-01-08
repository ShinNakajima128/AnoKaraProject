using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataObject : MonoBehaviour, ISave
{
    /// <summary> ゲームデータ </summary>
    [SerializeField]
    SaveData.GameData gameData;

    /// <summary> プレイヤーのデータを保管するScriptableObject </summary>
    [SerializeField]
    PlayerData m_playerData = default;

    /// <summary> ゲームデータを取得する </summary>
    public SaveData.GameData GameData => gameData;

    /// <summary> 初プレイかどうかのフラグを取得する </summary>
    public bool FirstPlay { get => gameData.FirstPlay; set => gameData.FirstPlay = value; } 

    /// <summary> プレイヤー名を取得する </summary>
    public string PlayerName { get => gameData.PlayerName; set => gameData.PlayerName = value; }
    /// <summary> 性別を取得する </summary>
    public GenderType PlayerGender { get => gameData.Gender; set => gameData.Gender = value; }
    /// <summary> クリアフラグを取得する </summary>
    public GameManager.ClearFlagArray[] ClearFlag { get => gameData.ClearFlags; set => gameData.ClearFlags = value; }

    /// <summary>
    /// セーブする
    /// </summary>
    /// <param name="data"> セーブ先のデータ </param>
    public void Save(SaveData.GameData data)
    {
        data.PlayerName = gameData.PlayerName;
        data.Gender = gameData.Gender;
        data.ClearFlags = gameData.ClearFlags;
    }

    /// <summary>
    /// ロードする
    /// </summary>
    /// <param name="data"> ロード先のデータ </param>
    public void Load(SaveData.GameData data)
    {
        gameData.PlayerName = data.PlayerName;
        gameData.Gender = data.Gender;
        gameData.ClearFlags = data.ClearFlags;
    }

    /// <summary>
    /// 現在のゲームデータをプレイヤーのデータを持つScriptableObjectにセットする
    /// </summary>
    public void SetUp()
    {
        var str = PlayerPrefs.GetString("GameData");
        if (str == null)
        {
            return;
        }
        else
        {
            var g = JsonUtility.FromJson<SaveData>(str);

            if (g == null)
            {
                return;
            }
            else if (!g.CurrentGameData.FirstPlay)
            {
                gameData = SaveManager.GetData().CurrentGameData;
            }
        }    
    }

    public void UpdatePlayerData()
    {
        m_playerData.PlayerName = gameData.PlayerName;
        m_playerData.PlayerGender = gameData.Gender;
        m_playerData.ClearFlags = gameData.ClearFlags;
    }
}
