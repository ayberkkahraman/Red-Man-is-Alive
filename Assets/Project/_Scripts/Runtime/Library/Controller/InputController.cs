namespace Project._Scripts.Runtime.Library.Controller
{
  public static class InputController
  {
    #region Controller Input Configuration
    public static Controller ControllerInput;
    public static InputSequence Input;
    
    public static void CreateControllerInput()
    {
      ControllerInput ??= new Controller();
    }
    public static void InitializeControllerInput()
    {
      ControllerInput.CharacterControls.Enable();
      ControllerInput.CameraController.Enable();
      ControllerInput.InteractionController.Enable();
      ControllerInput.CombatController.Enable();

      Input = new InputSequence();
    }

    public static void DeInitializeControllerInput()
    {
      ControllerInput.CharacterControls.Disable();
      ControllerInput.CameraController.Disable();
      ControllerInput.InteractionController.Disable();
      ControllerInput.CombatController.Disable();
    }
    #endregion
    
    /// <summary>
    /// This script will be used as the static-input generic extension class
    /// </summary>
    /// <returns></returns>
    
    public static InputSequence Jump() => Input.Sequence(ControllerInput.CharacterControls.Jump);
    public static InputSequence Run() => Input.Sequence(ControllerInput.CharacterControls.Run);

  }
}
