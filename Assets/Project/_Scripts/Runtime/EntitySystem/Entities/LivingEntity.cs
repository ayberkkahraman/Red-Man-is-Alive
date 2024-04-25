using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

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
        
        public void Die()
        {
            ManagerContainer.Instance.GetInstance<CameraManager>().UpdateFollowTarget(null);
            // ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(DeathAudio);
            Animator.speed = 1f;
            Animator.SetTrigger(Death);
        }
    }
}