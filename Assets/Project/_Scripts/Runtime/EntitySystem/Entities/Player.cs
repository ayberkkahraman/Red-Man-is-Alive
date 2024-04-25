using UnityEngine;

namespace Project._Scripts.Runtime.EntitySystem.Entities
{
    public class Player : LivingEntity
    {
        public GameObject Ragdoll;
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

        public void ActivateRagdoll()
        {
            Animator.enabled = false;
            Ragdoll.transform.parent = null;
            gameObject.SetActive(false);
            Ragdoll.SetActive(true);
        }
    }
}