using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using UniRx.Triggers;
using System.Linq;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Text TimeText;
    [SerializeField] private int defaultLimitTime;

    private ReactiveProperty<int> limitTime = new IntReactiveProperty();
    public IReadOnlyReactiveProperty<int> LimitTime { get {return limitTime; } }

    private IConnectableObservable<int> countDownObservable;
    public IObservable<int> CountDownObservable{ get { return countDownObservable.AsObservable(); } }


    public void Initialize()
    {
        limitTime.Value = defaultLimitTime;
        countDownObservable = CreateCountDownObservable(limitTime.Value).Publish();
    }

    public void StartTimer()
    {
        countDownObservable.Connect();

        CountDownObservable.Subscribe(time =>
        {
            SetTime(time);
        },
        () =>
        {
            SetTime(0);
        });

        CountDownObservable.First(time => time <= 10)
            .Subscribe(_ => TimeText.color = Color.red);
    }

    private IObservable<int> CreateCountDownObservable(int LimitTime)
    {
        return Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Select(x => (int)(LimitTime - x))
            .TakeWhile(x => x > 0);
    }

    public void SetTime(int time)
    {
        TimeText.text = time.ToString();
    }
}
