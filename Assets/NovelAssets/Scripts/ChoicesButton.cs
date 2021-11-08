using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesButton : MonoBehaviour
{
    Button m_b = default;
    public int NextMessageId { get; set; }
    public int AfterReactionMessageId { get; set; }

    void Start()
    {
        m_b = GetComponent<Button>();
        m_b.onClick.AddListener(() =>
        {
            Choice(NextMessageId);
        });
    }
    public void Choice(int nextId)
    {
        ScenarioManager.Instance.SwitchIndex(nextId);
        ScenarioManager.Instance.AfterReactionMessageId = AfterReactionMessageId;
    }
}
