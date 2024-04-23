using System;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class GameManager : MonoBehaviour
  {
    public CharacterData Data;

    private void Start()
    {
      PlayerPrefs.DeleteKey($"{Data.Name} DialogueIndex");
      PlayerPrefs.DeleteKey($"{Data.Name} IsInteractable");
      PlayerPrefs.DeleteKey($"Checkpoint");
      PlayerPrefs.DeleteKey($"Coin");
    }
  }
}
