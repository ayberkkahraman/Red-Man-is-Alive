using System;
using Project._Scripts.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Project._Scripts.Runtime.InGame.Dynamics.Zone
{
  public class TriggerZone : MonoBehaviour, IInteractable
  {
    public UnityEvent OnTriggeredEvent;

    private void OnEnable()
    {
      OnTriggeredHandler += InitializeTriggerEvent;
    }

    private void OnDisable()
    {
      OnTriggeredHandler -= InitializeTriggerEvent;
    }

    private void InitializeTriggerEvent(Collider other)
    {
      OnTriggeredEvent?.Invoke();
    }
    public void OnTrigger(Collider triggeredCollider)
    {
      OnTriggeredHandler?.Invoke(triggeredCollider);
    }
    public IInteractable.OnTriggered OnTriggeredHandler { get; set; }

    public void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
      
      OnTrigger(other);
      
      enabled = false;
    }
  }
}
