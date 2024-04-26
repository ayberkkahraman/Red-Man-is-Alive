using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;
using UnityEngine.Events;

namespace Project._Scripts.Runtime.EntitySystem.Entities
{
    public abstract class LivingEntity : MonoBehaviour
    {
        public string DeathAudio;
        public delegate void OnDie();
        public OnDie OnDieHandler;
        
        private static readonly int Death = Animator.StringToHash("Death");

        protected Animator Animator { get; set; }
        protected Rigidbody Rigidbody { get; set; }

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void OnEnable()
        {
            OnDieHandler += Die;
        }
        
        protected virtual void OnDisable()
        {
            OnDieHandler -= Die;
        }

        /// <summary>
        /// This method will be execute when the Player Dies whether interactions or entity targets
        /// </summary>
        protected abstract void Die();
    }
}