using System;
using System.Collections.Generic;
using Project._Scripts.Runtime.Managers.Manager;
using UnityEngine;
using UnityEngine.Events;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class CheckpointManager : MonoBehaviour
  {
    public Transform CurrentCheckpoint { get; set; }

    public delegate void UpdateCheckpoint(Transform checkpoint);
    public UpdateCheckpoint UpdateCheckpointHandler;

    public UnityEvent<Transform> InitializeCheckpointEvent;
    public List<Transform> CheckpointZones;
    
    public void OnEnable()
    {
      UpdateCheckpointHandler += UpdateCurrentCheckpoint;
    }

    public void OnDisable()
    {
      UpdateCheckpointHandler -= UpdateCurrentCheckpoint;
    }

    public void Start()
    {
      if (PlayerPrefs.HasKey("Checkpoint"))
      {
        StartFromCheckpoint();
      }
    }

    public void UpdateCurrentCheckpoint(Transform checkpoint)
    {
      CurrentCheckpoint = checkpoint;
      SaveManager.SaveData("Checkpoint", CurrentCheckpoint.position);
    }

    public void StartFromCheckpoint()
    {
      Vector3 position = SaveManager.LoadData<Vector3>("Checkpoint", Vector3.zero);
      UpdateCheckpointHandler(CheckpointZones.Find(x => x.position == position));
      
      InitializeCheckpointEvent?.Invoke(CurrentCheckpoint);
    }

    public static void SaveForCheckpoint<T>(Action action, string dataName, T data)
    {
      if (!PlayerPrefs.HasKey("Checkpoint"))
      {
        SaveManager.SaveData(dataName, data);
        return;
      }
      
      action?.Invoke();
    }
    
    public static void LoadForCheckpoint(Action action)
    {
      if(!PlayerPrefs.HasKey("Checkpoint")) return;
      
      action?.Invoke();
    }

  }
}
