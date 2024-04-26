using Project._Scripts.Runtime.Managers.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
  public class GameManager : MonoBehaviour
  {
    public enum State{Running, GameOver, Success}
    private State _gameState;

    public UnityEvent OnGameSuccess;
    public UnityEvent OnGameOver;

    /// <summary>
    /// Initializing the Game State on the Awake
    /// </summary>
    private void Awake()
    {
      SetGameState(State.Running);
    }
    /// <summary>
    /// Sets the Game State
    /// </summary>
    /// <param name="state"></param>
    public void SetGameState(State state)
    {
      _gameState = state;

      switch ( _gameState )
      {
        case State.Success:
          OnGameSuccess?.Invoke();
          break;
        case State.GameOver:
          OnGameOver?.Invoke();
          break;
      }

      //Restart stage after game ends
      if (state != State.Running)
      {
        ManagerContainer.Instance.GetInstance<StageManager>().LoadSceneAtIndex(SceneManager.GetActiveScene().buildIndex);
      }
    }

    //------------------------------------------------------------------------
    //These sections in below will be used in the UnityEvent on the Inspector
    //------------------------------------------------------------------------
    public void UNITY_EVENT_GameSuccess()
    {
      SetGameState(State.Success);
    }
    
    public void UNITY_EVENT_GameOver()
    {
      SetGameState(State.GameOver);
    }
    //------------------------------------------------------------------------
  }
}
