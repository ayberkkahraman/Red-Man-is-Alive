﻿using System;
using DG.Tweening;
using Project._Scripts.Runtime.EntitySystem.Entities;
using UnityEngine;

namespace Project._Scripts.Runtime.InGame.Dynamics.Traps
{
  public class Swiper : TrapBase
  {
    [Range(1f, 10f)][SerializeField] private float RotateSpeed = 1f;
    [Range(1f, 100f)][SerializeField] private float Force = 25f;
    protected override void OnTrigger(Collider triggeredCollider)
    {
      TargetAnimator.applyRootMotion = false;
      ForceImpact(transform.right * RotateSpeed);

      //Preventing the "Null Reference Exception" if the component is null
      triggeredCollider.TryGetComponent(out LivingEntity entity);
        entity.OnDieHandler?.Invoke();
    }

    private void Start()
    {
      transform.DOLocalRotate(Vector3.up * -36, .5f / RotateSpeed)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
    }
    
    public void ForceImpact(Vector3 direction)
    {
      TargetRigidbody.AddForce(direction * Force * -20f, ForceMode.Impulse);
    }
  }
}
