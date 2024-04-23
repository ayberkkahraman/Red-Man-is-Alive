using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project._Scripts.Runtime.ScriptableObjects
{
    public class LocalizationData : MonoBehaviour
    {
        public List<LocalizationContent> LocalizationContents;

        public void Start()
        {
            UpdateText();
        }

        public void UpdateText()
        {
            // if (LocalizationManager.Instance == null) return;
            // GetComponent<Text>().text = LocalizationContents.Find(x => x.Language == LocalizationManager.Instance.language.ToString()).Content;
        }
    }

    [System.Serializable]
    public class LocalizationContent
    {
        public string Language;
        [TextArea]public string Content;
    }
}