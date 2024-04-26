using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using Project._Scripts.Runtime.Library.Controller;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public class CharacterIdleState : CharacterBaseState
  {
    public CharacterIdleState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory)
      : base(currentContext, characterStateFactory){}

    protected override void AccelerationConfiguration(float multiplier = 1f, bool rotationSmooth = true)
    {
      Context.AccelerationConfiguration();
      
      var currentPosition = Context.transform.position;
      currentPosition.z = 0f;
      Context.transform.position = currentPosition;
    }
    protected override void RotationConfiguration(float multiplier = 1f)
    {
      
    }
    protected override void HandleGravity(float speedMultiplier = 1)
    {
      Context.HandleGravity(Vector3.up,speedMultiplier);
    }
    public override void EnterState()
    {
    
    }
    public override void FixedUpdateState()
    {
      
    }
    public override void UpdateState()
    {
      AccelerationConfiguration();
      CheckSwitchStates();
    }
    public override void LateUpdateState()
    {
      
    }
    protected override void ExitState()
    {

    }
    public override void CheckSwitchStates()
    {
      if (Context.CurrentMovementSpeed > Context.MinSpeedTreshold)
      {
        SwitchState(Factory.Walk());
        Factory.WalkState.CanMove = true;
      }

      if (InputController.Jump().HasInputTriggered())
      {
        if (!Context.ReadyForJump()) return;
        
        SwitchState(Factory.Jump());
      }
    }
    public override void InitializeSubState()
    {
    
    }
    public override void InitializeState()
    {
      InitializeSubState();
    }
  }
}
