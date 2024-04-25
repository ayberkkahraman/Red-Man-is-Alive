using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project._Scripts.Runtime.InGame.Dynamics._3DText
{
  public class Text3D : MonoBehaviour
  {
    public List<Transform> LetterTransforms;

    [Serializable]
    public struct Letter
    {
      public string LetterName;
      public GameObject LetterModel;
    }

    public List<Letter> Letters;
    public void Start()
    {
      for (int i = 0; i < LetterTransforms.Count; i++)
      {
        Transform letter = LetterTransforms[i];

        Vector3 targetPosition = letter.transform.position + Vector3.up * .125f;

        letter.DOMove(targetPosition, .25f)
          .SetDelay(i * .125f)
          .SetEase(Ease.InOutSine)
          .SetLoops(-2, LoopType.Yoyo);
      }
    }

    public void SpawnTextInPosition(string textContent, Vector3 position, float letterOffset = .5f)
    {
      Transform targetTransform = Instantiate(
        new GameObject(textContent), 
        transform.position + Vector3.right * letterOffset * (textContent.Length/2), 
        Quaternion.identity, 
        transform).transform;
      
      for (int i = 0; i < textContent.Length; i++)
      {
        char letter = char.ToUpper(textContent[i]);
        Letter letter3D = Letters.Find(x => x.LetterName == letter.ToString());
        
        Instantiate(letter3D.LetterModel, 
          position + Vector3.right * letterOffset * i, 
          letter3D.LetterModel.transform.rotation, 
          targetTransform);
      }
    }
  }
}
