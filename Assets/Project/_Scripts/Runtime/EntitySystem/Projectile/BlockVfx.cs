using UnityEngine;

namespace Project._Scripts.Runtime.Entity.Projectile
{
  public class BlockVfx : MonoBehaviour
  {
    public void ANIM_EVENT_Vanish()
    {
      gameObject.SetActive(false);
    }
  }
}
