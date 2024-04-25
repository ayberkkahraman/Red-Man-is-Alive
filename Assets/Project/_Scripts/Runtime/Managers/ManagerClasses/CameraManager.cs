using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class CameraManager : MonoBehaviour
    {
        #region Cameras
        public Camera MainCamera;
        public CinemachineVirtualCamera DefaultCam{ get; set; }
        public CinemachineVirtualCamera CharacterCamera;
        public CinemachineVirtualCameraBase CurrentCamera { get; set; }
        #endregion

        #region Fields
        public List<CinemachineVirtualCamera> CinemachineVirtualCameras { get; set; }
        private CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin{ get; set; }
        [Header("Camera Shake")]
        [Space]
        [Range(.5f, 7.5f)]
        public float ShakeIntensity = 1.0f;
        [Range(0.0f, .5f)]
        public float ShakeDuration = .1f;
        #endregion

    #region Unity Functions
        private void OnEnable()
        {
            Init();
        }
    #endregion

    
    #region Init
        public void Init()
        {
            //Setting the cinemachine cameras for the list to be let it accessible
            CinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>().ToList();

            DefaultCam = CharacterCamera;
            MainCamera = Camera.main;
            CurrentCamera = CharacterCamera;
            CinemachineBasicMultiChannelPerlin = DefaultCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    #endregion
    
    #region Camera

        /// <summary>
        /// Changes the current active camera in a spesific time
        /// </summary>
        /// <param name="targetCamera"></param>
        public void ChangeActiveCamera(CinemachineVirtualCameraBase targetCamera)
        {
            if (targetCamera == CurrentCamera) return;
            
            //Disables all the cameras to access only one camera easily
            foreach (CinemachineVirtualCamera cinemachineVirtualCamera in CinemachineVirtualCameras)
            {
                cinemachineVirtualCamera.Priority = 0;
            }

            DefaultCam.Priority = 0;
            targetCamera.Priority = 10;

            CurrentCamera = targetCamera;
            CharacterCamera = CurrentCamera as CinemachineVirtualCamera;
            DefaultCam = CharacterCamera;
            CinemachineBasicMultiChannelPerlin = DefaultCam != null ? DefaultCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() : null;
            //-----------------------------------------------
        }

        /// <summary>
        /// Updates the follow transform of the Camera
        /// </summary>
        /// <param name="targetTransform"></param>
        public void UpdateFollowTarget(Transform targetTransform)
        {
            CurrentCamera.Follow = targetTransform;
        }


        public void ShakeCamera()
        {
            StartCoroutine(ShakeCameraCoroutine(ShakeIntensity, ShakeDuration));
        }
    
        public void ShakeCamera(float intensity, float duration, float frequencyGain = 0f)
        {
            StartCoroutine(ShakeCameraCoroutine(intensity, duration, frequencyGain));
        }
        
    
        /// <summary>
        /// Shakes the Camera
        /// </summary>
        /// <returns></returns>
        public IEnumerator ShakeCameraCoroutine(float intensity, float duration, float frequencyGain = 0f)
        {
            StartCoroutine(SmoothIncreaseAmplitude(.25f, intensity));
        
            var time = Time.time;
            while (Time.time < time + duration)
            {
                CinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyGain;
                yield return null;
            }

            StartCoroutine(SmoothReturnToZero(.5f));
        }
        
        public IEnumerator SmoothIncreaseAmplitude(float duration, float targetValue)
        {
            float startTime = Time.time;
            float startValue = CinemachineBasicMultiChannelPerlin.m_AmplitudeGain;

            while (Time.time < startTime + duration)
            {
                float elapsedTime = Time.time - startTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float newValue = Mathf.Lerp(startValue, targetValue, t);

                CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = newValue;

                yield return null;
            }
            
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = targetValue;
        }
        public IEnumerator SmoothReturnToZero(float duration)
        {
            float startTime = Time.time;
            float startValue = CinemachineBasicMultiChannelPerlin.m_AmplitudeGain;

            while (Time.time < startTime + duration)
            {
                float elapsedTime = Time.time - startTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float newValue = Mathf.Lerp(startValue, 0f, t);

                CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = newValue;

                yield return null;
            }
            
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
        #endregion
    
    }
}