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
        
        public void Die()
        {
            ManagerContainer.Instance.GetInstance<GameManager>().ActivateVignette(.35f);
            ManagerContainer.Instance.GetInstance<GameManager>().ActivatePaniniProjection(.35f);
            ManagerContainer.Instance.GetInstance<GameManager>().ActivateChromaticAberration(.5f);
            
            ManagerContainer.Instance.GetInstance<CameraManager>().UpdateFollowTarget(null);
            ManagerContainer.Instance.GetInstance<CameraManager>().ShakeCamera(25, .3f, .075f);
            //ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(DeathAudio);
            
            Animator.speed = 1f;
        }
    }
}