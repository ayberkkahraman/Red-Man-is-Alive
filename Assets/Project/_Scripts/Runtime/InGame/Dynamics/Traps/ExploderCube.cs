using System.Linq;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.InGame.Dynamics.Traps
{
  public class ExploderCube : TrapBase
  {
    [Range(1f, 100f)] [SerializeField] private float ExplosionForce = 10f;
    [Range(.5f, 10f)] [SerializeField] private float ExplosionRadius = 3f;
    protected override void OnTrigger(Collider triggeredCollider)
    {
      Rigidbody cubeRigidbody = GetComponent<Rigidbody>();
      
      cubeRigidbody.AddExplosionForce(ExplosionForce * 50f * cubeRigidbody.mass,transform.position, ExplosionRadius);

      ManagerContainer.Instance.GetInstance<CameraManager>().ShakeCamera(ExplosionForce*2f, .3f, .08f);
      
      if (ExplosionForce >= 5f && Physics.OverlapSphere(transform.position, ExplosionRadius).ToList().Exists(x => x == triggeredCollider))
      {
        KillThePlayer(triggeredCollider);
      }
      
      AudioManager.Instance.PlayAudio("Explode");
      Destroy(this);
    }
  }
}
