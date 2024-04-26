using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project._Scripts.Runtime.Managers.Manager
{
  public class ManagerContainer : MonoBehaviour{
  #region Fields
    public bool DestroyOnLoad = true;
    public static ManagerContainer Instance;

    public List<MonoBehaviour> Managers;
  #endregion

  #region Singleton
    private void Awake()
    {
      if(!DestroyOnLoad) DontDestroyOnLoad(gameObject);
      
      if (Instance == null) Instance = this;
      else { Destroy(Instance); }
    }
  #endregion
    
    /// Get Instance for singleton access
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetInstance<T>() where T : MonoBehaviour
    {
      //CHECKS IF THE MANAGERS LIST CONTAINS THE "T" INSTANCE
      if (Managers.Exists(x => x as T != null))
      {
        //FINDS THE INSTANCE FOR ASSIGNING TO ACCESS
        return Managers.Find(x => x as T != null) as T;
      }
      // else { Debug.LogError($"Managers list not contains the **{typeof(T)}**");
      return null;
    }
  
    public void AddInstance<T>(T instance) where T : MonoBehaviour
    {
      if(Managers.Contains(this)) return;

      Managers.Add(instance);
    }

    public MonoBehaviour GetSpesificInstance<T>(T instance) where T : MonoBehaviour
    {
      return Managers.Find(x => x == instance);
    }

    public void RemoveInstance<T>(T instance) where T : MonoBehaviour
    {
      Managers.Remove(instance);
    }

    public IEnumerator RunAfterSecondsCoroutine(float delay, Action action)
    {
      yield return new WaitForSeconds(delay);
      
      action?.Invoke();
    }

    public void RunAfterSeconds(float delay, Action action)
    {
      StartCoroutine(RunAfterSecondsCoroutine(delay, action));
    }
    
  }
}
