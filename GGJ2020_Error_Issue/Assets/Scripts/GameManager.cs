using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public GameStateReactiveProperty CurrentState
      = new GameStateReactiveProperty(GameState.Initialize);

  public IReadOnlyReactiveProperty<GameState> CurrentGameState
  {
    get { return CurrentState; }
  }

  void Start()
  {
    CurrentState.Subscribe(state =>
    {
      OnStateChanged(state);
    });

  }

  private void OnStateChanged(GameState nextState)
  {
    switch (nextState)
    {
      case GameState.Title:
        break;
      case GameState.Initialize:
        break;
      case GameState.Ready:
        break;
      case GameState.InGame:
        break;
      case GameState.Result:
        break;
      default:
        break;
    }
  }
}