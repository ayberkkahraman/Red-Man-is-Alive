using System;
using UnityEngine;

namespace Project._Scripts.Runtime.ScriptableObjects
{
  [CreateAssetMenu(fileName = "DialogueSequenceData", menuName = "DialogueSequence")]
  public class DialogueOptionData : ScriptableObject
  {
    public DialogueOption[] DialogueOptions;
    
    [Serializable]
    public struct DialogueOption
    {
      [TextArea(3, 10)]
      public string Sentence;
      public DialogueData DialogueData;
    }
  }
}
