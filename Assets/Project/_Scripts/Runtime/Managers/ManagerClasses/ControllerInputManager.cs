using Project._Scripts.Runtime.Library.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class ControllerInputManager : MonoBehaviour
    {
        /// <summary>
        /// Input System Initializer Script 
        /// </summary>

        
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
    }
}
