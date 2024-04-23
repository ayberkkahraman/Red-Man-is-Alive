using System.Collections.Generic;
using System.Linq;
using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public class InitializationState : CharacterBaseState
  {

    public InitializationState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
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
      States = new HashSet<CharacterBaseState>
      {
        Factory.Initialize(),
        Factory.Jump(),
        Factory.Idle(),
        Factory.Walk(),
        Factory.Fall(Vector3.up),
        Factory.Dead(),
      };

      States.ToList().ForEach(x => x.InitializeState());

      SwitchState(Factory.Idle());
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