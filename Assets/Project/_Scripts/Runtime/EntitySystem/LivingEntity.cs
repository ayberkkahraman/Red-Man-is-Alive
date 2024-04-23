using System;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using Project._Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.Runtime.Entity.EntitySystem
{
    public abstract class LivingEntity : MonoBehaviour
    {
        public HealthBar.HealthBar HealthBar;
        public UnitData UnitData;
        public Transform AttackPoint;
        public int MaxHealth { get; set; }
        public bool IsVulnerable { get; set; }
        public Action<Transform> BlockCallback;
        public float Health{ get; set; }
        
        public string AttackAudio;
        public string TakeDamageAudio;
        public string DeathAudio;
        public delegate void OnAttack(LivingEntity entity, bool dodgeable);
        public delegate void OnTakeDamage(int damage);
        public delegate void OnDie();

        public OnTakeDamage OnTakeDamageHandler;
        protected OnDie OnDieHandler;
        public OnAttack OnAttackHandler;
        
        private static readonly int TakeHit = Animator.StringToHash("TakeHit");
        private static readonly int Death = Animator.StringToHash("Death");

        protected Animator Animator { get; set; }
    

        protected virtual void Start()
        {
            MaxHealth = UnitData.Health;
            Health = MaxHealth;
            Animator = GetComponent<Animator>();
        }

        protected virtual void OnEnable()
        {
            OnTakeDamageHandler += TakeDamage;
            OnDieHandler += Die;
            OnAttackHandler += Attack;
        }
        
        protected virtual void OnDisable()
        {
            OnTakeDamageHandler -= TakeDamage;
            OnDieHandler -= Die;
            OnAttackHandler -= Attack;
        }
        
        public void TakeHitStateEnd()
        {
            Animator.SetBool(TakeHit, false);
        }
        public void Die()
        {
            ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(DeathAudio);
            Animator.speed = 1f;
            Animator.SetTrigger(Death);
        }

        public void Attack(LivingEntity entity, bool dodgeable)
        {
            if(entity == null) return;
            
            if(entity.Health <= 0 ) return;
            
            if(!string.IsNullOrEmpty(AttackAudio))ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(AttackAudio);
            if (dodgeable)
            {
                if (IsAttackApproved(entity))
                {
                    if (!entity.IsVulnerable)
                    {
                        entity.OnTakeDamageHandler(UnitData.Damage);
                        return;
                    }
                    
                    entity.BlockCallback?.Invoke(transform);
                }

                else
                {
                    entity.OnTakeDamageHandler(UnitData.Damage);
                }
            }

            else
            {
                entity.OnTakeDamageHandler(UnitData.Damage);
            }
        }

        public void DeadState()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Animator.speed = 0f;
            enabled = false;
        }

        protected bool IsAttackApproved(LivingEntity entity)
        {
            return entity.transform.right != transform.right;
        }

        public void TakeDamage(int damage)
        {
            if (Health <= 0) return;

            
            Animator.SetTrigger(TakeHit);
            Health -= damage;
            if(HealthBar != null){HealthBar.UpdateHealthBar(damage);}

            if (Health <= 0) { OnDieHandler(); return;}
            
            ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(TakeDamageAudio);
        }

        public abstract void Attack();
    }
}