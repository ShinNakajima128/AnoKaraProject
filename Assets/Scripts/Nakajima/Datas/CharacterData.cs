using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;

[CreateAssetMenu(menuName = "MyScriptable/Create CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("キャラクター名")]
    [SerializeField]
    string m_characterName = default;

    [SerializeField]
    PeriodTypes m_characterPeriod = default;

    [SerializeField]
    Sprite[] m_characterImages = default;

    [Header("各偉人のクイズパートの考え中・正解・不正解のセリフ")]
    [SerializeField]
    string m_thinkingChat = default;
    [SerializeField]
    string m_correctChat = default;

    [SerializeField]
    string m_incorrectChat = default;

    public string CharacterName => m_characterName;
    public PeriodTypes CharacterPeriod => m_characterPeriod;
    public Sprite[] CharacterImages => m_characterImages;
    public string ThinkingChat => m_thinkingChat;
    public string CorrectChat => m_correctChat;
    public string IncorrectChat => m_incorrectChat;
}
