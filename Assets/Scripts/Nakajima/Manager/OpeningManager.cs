using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningManager : MonoBehaviour
{
    [Header("時代選択画面のScene名")]
    [SerializeField]
    string m_periodSelectSceneName = default;



    private void Start()
    {
        EventManager.ListenEvents(Events.FinishDialog, LoadPeriodSelectScene);
        SoundManager.Instance.PlayBgm(SoundManager.Instance.BgmName);
    }
    /// <summary>
    /// 時代選択画面に遷移する
    /// </summary>
    void LoadPeriodSelectScene()
    {
        LoadSceneManager.AnyLoadScene(m_periodSelectSceneName);
    }
}
