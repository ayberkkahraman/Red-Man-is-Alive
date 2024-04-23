using System.Collections.Generic;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;
using UnityEngine.Events;

namespace Project._Scripts.Runtime.Entity.EntitySystem.Entities
{
  public class Player : Unit
  {
    public GameObject BlockVFX;
    public UnityEvent OnDeadEvent;
    public UnityEvent OnBlockEvent;

    private readonly Collider2D[] _enemiesInRange = new Collider2D[10];
    private HashSet<LivingEntity> _entities;
    protected override void Start()
    {
      base.Start();

      BlockCallback = Block;
      OnDieHandler += OnDead;
    }

    public void Block(Transform vfxTransform)
    {
      CameraManager.ShakeCamera(.65f, .4f, .1f);
      BlockVFX.SetActive(true);
      OnBlockEvent?.Invoke();
    }

    public void OnDead()
    {
      OnDeadEvent?.Invoke();
    }
    public override void Attack()
    {
      ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio("OrcAttack");
      
      int enemyCount = Physics2D.OverlapCircleNonAlloc
        (
          AttackPoint.position, 
          UnitData.DamageRadius, 
          _enemiesInRange, 
          UnitData.TargetLayers
        );
      
      if (enemyCount == 0) return;

      _entities = new HashSet<LivingEntity>();

      foreach (var enemy in _enemiesInRange)
      {
        _entities.Add(enemy.GetComponent<LivingEntity>());
      }
      
      foreach (var entity in _entities)
      {
        if(entity.Health <= 0) return;
        entity.OnTakeDamageHandler(UnitData.Damage);
        entity.GetComponent<Rigidbody2D>().AddForce(transform.right * UnitData.ForceStrength, ForceMode2D.Impulse);
      }

      CameraManager.ShakeCamera();
    }
  }
  
}
