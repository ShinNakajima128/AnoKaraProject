using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenderSelect : MonoBehaviour
{
    [Header("性別選択のボタンのImage")]
    [SerializeField]
    Image[] m_genderImages = default;

    [Header("確認画面のPanel")]
    [SerializeField]
    GameObject m_confirmPanel = default;

    [Header("ガイドテキスト")]
    [SerializeField]
    Text m_genderSelectGuide = default;

    /// <summary> 選択した性別 </summary>
    GenderType m_selectGender = default;

    private void Start()
    {
        m_confirmPanel.SetActive(false);
    }

    private void OnEnable()
    {
        m_genderSelectGuide.text = "性別を選ぼう！";
    }

    /// <summary>
    /// 性別を選択する
    /// </summary>
    /// <param name="Type"> 性別の種類 </param>
    public void SelectGender(int Type)
    {
        //Boyを選択した場合、Girlを暗転して仮でBoyを変数にセットする
        if (Type == 0)
        {
            m_genderImages[1].color = new Color(0.2f, 0.2f, 0.2f);
            m_selectGender = GenderType.Boy;
            m_genderSelectGuide.text = "男の子でよろしいですか？";
        }
        //Girlを選択した場合、Boyを暗転して仮でGirlを変数にセットする
        else
        {
            m_genderImages[0].color = new Color(0.3f, 0.3f, 0.3f);
            m_selectGender = GenderType.Girl;
            m_genderSelectGuide.text = "女の子でよろしいですか？";
        }
        //確認画面を表示する
        m_confirmPanel.SetActive(true);
        SoundManager.Instance.PlaySe("SE_touch");
    }

    /// <summary>
    /// 選択した性別で決定し、次の項目を表示する
    /// </summary>
    public void Submit()
    {
        m_confirmPanel.SetActive(false);
        foreach (var i in m_genderImages)
        {
            i.color = new Color(1f, 1f, 1f);
        }
        TitleManager.Instance.TempGender = m_selectGender;
        TitleManager.Instance.ChangePanel(TitleStates.InputName);
        SoundManager.Instance.PlaySe("SE_select");
    }

    /// <summary>
    /// 選択をやり直す
    /// </summary>
    public void Cancel()
    {
        m_confirmPanel.SetActive(false);
        foreach (var i in m_genderImages)
        {
            i.color = new Color(1f, 1f, 1f);
        }
        m_genderSelectGuide.text = "性別を選ぼう！";
    }
}
