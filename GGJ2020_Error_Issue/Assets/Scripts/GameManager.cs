using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField] private TitlePresenter titlePresenter;
  [SerializeField] private ScorePresenter scorePresenter;
  [SerializeField] private QuestionPagePresenter questionPagePresenter;

  private GameStateReactiveProperty currentState = new GameStateReactiveProperty();

  public IReadOnlyReactiveProperty<GameState> CurrentGameState
  {
    get { return currentState; }
  }

  void Start()
  {
    CurrentGameState.Subscribe(state =>
    {
      OnStateChanged(state);
    });

    currentState.Value = GameState.Title;
    titlePresenter.GameStartBtn.OnClickAsObservable().Subscribe(_ =>
      {
        titlePresenter.ShowInGameFromTitle();
        currentState.Value = GameState.Initialize;
      }).AddTo(this);

    titlePresenter.EnterHowToPlayBtn.OnClickAsObservable()
      .Subscribe(_ => titlePresenter.ShowFirstHowToPlayFromTitle())
      .AddTo(this);

    titlePresenter.FirstNextBtn.OnClickAsObservable()
      .Subscribe(_ =>titlePresenter.ShowSecondHowToPlay())
      .AddTo(this);

    titlePresenter.FirstBackBtn.OnClickAsObservable()
      .Subscribe(_ =>
      {
        titlePresenter.ShowTitleFromFirst();
        currentState.Value = GameState.Title;
      }).AddTo(this);

    titlePresenter.SecondStartBtn.OnClickAsObservable()
        .Subscribe(_ =>
        {
          titlePresenter.ShowInGameFromSecond();
          currentState.Value = GameState.Initialize;
        }).AddTo(this);

    titlePresenter.SecondBackBtn.OnClickAsObservable()
        .Subscribe(_ => titlePresenter.ShowFirstHowToPlayFromSecond())
        .AddTo(this);

    titlePresenter.RetryBtn.OnClickAsObservable()
        .Subscribe(_ =>
        {
          titlePresenter.ShowInGameFromResult();
          currentState.Value = GameState.Initialize;
        }).AddTo(this);

    titlePresenter.BackToTitleBtn.OnClickAsObservable()
        .Subscribe(_ =>
        {
          titlePresenter.ShowTitleFromResult();
          currentState.Value = GameState.Title;
        }).AddTo(this);

      titlePresenter.InitView();
  }

  private void OnStateChanged(GameState nextState)
  {
    switch (nextState)
    {
      case GameState.Title:
        TitleState();
        break;
      case GameState.Initialize:
        InitializeState();
        break;
      case GameState.Ready:
        StartCoroutine(ReadyState());
        break;
      case GameState.InGame:
        InGameState();
        break;
      case GameState.Result:
        ResultState();
        break;
      default:
        break;
    }
  }

  private void TitleState()
  {

  }

  private void InitializeState()
  {
    questionPagePresenter.InitializeQuestionPage(1);
    scorePresenter.Initialize(questionPagePresenter.IsCorrectAnswer, this);
    currentState.Value = GameState.Ready;
  }

  private IEnumerator ReadyState()
  {
    titlePresenter.StartCountDown();
    yield return new WaitForSeconds(3.0f);
    currentState.Value = GameState.InGame;
  }

  private void InGameState()
  {
    //timeManager.Start();
    questionPagePresenter.StartGame();
  }

  private void ResultState()
  {
    //scorePresenter
    titlePresenter.ShowResult();
  }
}