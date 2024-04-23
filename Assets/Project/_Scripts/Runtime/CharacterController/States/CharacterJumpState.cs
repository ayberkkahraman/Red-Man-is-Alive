using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using Project._Scripts.Runtime.Library.SubSystems;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public class CharacterJumpState : CharacterBaseState
  {
    private static readonly int JumpAnimationHash = Animator.StringToHash("Jump");
    private static readonly int IsLandedAnimationHash = Animator.StringToHash("IsLanded");

    public bool IsMoving;
    public bool IsLanded { get; set; }
    protected bool Jumped { get; set; }
    public bool IsInAir { get; set; }
    protected float JumpForce => Context.JumpForce;

    private float _verticalVelocity;
    private static readonly int IsInAirAnimationHash = Animator.StringToHash("IsInAir");


    private const float Gravity = -9.81f;
    private const float FallSpeedTreshold = 8.5f;

    protected float TimeInAir;

    public CharacterJumpState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory, bool isMoving) : base(currentContext, characterStateFactory)
    {
      IsMoving = isMoving;
    }

    protected override void AccelerationConfiguration(float multiplier = 1, bool rotationSmooth = true)
    {
      Context.AccelerationConfiguration(multiplier, false);
    }
    protected override void RotationConfiguration(float multiplier = 1)
    {
      Context.RotationConfiguration(multiplier);
    }
    protected override void HandleGravity(float speedMultiplier = 1)
    {
      Context.HandleGravity(Vector3.up);

      TimeInAir += Time.deltaTime;
    }
    public override void EnterState()
    {
      TimeInAir = 0f;
      
      IsLanded = false;
      Jumped = true;

      Context.Animator.SetTrigger(JumpAnimationHash);
      
      _verticalVelocity = Mathf.Sqrt(2f * JumpForce * -Gravity);
      
      Context.Animator.ResetTrigger(IsLandedAnimationHash);

      BaseBehaviour.RunAfterSeconds(.2f, () =>
      {
        Context.Animator.ResetTrigger(JumpAnimationHash);
        IsInAir = true;
        Context.Animator.SetBool(IsInAirAnimationHash, IsInAir);
      });
    }
    public override void FixedUpdateState()
    {

    }
    
    public override void UpdateState()
    {
      HandleGravity();
      AccelerationConfiguration(Context.MovementControlOnAir);
      RotationConfiguration(Context.RotationControlOnAir);
      Context.MoveCharacter(Context.InputDirection, Context.MovementControlOnAir);

      // AnyClimbable();
      
      if (!Jumped) return;
      
      if (IsLanded) return;
      
      JumpConfiguration();
    }

    public override void LateUpdateState()
    {
      
    }
    protected override void ExitState()
    {
      Jumped = false;

      IsLanded = true;
      
      BaseBehaviour.RunAfterSeconds(.05f, () => Context.Animator.SetBool(IsInAirAnimationHash, false));
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
    

    public void JumpConfiguration()
    {
      _verticalVelocity += Gravity * Time.deltaTime;
      
      JumpHandler();

      if (!IsInAir) return;

      CheckSwitchStates();
      
      if (_verticalVelocity < FallSpeedTreshold)
      {
        IsInAir = false;
        Factory.FallState.TimeInAir = TimeInAir*.625f;
        SwitchState(Factory.Fall(Vector3.up,_verticalVelocity));
      }

      if (!Context.IsGrounded())
        return;

      Context.Animator.SetTrigger(IsLandedAnimationHash);
      
      IsInAir = false;

      IsLanded = true;
      SwitchState(Factory.Walk());
    }

    public void JumpHandler()
    {
      switch ( IsMoving )
      {
        case true:
          JumpAction(Context.transform.forward);
          break;
        case false:
          JumpAction(Vector3.up);
          break;
      }
    }

    public void JumpAction(Vector3 direction)
    {
      var multiplier = IsMoving ? 1.25f + (Context.WalkingSpeed) : 1f;

      direction *= multiplier;

      direction.y = IsMoving ? _verticalVelocity + 1.5f * Context.CurrentMovementSpeed / Context.WalkingSpeed : _verticalVelocity;

      Context.CharacterController.Move(direction * Time.deltaTime);
    }
  }
}
