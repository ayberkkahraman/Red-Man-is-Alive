using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project._Scripts.Runtime.Library.Extensions
{
  public static class Extensions
  {
    #region Component Extensions
    public static void DestroyAfterSeconds(this GameObject gameObject, float delay)
    {
      Object.Destroy(gameObject, delay);
    }
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component
    {
      return gameObject.GetComponent<T>() != null;
    }
    
    public static bool HasInterface<T>(this GameObject gameObject)
    {
      return typeof(T).GetTypeInfo().IsInterface && gameObject.GetComponent<T>() != null;
    }
    
    public static bool HasComponent<T>(this Collider collider) where T : Component
    {
      return collider.GetComponent<T>() != null;
    }
    #endregion

    #region Class Configuration
        
    public static T CastAs<T>(this MonoBehaviour script,object castType)where T : MonoBehaviour
    {
      return castType as T;
    }
    #endregion

    #region Variable Configuration

    public static IEnumerator UpdateValue(float defaultValue, float targetValue, float duration, Action<float> updateValueCallback)
    {
      var timer = 0f;
      var initialValue = defaultValue;
      while (timer < duration)
      {
        defaultValue = Mathf.Lerp(initialValue, targetValue, timer / duration);
        updateValueCallback(defaultValue);
        timer += Time.deltaTime;
        yield return null;
      }
      defaultValue = targetValue;
      updateValueCallback(defaultValue);

    }
    #endregion
    
    public static string[] GetLayerNamesFromMask(this LayerMask layerMask)
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

    public static void SetAndSave<T>([NotNull] this T dataType, string dataName, T data)
    {
      if (dataType == null)
        throw new ArgumentNullException(nameof(dataType));
      dataType = data;
      SaveManager.SaveData(dataName, dataType);
    }

    public static float GetDiagonalLength(this BoxCollider boxCollider)
    {
      float a = boxCollider.transform.localScale.x;
      float b = boxCollider.transform.localScale.y;
      float c = boxCollider.transform.localScale.z;

      var n = a*a + c*c;
      var m = Mathf.Sqrt(n);
      var k = b*b;
      var l = Mathf.Sqrt(m + k);

      return l;
    }

    public static Vector3 DirectionTo(this Vector3 vector3, Vector3 to)
    {
      return -(vector3 - to).normalized;
    }

    public static string[] GetLayerNamesFromMask(this GameObject gameObject, LayerMask layerMask)
    {
      List<string> layerNames = new List<string>();

      for (int i = 0; i < 32; i++)
      {
        if (((1 << i) & layerMask.value) != 0)
        {
          string layerName = LayerMask.LayerToName(i);
          layerNames.Add(layerName);
        }
      }

      return layerNames.ToArray();
    }
    
    public static string[] GetLayerNamesFromMask(this Transform transform, LayerMask layerMask)
    {
      List<string> layerNames = new List<string>();

      for (int i = 0; i < 32; i++)
      {
        if (((1 << i) & layerMask.value) != 0)
        {
          string layerName = LayerMask.LayerToName(i);
          layerNames.Add(layerName);
        }
      }

      return layerNames.ToArray();
    }

  }

}
