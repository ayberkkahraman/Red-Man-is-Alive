namespace Project._Scripts.Runtime.EntitySystem.Entities
{
    public class Player : LivingEntity
    {
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
        }
    }
}