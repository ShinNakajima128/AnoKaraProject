using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DialogMasterData
{
    public class DialogMasterDataClass<T>
    {
        public int BGType;
        public T[] Data;
    }

    [Serializable]
    public class CharacterData
    {
        public int MessageId = default;
        public string Talker = default;
        public int Position = default;
        [HideInInspector]
        public string FaceType = default;
        [SerializeField]
        int[] m_faceTypes = default;
        public string StartAnimationType = default;
        public string EndAnimationType = default;
        [HideInInspector]
        public string Messages = default;
        public int ChoicesId = default;
        public int NextId = default;
        [SerializeField,TextArea(0, 10)]
        string[] m_allMessages = default;

        public int[] FaceTypes => m_faceTypes; 
        public string[] AllMessages { get => m_allMessages; set => m_allMessages = value; }

        public void MessagesAndFacetypeToArray()
        {
            string[] del = { "\n" };
            m_allMessages = Messages.Split(del, StringSplitOptions.None);
            var f = FaceType.Split(del, StringSplitOptions.None);
            m_faceTypes = new int[f.Length];

            for (int i = 0; i < f.Length; i++)
            {
                m_faceTypes[i] = int.Parse(f[i]);
            }
        }
    }

    [Serializable]
    public class ChoicesData
    {
        public int ChoicesId = default;

        [HideInInspector]
        public string Choices = default;

        [HideInInspector]
        public string NextMessageId = default;

        [SerializeField, TextArea(0, 10)]
        string[] m_allChoices = default;

        [SerializeField]
        int[] m_nextId = default;

        public string[] AllChoices { get => m_allChoices; set => m_allChoices = value; }
        public int[] NextId { get => m_nextId; set => m_nextId = value; }
        public void MessagesAndNextIdToArray()
        {
            string[] del = { "\n" };
            m_allChoices = Choices.Split(del, StringSplitOptions.None);
            var n = NextMessageId.Split(del, StringSplitOptions.None);
            m_nextId = new int[n.Length];
            for (int i = 0; i < n.Length; i++)
            {
                m_nextId[i] = int.Parse(n[i]);
            }
        }
    }
}
