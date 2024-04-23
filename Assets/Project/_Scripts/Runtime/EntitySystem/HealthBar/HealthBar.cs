using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project._Scripts.Runtime.Entity.EntitySystem.HealthBar
{
  public class HealthBar : HealthBarBase
  {
    public Image ReducerBarFiller;
    [Range(.1f, 5f)]public float ReduceSpeed = .2f;
    private static readonly int Close = Animator.StringToHash("Close");

    private float _timer = 1f;
    [Range(.25f, 2f)]public float DefaultTimer = 1f;

    private bool _activated;
    public bool HasReducer;
    public bool Destroyable;

    protected override void Start()
    {
      base.Start();
      
      DefaultTimer = _timer;
    }

    protected void Update()
    {
      if(!HasReducer) return;
      
      if(!_activated) return;
      
      if (_timer < DefaultTimer) _timer += Time.deltaTime;
      else
      {
        UpdateReducerBar();
        _activated = false;
      }
    }
    public override void UpdateHealthBar(int damage)
    {
      _activated = true;
      _timer = 0f;
      
      Bar.fillAmount = Unit.Health / Unit.MaxHealth;

      if (!(Unit.Health <= 0))
        return;
      
      Invoke(nameof(CloseHealthBar), 1f);
      if (Destroyable) gameObject.SetActive(false);
    }

    public void CloseHealthBar()
    {
      if(GetComponent<Animator>() != null)GetComponent<Animator>()?.SetTrigger(Close);
    }

    public void UpdateReducerBar()
    {
      StopCoroutine(LoadLevelWithSlider());
      StartCoroutine(LoadLevelWithSlider());
    }
    
    private IEnumerator LoadLevelWithSlider()
    {
      float sliderValue = ReducerBarFiller.fillAmount;

      while (sliderValue >= Bar.fillAmount)
      {
        sliderValue -= Time.deltaTime * ReduceSpeed;
        
        ReducerBarFiller.fillAmount = sliderValue;

        yield return null;
      }
      _activated = false;
    }
  }
}
