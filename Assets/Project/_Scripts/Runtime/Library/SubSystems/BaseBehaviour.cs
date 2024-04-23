using System;
using System.Collections;
using System.Collections.Generic;
using Project._Scripts.Runtime.Managers.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project._Scripts.Runtime.Library.SubSystems
{
  public static class BaseBehaviour
  {
    public static void RunAfterSeconds(float delayTime, Action action)
    {
      ManagerContainer.Instance.StartCoroutine(RunAfterSecondsCoroutine(delayTime, action));
    }
    
    public static IEnumerator RunAfterSecondsCoroutine(float delayTime, Action action)
    {
      yield return new WaitForSeconds(delayTime);
      action?.Invoke();
    }

    public static T CastAs<T>(object castType)where T : MonoBehaviour
    {
      return castType as T;
    }

    public static string[] GetLayerNamesFromMask(LayerMask layerMask)
    {
      List<string> layerNames = new List<string>();

      for (int i = 0; i < 32; i++)
      {
        if (((1 << i) & layerMask.value) == 0)
          continue;
        string layerName = LayerMask.LayerToName(i);
        layerNames.Add(layerName);
      }

      return layerNames.ToArray();
    }

    public static bool RandomBool()
    {
      int randomValue = Random.Range(0, 2);

      return randomValue == 1;
    }
    
    public static void ExecuteWithChance(int chance, Action action)
    {
      int chanceRate = Random.Range(0, 101);
      
      if(chance <= chanceRate) action?.Invoke();
    }
  }
}
