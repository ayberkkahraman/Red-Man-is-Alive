using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.Entity.EntitySystem.Entities
{
  public class RangedUnit : EnemyUnit
  {
    public Projectile.Projectile Projectile;
    public Transform ProjectileSpawnPoint;

    public override void Attack()
    {
      
      Animator.SetTrigger(AttackAnimationHash);
      CurrentState = State.Attack;
    }

    public void ANIM_EVENT_SpawnProjectile()
    {
      ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(AttackAudio);
      Projectile.Projectile projectile = Instantiate(Projectile, ProjectileSpawnPoint.position, transform.rotation, transform);
      projectile.Owner = this;
      projectile.Damage = UnitData.Damage;
    }
  }
}
