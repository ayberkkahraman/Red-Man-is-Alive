using Project._Scripts.Runtime.Entity.EntitySystem.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Project._Scripts.Runtime.Entity.EntitySystem.HealthBar
{
    public class HealthBarBase : MonoBehaviour
    {
        public Image Bar;
        public Unit Unit { get; set; }

        protected virtual void Start()
        {
            Unit = GetComponentInParent<Unit>();
            Unit.OnTakeDamageHandler += UpdateHealthBar;
        }

        protected virtual  void LateUpdate()
        {
            transform.localRotation = transform.parent.transform.rotation;
        }

        public virtual  void UpdateHealthBar(int health)
        {
            Bar.fillAmount = Unit.Health / Unit.MaxHealth;
            if(Unit.Health <= 0) gameObject.SetActive(false);
        }
    }
}