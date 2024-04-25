using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public class CharacterDeadState : CharacterBaseState
  {

    public CharacterDeadState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
    {
    }
    protected override void AccelerationConfiguration(float multiplier = 1, bool rotationSmooth = true)
    {
      
    }
    protected override void RotationConfiguration(float multiplier = 1)
    {
      
    }
    protected override void HandleGravity(float speedMultiplier = 1)
    {
      
    }
    public override void EnterState()
    {
      Context.gameObject.layer = LayerMask.NameToLayer($"Default");
    }
    public override void FixedUpdateState()
    {
      
    }
    public override void UpdateState()
    {
      
    }
    public override void LateUpdateState()
    {
      
    }
    protected override void ExitState()
    {
      
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
  }
}
