using _Scripts.Runtime.Entity.CharacterController.StateFactory;
using Cinemachine;
using Project._Scripts.Runtime.Library.Controller;
using Project._Scripts.Runtime.Managers.Manager;
using Project._Scripts.Runtime.Managers.ManagerClasses;
using UnityEngine;

namespace _Scripts.Runtime.Entity.CharacterController.States.BaseStates
{
  public sealed class CharacterWalkState : CharacterBaseState
  {
    public CinemachineVirtualCamera CharacterCamera { get; set; }
    
    public CharacterWalkState(CharacterStateMachine currentContext, CharacterStateFactory characterStateFactory) : base(currentContext, characterStateFactory)
    {
      
    }

    public bool CanMove { get; set; }
    public bool CanRun { get; set; }

    public bool IsRunning { get; set; }
    private float SpeedMultiplier => IsRunning ? Context.RunningSpeed / Context.WalkingSpeed : 1f;


    public override void EnterState()
    {
      CanRun = true;

      CharacterCamera ??= ManagerContainer.Instance.GetInstance<CameraManager>().CharacterCamera;
    }
    public override void FixedUpdateState()
    {
      
    }
    public override void UpdateState()
    {
      CheckIsRunning();

      AccelerationConfiguration(SpeedMultiplier);

      RotationConfiguration(1 / SpeedMultiplier);

      Movement(Context.InputDirection, SpeedMultiplier);

      CheckSwitchStates();

      CheckSwitchSubStates();
    }
    public override void LateUpdateState()
    {
      
    }
    protected override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
      if (Context.CurrentMovementSpeed <= Context.MinSpeedTreshold)
      {
        SwitchState(Factory.Idle());
      }

      if (Context.IsFalling()) SwitchState(Factory.Fall(Vector3.up));

    }
    public override void InitializeSubState()
    {
      
    }

    public void CheckIsRunning()
    {
      IsRunning = CanRun && InputController.Run().HasInputPerformed();
    }

    private void CheckSwitchSubStates()
    {
      if (!InputController.Jump().HasInputTriggered())
        return;
      
      if (!Context.ReadyForJump()) return;
      
      bool isMoving = Context.InputDirection != Vector3.zero;

      bool jumpState = isMoving ? Context.CurrentMovementSpeed >= .25f : Context.CurrentMovementSpeed >= 2f;

      SwitchState(Factory.Jump(jumpState));
    }
    public override void InitializeState()
    {
      InitializeSubState();
    }

   #region Movement
    protected override void AccelerationConfiguration(float multiplier = 1f, bool rotationSmooth = true)
    {
      Context.AccelerationConfiguration(multiplier);
    }
    protected override void RotationConfiguration(float multiplier = 1)
    {
      if (!CanMove) return;

      Context.RotationConfiguration(multiplier);
    }
    protected override void HandleGravity(float speedMultiplier = 1)
    {
      Context.HandleGravity(Vector3.up,speedMultiplier);
    }

    /// <summary>
    /// Handles the movement
    /// </summary>
    private void Movement(Vector3 direction, float multiplier)
    {
      if (!CanMove) return;
      
      Context.MoveCharacter(direction, multiplier);
    }
    #endregion
  }
}
