using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 探索パートの各タスクボタンの機能を持つクラス
/// </summary>
public class EventButton : MonoBehaviour
{
    /// <summary> タスク完了時に差し替える画像 </summary>
    [SerializeField]
    Sprite m_complitedImage = default;

    /// <summary> アニメーションの時間 </summary>
    [SerializeField]
    float m_animTimer = 0.25f;
    /// <summary> 差し替え先のImage </summary>
    [SerializeField]
    Image m_buttonImage = default;
    /// <summary> 操作するボタン </summary>
    Button m_eventButton = default;

    /// <summary> 次に表示するタスクオブジェクト </summary>
    [SerializeField]
    GameObject[] m_nextTaskObjects = default; 
    
    /// <summary> 操作するボタンのRectTransform </summary>
    RectTransform m_rt = default;

    void Start()
    {
        m_eventButton = GetComponent<Button>();
        m_rt = m_buttonImage.GetComponent<RectTransform>();

        //ボタンが押された時の処理を追加
        m_eventButton.onClick.AddListener(RegisteringFunction);
    }

    /// <summary>
    /// タスク開始時にeventManagerへタスク終了時の関数を登録する。ボタン用
    /// </summary>
    void RegisteringFunction()
    {
        //EventManagerのタスク終了時のイベント通知に登録
        EventManager.ListenEvents(Events.TaskComplite, FinishTask);
        EventManager.OnEvent(Events.BeginTask);
    }

    /// <summary>
    /// タスクが終了したら、ボタンをOFFにして現在のタスクの処理をEventManagerから削除する
    /// </summary>
    void FinishTask()
    {
        m_eventButton.interactable = false; //ボタンを押せなくする
        m_rt.sizeDelta = new Vector2(100, 100); //ボタンのWidth,Heightを調整
        m_rt.localScale = new Vector3(100, 100, 100); //アニメーション用にImageのサイズを変更
        m_buttonImage.sprite = m_complitedImage; //画像差し替え

        //指定した時間で元のサイズに戻すアニメーションを開始
        m_rt.DOScale(new Vector3(1, 1, 1), m_animTimer)
            .OnComplete(() => 
            { 
                //SoundManager.Instance.PlaySe("SE_touch");
                SearchManager.Instance.IsTaskComplited = false;
                if (m_nextTaskObjects != null)
                {
                    foreach (var nt in m_nextTaskObjects)
                    {
                        nt.SetActive(true);
                    }
                    SoundManager.Instance.PlaySe("SE_quiz");
                }
            });
        
        //EventManagerから関数を削除
        EventManager.RemoveEvents(Events.TaskComplite, FinishTask);
    }
}
