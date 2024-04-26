using Project._Scripts.Runtime.EntitySystem.Entities;
using Project._Scripts.Runtime.Interfaces;
using UnityEngine;

namespace Project._Scripts.Runtime.InGame.Dynamics.Traps
{
  public abstract class TrapBase : MonoBehaviour, IInteractable
  {
    protected Rigidbody TargetRigidbody { get; set; }
    protected Animator TargetAnimator { get; set; }

    protected abstract void OnTrigger(Collider triggeredCollider);
    public IInteractable.OnTriggered OnTriggeredHandler { get; set; }

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

      enabled = false;
    }

    protected virtual void KillThePlayer(Collider triggeredCollider)
    {
      triggeredCollider.TryGetComponent(out LivingEntity entity);
      if (entity == null)
        triggeredCollider.GetComponentInParent<LivingEntity>().OnDieHandler();
      else  
        entity.OnDieHandler?.Invoke();
    }
    void IInteractable.OnTrigger(Collider triggeredCollider)
    {
      OnTrigger(triggeredCollider);
    }
  }
}
