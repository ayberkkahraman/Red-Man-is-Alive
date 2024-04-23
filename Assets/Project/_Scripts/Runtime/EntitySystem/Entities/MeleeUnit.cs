using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.Entity.EntitySystem.Entities
{
  public class MeleeUnit : EnemyUnit
  {
    protected override void Start()
    {
      base.Start();
    }
    public override void Attack()
    {
      IsAttacking = true;
      Animator.SetTrigger(AttackAnimationHash);
      CurrentState = State.Attack;
    }
    public virtual void ANIM_EVENT_Attack()
    {
      Collider2D[] enemiesInRange =
        Physics2D.OverlapCircleAll(AttackPoint.position, UnitData.DamageRadius, UnitData.TargetLayers);
            
      if(enemiesInRange.Length == 0) return;

      foreach (var enemy in enemiesInRange)
      {
        LivingEntity entity = enemy.GetComponent<LivingEntity>();
        if(entity.Health <= 0) return;
        Attack(entity, true);
        entity.GetComponent<Rigidbody2D>().AddForce(transform.right * UnitData.ForceStrength, ForceMode2D.Impulse);
      }

            
      CameraManager.ShakeCamera(CameraManager.ShakeIntensity * 1.85f, CameraManager.ShakeDuration);
    }
  }
}
