using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public class CharacterFallState : CharacterBaseState
  {
    private static readonly int IsFallingAnimationHash = Animator.StringToHash("IsFalling");
    private static readonly int TimeInAirAnimationHash = Animator.StringToHash("TimeInAir");
    
    protected const float MovementMultiplierOnFall = .625f;

    private float _airTimerForLocomotion;
    private const float FallFromHighTime = .75f;

    public bool FallFromHigh { get; set; }
    public float VerticalVelocity { get; set; }
    public float TimeInAir { get; set; }
    public bool IsFalling { get; set; }
    public Vector3 FallDirection;

    private static readonly int FallFromHighAnimationHash = Animator.StringToHash("FallFromHigh");

    public CharacterFallState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory, Vector3 fallDirection, float verticalVelocity) : base(currentContext, characterStateFactory)
    {
      FallDirection = fallDirection;
      VerticalVelocity = verticalVelocity;
    }
    protected override void AccelerationConfiguration(float multiplier = 1, bool rotationSmooth = true)
    {
      Context.AccelerationConfiguration(multiplier);
    }
    protected override void RotationConfiguration(float multiplier = 1f)
    {
      Context.RotationConfiguration(multiplier);
    }
    protected override void HandleGravity(float speedMultiplier = 1)
    {
      Context.HandleGravity(FallDirection, VerticalVelocity);
    }
    public override void EnterState()
    {
      Factory.WalkState.CanRun = TimeInAir <= FallFromHighTime/2;
      TimeInAir = 0f;
      _airTimerForLocomotion = 1f;

      FallFromHigh = false;
      IsFalling = true;
    
      Context.FullBodyBipedIK.enabled = false;

      Context.Animator.SetFloat(TimeInAirAnimationHash, TimeInAir);
      Context.Animator.SetBool(IsFallingAnimationHash, IsFalling);
      Context.Animator.SetBool(FallFromHighAnimationHash, FallFromHigh);
    }
    public override void UpdateState()
    {
      if (!IsFalling) return;

      FallUpdate();
    
      LocomotionConfiguration();
    
      GroundChecker();
    }
    public override void FixedUpdateState()
    {
    
    }
    public override void LateUpdateState()
    {
    
    }
    protected override void ExitState()
    {
      Factory.WalkState.CanMove = true;
    }
    public override void CheckSwitchStates()
    {
    
    }
    public override void InitializeSubState()
    {
    
    }
    public override void InitializeState()
    {
    
    }

    protected void FallUpdate()
    {
      TimeInAir += Time.deltaTime;
      FallFromHigh = TimeInAir >= FallFromHighTime;

      if (FallDirection.x > 0) FallDirection.x -= Time.deltaTime*7.5f;
      if (FallDirection.z > 0) FallDirection.z -= Time.deltaTime*7.5f;
    
      if (FallDirection.x < 0) FallDirection.x += Time.deltaTime*7.5f;
      if (FallDirection.z < 0) FallDirection.z += Time.deltaTime*7.5f;
    
      Context.Animator.SetFloat(TimeInAirAnimationHash, TimeInAir);
      Context.Animator.SetBool(FallFromHighAnimationHash, FallFromHigh);
    }

    public bool CheckIsFallingFromPlatform()
    {
      bool isFallingFromPlatform = false;
      if(Physics.Raycast((Context.transform.position + (Context.transform.forward * -1f) + (Vector3.up * 2f)), Vector3.down, 
           out _, 4f, LayerMask.GetMask($"Platform")))
      {
        isFallingFromPlatform = Context.PreviousState == Factory.WalkState;
      }

      return isFallingFromPlatform;
    }

    protected void LocomotionConfiguration()
    {
      if (_airTimerForLocomotion > MovementMultiplierOnFall) { _airTimerForLocomotion -= Time.deltaTime/2f;}
    
      AccelerationConfiguration();
      RotationConfiguration(_airTimerForLocomotion * .25f);

      Context.MoveCharacter(Context.InputDirection, _airTimerForLocomotion);
    }

    protected void GroundChecker()
    {
      IsFalling = !Context.IsGrounded();
      Context.Animator.SetBool(IsFallingAnimationHash, IsFalling);
      
      if (!Context.IsGrounded()) return;

      Context.FullBodyBipedIK.enabled = true;

      OnLanded();
    }

    public void OnLanded()
    {
      if (!FallFromHigh)
      {
        if(TimeInAir > .75f)
          ManagerContainer.Instance.GetInstance<CameraManager>().ShakeCamera(.75f * TimeInAir, .1f * TimeInAir);
      
        Factory.WalkState.CanMove = TimeInAir < .75f;
        SwitchState(Factory.Walk());
      }
      
      else
      {
        Factory.WalkState.CanMove = false;
        ManagerContainer.Instance.GetInstance<CameraManager>().ShakeCamera(2f, .25f);
      }
    }

    public void SwitchToWalk()
    {
      SwitchState(Factory.Walk());
      Factory.WalkState.CanMove = true;
    }
  }
}
