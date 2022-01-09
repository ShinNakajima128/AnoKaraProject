using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージ選択画面のアチーブの表示機能を持つクラス
/// </summary>
public class StageAchieveControl : MonoBehaviour
{
    [Header("アチーブを表示するImages")]
    [SerializeField]
    Image[] m_achieveImages = default;

    [Header("達成した画像データ")]
    [SerializeField]
    Sprite m_achievedSprite = default;

    [Header("未達成用の画像データ")]
    [SerializeField]
    Sprite m_unachievedSprite = default;

    /// <summary>
    /// アチーブステータスを表示する
    /// </summary>
    /// <param name="achieve"> アチーブのステータス </param>
    public void ViewAchieve(StageQuizAchieveStates achieve)
    {
        switch (achieve)
        {
            case StageQuizAchieveStates.None:
                UpdateImages(0);
                break;
            case StageQuizAchieveStates.One:
                UpdateImages(1);
                break;
            case StageQuizAchieveStates.Two:
                UpdateImages(2);
                break;
            case StageQuizAchieveStates.Three:
                UpdateImages(3);
                break;
            default:
                Debug.LogError("値が不正です");
                break;
        }
    }

    /// <summary>
    /// Imageデータを更新する
    /// </summary>
    /// <param name="updateNum"></param>
    void UpdateImages(int updateNum)
    {
        for (int i = 0; i < m_achieveImages.Length; i++)
        {
            if (i < updateNum)
            {
                m_achieveImages[i].sprite = m_achievedSprite;
            }
            else
            {
                m_achieveImages[i].sprite = m_unachievedSprite;
            }
        }
    }
}
