using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogMasterData;

public class Test : MonoBehaviour
{
    [SerializeField]
    CharacterData m_data = default;

    void Start()
    {
        m_data = new CharacterData();

        m_data.Talker = "みさき";
        m_data.Messages = "aaaaaaaaa," +
                          "bbbbbbb," +
                          "ccccccc";
    }
}
