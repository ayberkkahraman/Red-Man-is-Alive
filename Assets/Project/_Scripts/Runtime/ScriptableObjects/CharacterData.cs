using UnityEngine;

namespace Project._Scripts.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Character")]
    public class CharacterData : ScriptableObject
    {
        public string Name;
        public Sprite Image;
    }
}