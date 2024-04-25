using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class GameManager : MonoBehaviour
  {
    public Volume Volume;
    private Vignette _vignette;
    private PaniniProjection _paniniProjection;
    private ChromaticAberration _chromaticAberration;

    private void Start()
    {
      Volume.profile.TryGet(out _vignette);
      Volume.profile.TryGet(out _paniniProjection);
      Volume.profile.TryGet(out _chromaticAberration);
    }

    public void ActivateVignette(float targetIntensity)
    {
      DOVirtual.Float(0, targetIntensity, .5f, intensityValue =>
      {
        _vignette.intensity.value = intensityValue;
      }).SetLoops(2, LoopType.Yoyo);
    }

    public void ActivateChromaticAberration(float duration)
    {
      DOVirtual.Float(0, 1f, duration, intensityValue =>
      {
        _chromaticAberration.intensity.value = intensityValue;
      });
    }

    public void ActivatePaniniProjection(float targetIntensity)
    {
      DOVirtual.Float(0, targetIntensity, .5f, intensityValue =>
      {
        _paniniProjection.distance.value = intensityValue;
      }).SetLoops(2, LoopType.Yoyo);
    }
  }
}
