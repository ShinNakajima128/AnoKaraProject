using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NameInput : MonoBehaviour
{
    [SerializeField]
    InputField m_input = default;

    [SerializeField]
    Text m_caution = default;

    [SerializeField]
    string[] m_bannedWords = default;

    string m_tempName = default;

    public void ChackName()
    {
        if (m_input.text.Length < 1 || m_input.text.Length > 7)
        {
            m_caution.enabled = true;
            m_caution.text = "１文字以上7文字以内で入力してください";
            return;
        }
        else
        {
            var result = CheckBannedWord(m_input.text);
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

    public void Cancel()
    {
        m_input.text = "";
        TitleManager.Instance.ChangePanel(TitleStates.SelectGender);
    }

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
