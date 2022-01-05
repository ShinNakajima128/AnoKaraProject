using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ScenarioMasterData
{
    public class ScenarioMasterDataClass<T>
    {
        public T[] Data;
    }

    [Serializable]
    public class DialogData
    {
        public int MessageId = default;
        [HideInInspector]
        public string Talker = default;
        [SerializeField]
        string[] m_talker = default;
        [HideInInspector]
        public string Position = default;
        [SerializeField]
        int[] m_position = default;
        public int BackgroundType = default;
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
        [HideInInspector]
        public string VoiceId = default;
        [SerializeField]
        string[] m_allVoiceId = default;
        [SerializeField,TextArea(0, 10)]
        string[] m_allMessages = default;

        public string[] AllTalker { get => m_talker; } 
        public int[] AllPosition { get => m_position; }
        public int[] FaceTypes => m_faceTypes; 
        public string[] AllMessages { get => m_allMessages; set => m_allMessages = value; }
        public string[] AllVoiceId { get => m_allVoiceId; }
        public void MessagesAndFacetypeToArray()
        {
            string[] del = { "\n" };
            m_talker = Talker.Split(del, StringSplitOptions.None);
            m_allVoiceId = VoiceId.Split(del, StringSplitOptions.None);
            m_allMessages = Messages.Split(del, StringSplitOptions.None);
            var p = Position.Split(del, StringSplitOptions.None);
            m_position = new int[p.Length];

            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] != "")
                {
                    m_position[i] = int.Parse(p[i]);
                }
            }

            var f = FaceType.Split(del, StringSplitOptions.None);
            m_faceTypes = new int[f.Length];

            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] != "")
                {
                    m_faceTypes[i] = int.Parse(f[i]);
                }               
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
