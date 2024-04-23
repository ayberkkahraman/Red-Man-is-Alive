using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Project._Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue")]
    public class DialogueData : ScriptableObject
    {
        public List<Dialogue> Dialogues;
        public DialogueOptionData DialogueOptionData;

        [System.Serializable]
        public struct Dialogue
        {
            public CharacterData CharacterData;

            [TextArea(3, 10)]
            public string Sentence;
        }
    }
}