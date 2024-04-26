using System;
using System.Collections;
using Project._Scripts.Runtime.Managers.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class StageManager : MonoBehaviour
    {
        /// <summary>
        /// Loads the scene in desired "Index"
        /// </summary>
        /// <param name="index"></param>
        public void LoadSceneAtIndex(int index)
        {
            ManagerContainer.Instance.RunAfterSeconds(3f, () => SceneManager.LoadScene(index));
        }
    }
}
