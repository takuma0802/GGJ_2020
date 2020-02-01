using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public enum GameState
{
  Title,
  Menu,
  Initialize,
  Ready,
  InGame,
  Result
}

[Serializable]
public class GameStateReactiveProperty : ReactiveProperty<GameState>
{
  public GameStateReactiveProperty()
  {
  }

  public GameStateReactiveProperty(GameState initialValue)
      : base(initialValue)
  {
  }
}