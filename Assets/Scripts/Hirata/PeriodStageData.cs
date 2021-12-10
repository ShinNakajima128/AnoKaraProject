using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptable/Create StageData")]
public class PeriodStageData : ScriptableObject
{
    public List<PeriodStageDataBase> m_dataBases = new List<PeriodStageDataBase>();
}

[System.Serializable]
public class PeriodStageDataBase
{
    /// <summary>
    /// 表示される画像
    /// </summary>
    [SerializeField]
    Sprite[] m_sprites;

    /// <summary>
    /// ステージボタンに設定されるテキスト
    /// </summary>
    [SerializeField]
    string[] m_stageText;

    public Sprite[] StageSprite { get { return m_sprites; } }
    public string[] StageText { get { return m_stageText; } }
}
