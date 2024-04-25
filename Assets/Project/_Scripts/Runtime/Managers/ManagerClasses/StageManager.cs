using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class StageManager : MonoBehaviour
    {
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.R)) LoadSceneAtIndex(SceneManager.GetActiveScene().buildIndex);
        }
        public void LoadSceneAtIndex(int index)
        {
            RunAfterDuration(() => SceneManager.LoadScene(index), 4f);
        }
        
        public void RunAfterDuration(Action action, float duration) => StartCoroutine(RunAfterSeconds(action, duration));

        static IEnumerator RunAfterSeconds(Action action, float duration)
        {
            yield return new WaitForSeconds(duration);
            action?.Invoke();
        }
    }
}
