using UnityEngine.InputSystem;

namespace Project._Scripts.Runtime.Library.Controller
{
  public class InputSequence
  {
    private InputAction _inputAction;

    public InputSequence Sequence(InputAction inputAction)
    {
      _inputAction = inputAction;
      return this;
    }

    public bool HasInputTriggered()
    {
      return _inputAction.triggered;
    }

    public bool HasInputReleased()
    {
      return _inputAction.WasReleasedThisFrame();
    }

    public bool HasInputPerformed()
    {
      return _inputAction.IsPressed();
    }
  }
}
