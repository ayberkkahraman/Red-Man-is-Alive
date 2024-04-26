using UnityEngine;

namespace Project._Scripts.Runtime.ScriptableObjects.Audio
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Game/Audio")]
    public class AudioData : ScriptableObject
    {
    #region Derivatives
        public AudioClip AudioClip;
    #endregion
    
    #region Fields
        [Header("Attributes")]
        public AudioType Type;
        [Range(0,1f)]public float Volume = .5f;

        [Range(-.5f,.5f)]
        public float PitchVariation;
        public bool CertainPitch;
        public enum AudioType
        {
            BGM,
            MainSfx,
            SecondarySfx,
        }
    #endregion
    }
}