using System.Collections.Generic;
using Project._Scripts.Runtime.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
    
        public AudioMixer audioMixer;

        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        [Header("Buttons")]
        public Button resetProfileButton;
        public Button exitButton;
        [Space]
        [HideInInspector] public float masterVolumeLevel;
        [HideInInspector] public float musicVolumeLevel;
        [HideInInspector] public float sfxVolumeLevel;

        [Space]

        public Text collectableCountText;
        public Text deathCountText;
        public Text playTimeText;

        [Header("LanguageAttributes")]
        public Text languageHeader;
        public Button previousLanguage;
        public Button nextLanguage;
        private void Awake()
        {
            if(Instance == null) { Instance = this; }
            else
            {
                Instance.masterVolumeSlider = masterVolumeSlider;
                Instance.musicVolumeSlider = musicVolumeSlider;
                Instance.sfxVolumeSlider = sfxVolumeSlider;

                Instance.resetProfileButton = resetProfileButton;
                Instance.exitButton = exitButton;

                Instance.collectableCountText = collectableCountText;



                Destroy(gameObject);
            }
            Cursor.visible = true;
            //DontDestroyOnLoad(this);


            if (PlayerPrefs.HasKey("Language"))
            {
                // SaveManager.Instance.data.language = PlayerPrefs.GetString("Language");
                // LocalizationManager.Instance.language = (LocalizationManager.Language)System.Enum.Parse(typeof(LocalizationManager.Language), SaveManager.Instance.data.language);
            }

            languageHeader.text = LocalizationManager.Instance.language.ToString();
        }
        private void OnEnable()
        {
            masterVolumeSlider.onValueChanged.AddListener(delegate { UI_SetMasterVolumeLevel(); });
            musicVolumeSlider.onValueChanged.AddListener(delegate { UI_SetMusicVolumeLevel(); });
            sfxVolumeSlider.onValueChanged.AddListener(delegate { UI_SetSFXVolumeLevel(); });

            previousLanguage.onClick.AddListener(delegate { PreviousLanguage(); });
            nextLanguage.onClick.AddListener(delegate { NextLanguage(); });

            // SaveManager.Instance.onSaveLoaded += LoadSliderVolumes;
            LoadSliderVolumes();

        }


        public void SetTextToHoursNMinutes(Text _timeText, float _time)
        {
            int time = (int)_time;
        
            int hours = (int)(time / 3600);

            time = time - (hours * 3600);

            int minutes = (int)(time / 60);

            time = time - (minutes * 60);

            int seconds = (int)(time / 60);

            //_timeText.text = ((double)hours).ToString() + ":" + ((double)minutes).ToString();
            _timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);

        }

        private void OnDisable()
        {
            previousLanguage.onClick.RemoveListener(delegate { PreviousLanguage(); });
            nextLanguage.onClick.RemoveListener(delegate { NextLanguage(); });

            // SaveManager.Instance.onSaveLoaded -= LoadSliderVolumes;
        }


    #region LanguageSettings

        public void NextLanguage()
        {

            int currentLanguageIndex = (int)LocalizationManager.Instance.language;
            previousLanguage.interactable = true;

            if (currentLanguageIndex < (int)(LocalizationManager.Language.CountOfLanguages-1)){

                LocalizationManager.Language theEnum = LocalizationManager.Instance.language;
                int toInteger = (int)theEnum+1;
                LocalizationManager.Language newLanguage = (LocalizationManager.Language)(toInteger);

                LocalizationManager.Instance.language = newLanguage;

                UpdateLanguage();
            }

        }

        public void PreviousLanguage()
        {

            int currentLanguageIndex = (int)LocalizationManager.Instance.language;
            nextLanguage.interactable = true;

            if (currentLanguageIndex > 0)
            {

                LocalizationManager.Language theEnum = LocalizationManager.Instance.language;
                int toInteger = (int)theEnum-1;
                LocalizationManager.Language newLanguage = (LocalizationManager.Language)(toInteger);

                LocalizationManager.Instance.language = newLanguage;

                UpdateLanguage();
            }
        }

        public static List<T> FindObjectsOfTypeAll<T>()
        {
            List<T> results = new List<T>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.isLoaded)
                {
                    var allGameObjects = s.GetRootGameObjects();
                    for (int j = 0; j < allGameObjects.Length; j++)
                    {
                        var go = allGameObjects[j];
                        results.AddRange(go.GetComponentsInChildren<T>(true));
                    }
                }
            }
            return results;
        }
        public void UpdateLanguage()
        {
            // SaveManager.Instance.data.language = LocalizationManager.Instance.language.ToString();
            // PlayerPrefs.SetString("Language", SaveManager.Instance.data.language);
            languageHeader.text = LocalizationManager.Instance.language.ToString();



            List<LocalizationData> localizationDatas = FindObjectsOfTypeAll<LocalizationData>();

            foreach(LocalizationData ld in localizationDatas)
            {
                ld.UpdateText();
            }
            //for(int i = 0; i < localizationDatas.Count; i++)
            //{
            //    localizationDatas[i].UpdateText();
            //}
        }

    #endregion
        public void UI_ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            // SaveManager.Instance.ClearData();
        }

        // public void LateUpdate()
        // {
        //     if(collectableCountText != null)
        //         if(collectableCountText.gameObject.activeInHierarchy)
        //             collectableCountText.text = "x" + SaveManager.Instance?.data.collectableItems.Find(x => x.collectableName == "Banana").collectableCount.ToString();
        //
        //     if (deathCountText != null)
        //         if (deathCountText.gameObject.activeInHierarchy)
        //             deathCountText.text = "x" + SaveManager.Instance?.data.deathCount.ToString();
        //
        //     if(playTimeText != null)
        //     {
        //         if (playTimeText.gameObject.activeInHierarchy)
        //         {
        //             SetTextToHoursNMinutes(playTimeText, SaveManager.Instance.data.playTime);
        //         }
        //     }
        //
        //     LoadSliderVolumes();
        // }

        public void Start()
        {
            //PLAY AUDIO
            
            //----------
        }

    #region LoadingData
        public void LoadSliderVolumes()
        {        
            //-------LOADING VOLUME DATA-----------
        

            //-------------------------------------
        }
    #endregion

    #region UIInteractions
        public void UI_StartButton()
        {
            var sceneBuildIndex = 1;
            SceneManager.LoadScene(sceneBuildIndex);
        }

    #endregion

    #region SliderAttributes
        public void UI_SetMasterVolumeLevel()
        {
            masterVolumeLevel = Mathf.Log10(masterVolumeSlider.value) * 60;
            audioMixer.SetFloat("MasterVolume", masterVolumeLevel);

            // SaveManager.Instance.data.audioDatas.Find(x => x.audioType == "MasterVolume").sliderValue = masterVolumeSlider.value;
            // SaveManager.Instance.data.audioDatas.Find(x => x.audioType == "MasterVolume").volumeLevel = masterVolumeLevel;
            // SaveManager.Instance.onGameSaved();
        }

        public void UI_SetMusicVolumeLevel()
        {
            musicVolumeLevel = Mathf.Log10(musicVolumeSlider.value) * 60;
            audioMixer.SetFloat("MusicVolume", musicVolumeLevel);

            // SaveManager.Instance.data.audioDatas.Find(x => x.audioType == "MusicVolume").sliderValue = musicVolumeSlider.value;
            // SaveManager.Instance.data.audioDatas.Find(x => x.audioType == "MusicVolume").volumeLevel = musicVolumeLevel;
            // SaveManager.Instance.onGameSaved();
        }

        public void UI_SetSFXVolumeLevel()
        {
            sfxVolumeLevel = Mathf.Log10(sfxVolumeSlider.value) * 60;
            audioMixer.SetFloat("SFXVolume", sfxVolumeLevel);

            // SaveManager.Instance.data.audioDatas.Find(x => x.audioType == "SFXVolume").sliderValue = sfxVolumeSlider.value;
            // SaveManager.Instance.data.audioDatas.Find(x => x.audioType == "SFXVolume").volumeLevel = sfxVolumeLevel;
            // SaveManager.Instance.onGameSaved();
        }
    #endregion
    }
}
