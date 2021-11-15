using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    /// <summary> 設定中のプレイヤー名 </summary>
    string m_tempName = default;

    /// <summary>
    /// 入力された名前を確認する
    /// </summary>
    public void ChackName()
    {
        //文字数が不正だった場合
        if (m_input.text.Length < 1 || m_input.text.Length > 7)
        {
            m_caution.enabled = true;
            m_caution.text = "１文字以上7文字以内で入力してください";
            return;
        }
        else
        {
            var result = CheckBannedWord(m_input.text); //禁止ワードが含まれているか確認する
            
            //禁止ワードがあった場合
            if (result)
            {
                m_caution.enabled = true;
                m_caution.text = "その名前は使用できません";
                return;
            }
            else
            {
                m_caution.text = "";
                m_caution.enabled = false;
                m_input.text = "";
                TitleManager.Instance.TempPlayerName = m_tempName;
                TitleManager.Instance.ChangePanel(TitleStates.FinalConfirm);
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
    }

    /// <summary>
    /// 名前をセットする。InputField用
    /// </summary>
    public void SetName()
    {
        m_tempName = m_input.text;
    }

    /// <summary>
    /// 禁止ワードを判定する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    bool CheckBannedWord(string name)
    {
        var r = m_bannedWords.Select(w => w).Any(w => w == name);
        Debug.Log(r);
        return r;
    }
}
