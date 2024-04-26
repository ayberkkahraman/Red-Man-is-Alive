using DG.Tweening;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.Runtime.InGame.Dynamics.Traps
{
  public class Swiper : TrapBase
  {
    [Range(1f, 10f)][SerializeField] private float RotateSpeed = 1f;
    [Range(1f, 100f)][SerializeField] private float Force = 25f;
    
    public enum Axis{LeftToRight, RightToLeft}
    public Axis RotateAxis;
    protected override void OnTrigger(Collider triggeredCollider)
    {
      TargetAnimator.applyRootMotion = false;
      ForceImpact(transform.right * RotateSpeed);

      KillThePlayer(triggeredCollider);
      
      ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio("Force");
    }

    private void Start()
    {
      Rotate();
    }

    private void Rotate()
    {
      transform.DORotate(Vector3.up * -36, .5f / RotateSpeed)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);
    }
    
    public void ForceImpact(Vector3 direction)
    {
      TargetRigidbody.AddForce(direction * Force * -20f, ForceMode.Impulse);
    }
  }
}
