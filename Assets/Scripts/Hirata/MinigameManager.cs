using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ミニゲーム管理クラス</summary>
public class MinigameManager : MonoBehaviour
{
    /// <summary>
    /// ミニゲームの種類
    /// </summary>
    enum Minigame
    {
        /// <summary>ブロック崩し</summary>
        Block,
        /// <summary>選択型</summary>
        Choice,
        /// <summary>回転型</summary>
        Spin,
    }

    /// <summary>ミニゲーム</summary>
    Minigame m_minigame;

    /// <summary>
    /// ミニゲームを設定し、該当シーンに移動する
    /// ボタンに設定する
    /// </summary>
    /// <param name="minigame">呼びたいミニゲーム</param>
    public void MinigameButton(string minigame)
    {
        if (minigame == "Block")
        {
            m_minigame = Minigame.Block;
        }
        else if (minigame == "Choice")
        {
            m_minigame = Minigame.Choice;
        }
        else if (minigame == "Spin")
        {
            m_minigame = Minigame.Spin;
        }

        LoadSceneManager.AnyLoadScene(m_minigame.ToString(), () =>
        {
            LoadSceneManager.FadeOutPanel();
        });
    }
}
