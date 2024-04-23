using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.Entity.EntitySystem.Entities
{
    public class Unit : LivingEntity
    {
        protected float DefaultCooldown;
        public float CurrentCooldown { get; set; }
        protected static readonly int AttackAnimationHash = Animator.StringToHash("Attack");

        protected CameraManager CameraManager;
        protected override void Start()
        {
            base.Start();

            CameraManager = ManagerContainer.Instance.GetInstance<CameraManager>();
            DefaultCooldown = UnitData.AttackCooldown;
            CurrentCooldown = UnitData.AttackCooldown;
        }

        public virtual void Update()
        {
            if (CurrentCooldown < DefaultCooldown) CurrentCooldown += Time.deltaTime;
        }
        public override void Attack()
        {
            
        }
        protected void CheckForAttack()
        {
            if(Health <= 0) return;
            
            if(CurrentCooldown < DefaultCooldown) return;
            
            Attack();
            
            CurrentCooldown = 0;
        }
    }
}