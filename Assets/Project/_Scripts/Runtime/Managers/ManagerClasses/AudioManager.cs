using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.ScriptableObjects.Audio;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class AudioManager : MonoBehaviour
    {
        #region Components
        public AudioMixer MasterMixer;
        private AudioSource _bgmSource;
        #endregion
        
        #region Fields
        [Space]
        [Header("PoolObjects")]
        [SerializeField] private GameObject MainSfxPoolObject;
        [SerializeField] private GameObject SecondarySfxPoolObject;
        
        private List<AudioSource> _mainSfxPool = new();
        private List<AudioSource> _secondarySfxPool = new();

        private List<AudioData> _audioDatas;

        public AudioSource BGMAudio;

        #endregion

        public string StartBGMName;

        #region OnAwake
        public bool PlayOnAwake;

        public static AudioManager Instance { get; set; }

        protected void Awake()
        {
            //---------------------BASIC SINGLETON---------------------
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            //----------------------------------------------------------
            
            Initialize();
            InitializeAudioResources();
        }

        private void Start()
        {
            if(PlayOnAwake)ChangeBGMAudio(StartBGMName);
        }
        
        

        public void ChangeBGMAudio(string audioName)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeBGMCoroutine(audioName, GetAudioByName(audioName).Volume));
        }

        public void StopBGM(float duration = 3f)
        {
            StartCoroutine(StopBGMCoroutine(duration));
        }

        private IEnumerator StopBGMCoroutine(float duration)
        {
            while (BGMAudio.volume > 0)
            {
                BGMAudio.volume -= Time.deltaTime / duration;
                yield return null;
            }
        }

        private IEnumerator ChangeBGMCoroutine(string audioName, float value = 1)
        {
            if (BGMAudio.volume != 0)
            {
                float volume = BGMAudio.volume;
                while (BGMAudio.volume > 0)
                {
                    BGMAudio.volume -= Time.deltaTime / 1;
                    yield return null;
                }
            }

            StartCoroutine(ActivateBGMCoroutine(audioName, value));
        }
        
        private IEnumerator ActivateBGMCoroutine(string audioName, float value)
        {
            BGMAudio.clip = GetAudioByName(audioName).AudioClip;
            BGMAudio.volume = 0f;
            
            BGMAudio.Play();

            while (BGMAudio.volume < value)
            {
                BGMAudio.volume += Time.deltaTime / 2;
                yield return null;
            }
        }
        
        #endregion


        #region Audio Interactions

        private void Initialize()
        {
            //------------------------------INITIALIZING THE AUDIO CHANNELS----------------------------------
            _mainSfxPool = MainSfxPoolObject.GetComponentsInChildren<AudioSource>().ToList();
            _secondarySfxPool = SecondarySfxPoolObject.GetComponentsInChildren<AudioSource>().ToList();

        }

        private void InitializeAudioResources()
        {
            _audioDatas = Resources.LoadAll<AudioData>(nameof(Audio)).ToList();
        }

        /// <summary>
        /// Play Audio on UI Interaction
        /// </summary>
        public void UIF_PlayAudio(string audioName)
        {
            var audioData = GetAudioByName(audioName);
            PlayAudio(audioData.AudioClip, audioData.Volume, audioData.PitchVariation, audioData.Type);
        }

        /// <summary>
        /// Play audio clip with default settings
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        /// <param name="pitchVariation"></param>
        /// <param name="type"></param>
        public void PlayAudio(AudioClip clip, float volume = 1f, float pitchVariation = 0f,
            AudioData.AudioType type = AudioData.AudioType.MainSfx)
        {
            //Null Check
            if (clip == null) return;

            //Get the audio source
            AudioSource source = GetAvailableAudioSource(type);

            //----------------------AUDIO SETTINGS----------------------------
            source.clip = clip;
            source.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
            source.volume = volume;
            //----------------------------------------------------------------

            //Play audio
            source.Play();
        }

        /// <summary>
        /// Play audio based on given audio object
        /// </summary>
        /// <param name="audioObject"></param>
        public void PlayAudio(AudioData audioObject)
        {
            //Null Check
            if (audioObject.AudioClip == null) return;

            //Get the audio source
            AudioSource source = GetAvailableAudioSource(audioObject.Type);

            //-----------------------------------AUDIO SETTINGS-----------------------------------------
            source.clip = audioObject.AudioClip;
            if (audioObject.CertainPitch)
            {
                source.pitch = audioObject.PitchVariation+.5f + Random.Range(-.1f, .1f);
            }
            else
            {
                source.pitch = 1 + Random.Range(audioObject.PitchVariation, -audioObject.PitchVariation);
            }

            source.volume = audioObject.Volume;
            //------------------------------------------------------------------------------------------

            //Play audio
            source.Play();
        }

        public void PlayAudio(AudioData audioObject, float volume, float pitchVariation)
        {
            //Null Check
            if (audioObject.AudioClip == null) return;

            //Get the audio source
            AudioSource source = GetAvailableAudioSource(audioObject.Type);

            //-----------------------------------AUDIO SETTINGS-----------------------------------------
            source.clip = audioObject.AudioClip;
            source.pitch = 1 + Random.Range(pitchVariation, -pitchVariation);
            source.volume = volume;
            //------------------------------------------------------------------------------------------

            //Play audio
            source.Play();
        }

        /// <summary>
        /// Play audio with it's name
        /// </summary>
        /// <param name="audioName"></param>
        public void PlayAudio(string audioName)
        {
            var audioData = GetAudioByName(audioName);

            if (audioData is null)
            {
                Debug.LogError($"There is not audio like{audioName}");
                return;
            }

            PlayAudio(audioData);
        }
  
        #endregion

        #region Audio Gathering
        /// <summary>
        /// Returns the audio based on it's name from the given audio list
        /// </summary>
        /// <param name="audioName"></param>
        /// <returns></returns>
        public AudioData GetAudioByName(string audioName)
        {
            return _audioDatas.Find(x => x.name == audioName);
        }

        public AudioData GetAudioDataByClip(AudioClip clip)
        {
            return _audioDatas.ToList().Find(x => x.AudioClip == clip);
        }

        /// <summary>
        /// Returns the available audio source channel
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AudioSource GetAvailableAudioSource(AudioData.AudioType type)
        {
            AudioSource source;
            switch (type)
            {
                case AudioData.AudioType.BGM:
                    return _bgmSource;
                case AudioData.AudioType.MainSfx:
                    if (_mainSfxPool.Exists(x => x.isPlaying == false))
                    {
                        return _mainSfxPool.Find(x => x.isPlaying == false);
                    }
                    source = _mainSfxPool.OrderBy(x => x.time).Last();

                    source.Stop();
                    return source;
                case AudioData.AudioType.SecondarySfx:
                    if (_secondarySfxPool.Exists(x => x.isPlaying == false))
                    {
                        return _secondarySfxPool.Find(x => x.isPlaying == false);
                    }
                    source = _secondarySfxPool.OrderBy(x => x.time).Last();

                    source.Stop();
                    return source;
                default:
                    return null;
            }
        }

        #endregion

    }
#region Audio Class

    [System.Serializable]
    public class Audio
    {
        public string AudioName;
        public AudioData AudioData;
    }
#endregion
}