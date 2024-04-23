using UnityEngine;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
    public class TutorialManager : MonoBehaviour
    {

        public static TutorialManager Instance;
    
        private void Awake()
        {
            Instance = this;
        }

    }
}
