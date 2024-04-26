using UnityEngine.InputSystem;

namespace Project._Scripts.Runtime.Library.Controller
{
  public class InputSequence
  {
    private InputAction _inputAction;

    /// <summary>
    /// Sets the Sequence for an Input
    /// </summary>
    /// <param name="inputAction"></param>
    /// <returns></returns>
    public InputSequence Sequence(InputAction inputAction)
    {
      _inputAction = inputAction;
      return this;
    }

    /// <summary>
    /// Input Start
    /// </summary>
    /// <returns></returns>
    public bool HasInputTriggered()
    {
      return _inputAction.triggered;
    }

    /// <summary>
    /// Input End
    /// </summary>
    /// <returns></returns>
    public bool HasInputReleased()
    {
      return _inputAction.WasReleasedThisFrame();
    }

    /// <summary>
    /// Input state - "Still Running"
    /// </summary>
    /// <returns></returns>
    public bool HasInputPerformed()
    {
      return _inputAction.IsPressed();
    }
  }
}
