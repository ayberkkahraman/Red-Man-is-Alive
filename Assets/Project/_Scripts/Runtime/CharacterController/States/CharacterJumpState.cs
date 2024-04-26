using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public class CharacterJumpState : CharacterBaseState
  {
    private static readonly int JumpAnimationHash = Animator.StringToHash("Jump");
    private static readonly int IsInAirAnimationHash = Animator.StringToHash("IsInAir");

    public bool IsMoving{ get; set; }
    public bool IsInAir { get; set; }
    
    protected float TimeInAir{ get; set; }
    protected float JumpForce => Context.JumpForce;

    private float _verticalVelocity;
    
    private const float Gravity = -9.81f;
    private const float FallSpeedTreshold = 8.5f;

    

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

      Context.Animator.SetTrigger(JumpAnimationHash);
      
      _verticalVelocity = Mathf.Sqrt(2f * JumpForce * -Gravity);
      IsInAir = true;
      Context.Animator.SetBool(IsInAirAnimationHash, IsInAir);
      
      ManagerContainer.Instance.GetInstance<AudioManager>().PlayAudio("Jump");
    }
    public override void FixedUpdateState()
    {

    }
    
    public override void UpdateState()
    {
      HandleGravity();
      AccelerationConfiguration();
      RotationConfiguration(Context.RotationControlOnAir);
      Context.MoveCharacter(Context.InputDirection, Context.MovementControlOnAir);
      CheckSwitchStates();
      JumpConfiguration();
    }

    public override void LateUpdateState()
    {
      
    }
    protected override void ExitState()
    {
      IsInAir = false;
      
      Context.Animator.SetBool(IsInAirAnimationHash, false);
      ManagerContainer.Instance.RunAfterSeconds(.02f, () => Context.Animator.SetBool(IsInAirAnimationHash, false));
    }
    public override void CheckSwitchStates()
    {
      if(TimeInAir <= .2f) return;
      
      if(Context.IsGrounded()){SwitchState(Factory.Walk());}
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

      if (!(_verticalVelocity < FallSpeedTreshold))
        return;
      
      IsInAir = false;
    }

    public void  JumpHandler()
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
      var multiplier = IsMoving ? 1.25f + Context.WalkingSpeed : 1f;

      direction *= multiplier;

      direction.y = IsMoving ? _verticalVelocity + 1.5f * Context.CurrentMovementSpeed / Context.WalkingSpeed : _verticalVelocity;

      Context.CharacterController.Move(direction * Time.deltaTime);
    }
  }
}
