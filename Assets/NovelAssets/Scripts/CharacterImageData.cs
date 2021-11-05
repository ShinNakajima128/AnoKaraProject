using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterImageType
{
    A,
    B,
    C,
    D,
    E,
    F
}

[CreateAssetMenu(menuName = "MyScriptable/Create CharacterImageData")]
public class CharacterImageData : ScriptableObject
{

    [SerializeField]
    string m_dataName = default;

    [Header("キャラクターの画像")]
    [SerializeField]
    Sprite[] m_characterImages = default;

    public string CharacterName => m_dataName;
    public Sprite[] CharacterImages => m_characterImages;
}
