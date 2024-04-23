using System;
using System.Collections.Generic;
using Project._Scripts.Runtime.Managers.Manager;
using UnityEngine;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class AbilityManager : MonoBehaviour
  {
    [Serializable]
    public class Ability
    {
      public string Name;
      public bool Active;
    }

    [SerializeField]private List<Ability> _abilities;
    public static List<Ability> Abilities;

    private void Start()
    {
      Abilities = _abilities;
      
      LoadAbilities();
    }

    public static Ability GetAbilityByName(string abilityName) => Abilities.Find(x => x.Name == abilityName);

    public static void ActivateAbility(string abilityName)
    {
      GetAbilityByName(abilityName).Active = true;
      SaveManager.SaveData($"{abilityName}", true);
    }

    public static bool IsActive(string abilityName) => GetAbilityByName(abilityName).Active;

    private void LoadAbilities()
    {
      foreach (var ability in Abilities)
      {
        // Debug.Log($"{ability.Name} " + SaveManager.LoadData($"{ability.Name}", false));
        ability.Active = SaveManager.LoadData($"{ability.Name}", false);
      }
    }
  }
}
