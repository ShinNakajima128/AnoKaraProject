using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForbiddenWordDetector : MonoBehaviour
{
    /// <summary></summary>

    /// <summary>入力用のフィールド</summary>
    [SerializeField]
    InputField inputField;

    /// <summary>入力されたテキストを保存するメンバ―変数</summary>
    [SerializeField]
    Text m_text;

    private string[] NGwords = { "AAAA", "BBBB", "CCCC" };

    private bool _NG;
    void Start()
    {
        inputField = inputField.GetComponent<InputField>();
        m_text = m_text.GetComponent<Text>();
    }
    /// <summary>NGワードを検索する</summary>
    public void SearchNGwords()
    {
        _NG = false;

        for (int i = 0; i < NGwords.Length; i++)
        {
            if (NGwords[i] == inputField.text)
            {
                Debug.Log("禁止ワードです。");
                _NG = false;
                inputField.text = "";
            }
            else if (inputField == null)
            {
                _NG = true;
            }
        }
    }

    /// <summary>入力されたテキストを出力する</summary>
    public void InputText()
    {
        if (_NG)
        {
            Debug.Log(inputField.text);
            inputField.text = "";
        }
        else
        {
            m_text.text = inputField.text;
        }
    }
}