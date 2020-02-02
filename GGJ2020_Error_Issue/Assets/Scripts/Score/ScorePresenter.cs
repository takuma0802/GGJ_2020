using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UniRx;
using System;
using UniRx.Triggers;

using System.Linq;
using UnityEngine.EventSystems;

using UnityEngine.UI;


public class ScorePresenter : BaseView
{
    public ReactiveProperty<int> AllScore = new IntReactiveProperty(0);
    public int CorrectAnsValue, CorrectAllAnsValue, MistakeValue, NotEnoughValue;
    public Text scoreText;

    public void Initialize(IObservable<AnswerResult> answerObserbable, GameManager manager)
    {
        scoreText.text = "0";
        AllScore.Value = 0;
        ScoreSubscribe(answerObserbable,manager);
    }

    public void ScoreSubscribe(IObservable<AnswerResult> answerObserbable, GameManager manager)
    {
        answerObserbable.Subscribe(result =>
        {
            switch(result)
            {
                case AnswerResult.SelectCorrectAnswer:
                    AllScore.Value += CorrectAnsValue;
                    break;
                case AnswerResult.Mistake:
                    AllScore.Value += MistakeValue;
                    break;
                case AnswerResult.CorrectAllAnswer:
                    AllScore.Value += CorrectAllAnsValue;;
                    break;
                case AnswerResult.NotEnoughAnswer:
                    AllScore.Value += NotEnoughValue;
                    break;
            }
        });

        AllScore
            .Where(_ => manager.CurrentGameState.Value == GameState.InGame)
            .Subscribe(score => AddScore(score))
            .AddTo(this);
    }

    private void AddScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
