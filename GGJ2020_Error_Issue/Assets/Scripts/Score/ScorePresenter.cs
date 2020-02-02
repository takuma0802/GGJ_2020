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
    public Text scoreText, comboText;

    public void Initialize(IObservable<AnswerResults> answerResult, GameManager manager)
    {
        scoreText.text = "0";
        AllScore.Value = 0;
        ScoreSubscribe(answerResult,manager);

        AllScore
            .Where(_ => manager.CurrentGameState.Value == GameState.InGame)
            .Subscribe(score => AddScore(score))
            .AddTo(this);

        answerResult.Where(_ => manager.CurrentGameState.Value == GameState.InGame)
            .Subscribe(result => ChangeCombo(result.CurrentCombo))
            .AddTo(this);
    }

    public void ScoreSubscribe(IObservable<AnswerResults> answerObserbable, GameManager manager)
    {
        answerObserbable.Subscribe(result =>
        {
            Debug.Log($"correct:{result.IsCorrect}, combo:{result.CurrentCombo}, time:{result.TakenAnswerTime}");
            var basePoint = 0;
            float comboBonus = 0;
            float speedBonus = 0;
            switch(result.IsCorrect)
            {
                case AnswerResult.SelectCorrectAnswer:
                    basePoint = CorrectAnsValue;
                    break;
                case AnswerResult.Mistake:
                    basePoint = MistakeValue;
                    break;
                case AnswerResult.CorrectAllAnswer:
                    basePoint = CorrectAllAnsValue;
                    break;
                case AnswerResult.NotEnoughAnswer:
                    basePoint = NotEnoughValue;
                    break;
            }

            comboBonus = (float)result.CurrentCombo * 0.1f + 1;
            if(result.TakenAnswerTime < 0)
            {
                speedBonus = 1;
            }else
            {
                speedBonus = 4 - (int)Math.Ceiling(result.TakenAnswerTime);
                speedBonus = Mathf.Max(1,speedBonus);
            }

            var currentScore = basePoint * comboBonus * speedBonus;
            AllScore.Value += (int)currentScore;
            Debug.Log($"{basePoint},{comboBonus},{speedBonus}");
        });
    }

    private void AddScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void ChangeCombo(int combo)
    {
        //comboText.text = combo.ToString();
    }
}
