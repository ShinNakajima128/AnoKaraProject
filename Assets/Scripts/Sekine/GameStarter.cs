using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarter : MonoBehaviour
{
    /// <summary>入力用のインプットフィールドオブジェクト</summary>
    [SerializeField]
    InputField _inputField;

    /// <summary>画面全体を覆うボタンオブジェクト</summary>
    [SerializeField]
    Button _button;

    /// <summary>初期表示テキストオブジェクト</summary>
    [SerializeField]
    Text _text;

    /// <summary>タイトルテキストオブジェクト</summary>
    [SerializeField]
    Text _titleText;


    void Start()
    {
        _inputField.gameObject.SetActive(false);

        _button.gameObject.SetActive(true);
    }

    /// <summary>ボタンを消す</summary>
    public void eraceButton()
    {
        _button.gameObject.SetActive(false);
    }

    /// <summary>ボタンを表示する</summary>
    public void ActiveButton()
    {
        _button.gameObject.SetActive(true);
    }

    /// <summary>入力用フィールドを表示する</summary>
    public void activeField()
    {
        _inputField.gameObject.SetActive(true);
    }

    /// <summary>入力用フィールドを消す</summary>
    public void eraceField()
    {
        _inputField.gameObject.SetActive(false);
    }

    /// <summary>テキストオブジェクトを消す</summary>
    public void eraveText()
    {
        _text.gameObject.SetActive(false);
    }

    /// <summary>テキストオブジェクトを表示する</summary>
    public void activeText()
    {
        _text.gameObject.SetActive(true);
    }

    ///<summary>タイトルを消す</summary>
    public void eraceTitle()
    {
        _titleText.gameObject.SetActive(false);
    }

    ///<summary>タイトルを表示する</summary>
    public void activeTitle()
    {
        _titleText.gameObject.SetActive(true);
    }
}
