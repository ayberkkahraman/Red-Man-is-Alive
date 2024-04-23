using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class BehaviourManager : MonoBehaviour
  {
    public void RunAfterSeconds(float delayTime, Action action)
    {
      StartCoroutine(RunAfterSecondsCoroutine(delayTime, action));
    }
    
    public IEnumerator RunAfterSecondsCoroutine(float delayTime, Action action)
    {
      yield return new WaitForSeconds(delayTime);
      action?.Invoke();
    }
  }
}
