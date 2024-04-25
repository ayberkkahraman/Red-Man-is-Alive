using System;
using System.Linq;
using Project._Scripts.Runtime.Library.SubSystems;
using UnityEngine;

namespace Project._Scripts.Runtime.InGame.Dynamics.Traps
{
  public abstract class TrapBase : MonoBehaviour
  {
    public delegate void OnTriggered(Collider collider);
    public OnTriggered OnTriggeredHandler;
    
    protected Rigidbody TargetRigidbody { get; set; }
    protected Animator TargetAnimator { get; set; }

    protected abstract void OnTrigger(Collider collider);

    protected virtual void OnEnable() => Init();
    protected virtual void OnDisable() => DeInit();
    protected virtual void Init() => OnTriggeredHandler += OnTrigger;
    protected virtual void DeInit() => OnTriggeredHandler -= OnTrigger;
    
    protected void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

      if (TargetRigidbody == null)
      {
        TargetRigidbody = other.GetComponent<Rigidbody>();
        TargetAnimator = other.GetComponent<Animator>() != null ? other.GetComponent<Animator>() : other.GetComponentInParent<Animator>();
      }
      
      OnTriggeredHandler?.Invoke(other);

      // GetComponents<Collider>().ToList().ForEach(x => x.enabled = false);

      enabled = false;
    }
  }
}
