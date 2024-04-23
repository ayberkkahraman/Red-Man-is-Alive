using System;
using Project._Scripts.Runtime.Entity.EntitySystem;
using Project._Scripts.Runtime.Entity.EntitySystem.Entities;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.Entity.Projectile
{
  public abstract class ProjectileBase : MonoBehaviour
  {
    private Rigidbody2D _rb2D;
    public Unit Owner { get; set; }
    public int Damage { get; set; }
    public float ProjectileSpeed;
    private int _directionMultiplier;

    private void Start()
    {
      _rb2D = GetComponent<Rigidbody2D>();

      _directionMultiplier = transform.parent.transform.rotation.y == 0 ? 1 : -1;

      UnchainParent();
    }

    public void UnchainParent()
    {
      transform.parent = null;
    }

    public void FixedUpdate()
    {
      _rb2D.AddForce(Vector2.right * (ProjectileSpeed * _directionMultiplier), ForceMode2D.Impulse);
    }

    public void DestroyObject()
    {
      Destroy(gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
      if(other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

      Player entity = other.gameObject.GetComponent<Player>();
      Owner.OnAttackHandler(entity, true);
      // entity.OnTakeDamageHandler?.Invoke(Damage);
      entity.GetComponent<Rigidbody2D>().AddForce(transform.right * Owner.UnitData.ForceStrength, ForceMode2D.Impulse);
      
      DestroyObject();   
      var camera = ManagerContainer.Instance.GetInstance<CameraManager>();
      
      camera.ShakeCamera(camera.ShakeIntensity * 2, camera.ShakeDuration);
    }
  }
}
