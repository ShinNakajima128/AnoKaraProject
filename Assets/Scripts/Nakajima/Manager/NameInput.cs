using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class NameInput : MonoBehaviour
{
    [Header("名前入力用のInputField")]
    [SerializeField]
    InputField m_input = default;

    [Header("入力が有効では無かった時の警告文")]
    [SerializeField]
    Text m_caution = default;

    [Header("禁止ワード")]
    [SerializeField]
    string[] m_bannedWords = default;

    [SerializeField]
    Text m_nameText = default;

    /// <summary> 設定中のプレイヤー名 </summary>
    string m_tempName = default;

    private void OnEnable()
    {
        if (DataManager.Instance.PlayerData.PlayerName != null && DataManager.Instance.PlayerData.PlayerName != "")
        {
            m_input.text = DataManager.Instance.PlayerData.PlayerName;
            m_tempName = m_input.text;
        }
    }

    /// <summary>
    /// 入力された名前を確認する
    /// </summary>
    public void ChackName()
    {
        var s = m_input.text.Trim();

        //文字数が不正だった場合
        if (s.Length < 1 || s.Length > 7)
        {
            m_caution.enabled = true;
            m_input.text = "";
            m_caution.text = "１文字以上7文字以内で入力してください";
            return;
        }
        else
        {
            var result = CheckBannedWord(s); //禁止ワードが含まれているか確認する
            
            //禁止ワードがあった場合
            if (result)
            {
                m_caution.enabled = true;
                m_input.text = "";
                m_caution.text = "その名前は使用できません";
                SoundManager.Instance.PlaySe("SE_incorrect");
                return;
            }
            else
            {
                m_caution.text = "";
                m_caution.enabled = false;
                m_input.text = "";
                if (SceneManager.GetActiveScene().name == "Title")
                {
                    TitleManager.Instance.TempPlayerName = m_tempName;
                    TitleManager.Instance.ChangePanel(TitleStates.FinalConfirm);
                }
                else
                {
                    DataManager.Instance.PlayerData.PlayerName = m_tempName;
                    DataManager.UpdateData();
                    m_nameText.text = ($"名前が{m_tempName}に変更されました");
                }
                SoundManager.Instance.PlaySe("SE_touch");
                
            }
        }
    }

    /// <summary>
    /// 入力を中断し、性別選択画面に戻る
    /// </summary>
    public void Cancel()
    {
        m_input.text = "";
        TitleManager.Instance.ChangePanel(TitleStates.SelectGender);
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// 名前をセットする。InputField用
    /// </summary>
    public void SetName()
    {
        m_tempName = m_input.text;
    }

    public void ReName()
    {

    }

    /// <summary>
    /// 禁止ワードを判定する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool CheckBannedWord(string name)
    {
        var r = m_bannedWords.Select(w => w).Any(w => w == name);
        return r;
    }
}
