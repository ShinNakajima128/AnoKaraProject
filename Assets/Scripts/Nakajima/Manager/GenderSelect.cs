using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenderSelect : MonoBehaviour
{
    [SerializeField]
    Image[] m_genderImages = default;

    [SerializeField]
    GameObject m_confirmPanel = default;

    GenderType m_selectGender = default;

    private void Start()
    {
        m_confirmPanel.SetActive(false);
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
        }
        //Girlを選択した場合、Boyを暗転して仮でGirlを変数にセットする
        else
        {
            m_genderImages[0].color = new Color(0.3f, 0.3f, 0.3f);
            m_selectGender = GenderType.Girl;
        }
        //確認画面を表示する
        m_confirmPanel.SetActive(true);
    }

    public void Submit()
    {
        m_confirmPanel.SetActive(false);
        TitleManager.Instance.TempGender = m_selectGender;
        TitleManager.Instance.ChangePanel(TitleStates.InputName);
    }

    public void Cancel()
    {
        m_confirmPanel.SetActive(false);
        foreach (var i in m_genderImages)
        {
            i.color = new Color(1f, 1f, 1f);
        }
    }
}
