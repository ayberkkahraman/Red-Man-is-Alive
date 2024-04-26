using UnityEngine;

namespace Project._Scripts.Runtime.Interfaces
{
  public interface IInteractable
  {
    public void OnTrigger(Collider triggeredCollider);
    public delegate void OnTriggered(Collider other);
    public OnTriggered OnTriggeredHandler { get; set; }
  }
}
