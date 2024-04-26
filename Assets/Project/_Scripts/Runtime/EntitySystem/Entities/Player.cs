using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.EntitySystem.Entities
{
    public class Player : LivingEntity
    {
        public GameObject Ragdoll;
        private static readonly int VelocityAnimationHash = Animator.StringToHash("Velocity");
        protected override void OnEnable()
        {
            base.OnEnable();

            OnDieHandler += ActivateRagdoll;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            OnDieHandler -= ActivateRagdoll;
        }
        protected override void Die()
        {
            ManagerContainer.Instance.GetInstance<EffectManager>().ActivateVignette(.35f);
            ManagerContainer.Instance.GetInstance<EffectManager>().ActivatePaniniProjection(.35f);
            ManagerContainer.Instance.GetInstance<EffectManager>().ActivateChromaticAberration(.5f);
            
            ManagerContainer.Instance.GetInstance<CameraManager>().UpdateFollowTarget(null);
            ManagerContainer.Instance.GetInstance<CameraManager>().ShakeCamera(25, .3f, .075f);
            ManagerContainer.Instance.RunAfterSeconds(2f, () =>
            {
                ManagerContainer.Instance.GetInstance<GameManager>().SetGameState(GameManager.State.GameOver);
            });
            ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio(DeathAudio);
        }

        public void ActivateRagdoll()
        {
            Animator.enabled = false;
            Ragdoll.transform.parent = null;
            gameObject.SetActive(false);
            Ragdoll.SetActive(true);
        }

        public void ANIM_EVENT_FootstepSound()
        {
            if(Animator.GetFloat(VelocityAnimationHash) <= 1f) return;
            
            ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio("Footstep");
        }
    }
}