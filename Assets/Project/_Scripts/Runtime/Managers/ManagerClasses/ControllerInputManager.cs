using Project._Scripts.Runtime.Library.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class ControllerInputManager : MonoBehaviour
    {
        private void Awake()
        {
            InputController.CreateControllerInput();
        }

        private void OnEnable()
        {
            InputController.InitializeControllerInput();
        }

        private void OnDisable()
        {
            InputController.DeInitializeControllerInput();
        }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.R))
        //     {
        //         SceneTransitionManager.LoadScene(SceneTransitionManager.GetActiveScene().buildIndex);
        //     }
        //     if (Input.GetKeyDown(KeyCode.Y))
        //     {
        //         SceneTransitionManager.LoadScene(SceneTransitionManager.GetActiveScene().buildIndex+1);
        //     }
        //     if (Input.GetKeyDown(KeyCode.Backspace))
        //     {
        //         SceneTransitionManager.LoadScene(SceneTransitionManager.GetActiveScene().buildIndex-1);
        //     }
        // }

    }
}
