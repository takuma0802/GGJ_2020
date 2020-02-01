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
    private int allScore;
    public int addScoreValue, substractScoreValue;
    public Text scoreTestText;
    public Button addScoreBtn, subtractScoreBtn;
    void Start()
    {

        AddScoreSubscrive();

        SubtractScoreSubscribe();


    }
    public void AddScoreSubscrive()
    {
        subscriptions.Add
        (
            Observable.Timer(TimeSpan.FromMilliseconds(0))
                .Do(_ => allScore += addScoreValue)
                .Do(_ => scoreTestText.text = allScore.ToString())
                .Subscribe()
        );
    }
    public void  SubtractScoreSubscribe()
    {
        subscriptions.Add
        (
            Observable.Timer(TimeSpan.FromMilliseconds(0))
                .Do(_ => allScore -= substractScoreValue)
                .Do(_ => scoreTestText.text = allScore.ToString())
                .Subscribe()
        );
    }
}
