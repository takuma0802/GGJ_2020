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
using DG.Tweening;

public class TitlePresenter : BaseView
{
    private int upCount = 1500;
    private int underCount = -1500;
    private int rightCount = 1000;
    private int leftCount = -1000;
    [SerializeField] Text countDownText;

    [Header("Panel")]
    [SerializeField] RectTransform titlePanel;
    [SerializeField] RectTransform firstHowToPlayPanel;
    [SerializeField] RectTransform secondHowToPlayPanel;
    [SerializeField] RectTransform resultPanel;

    [Header("Buttone")]
    [SerializeField] Button gameStartBtn;
    [SerializeField] Button enterHowToPlayBtn;
    [SerializeField] Button firstNextBtn;
    [SerializeField] Button firstBackBtn;
    [SerializeField] Button secondStartBtn;
    [SerializeField] Button secondBackBtn;
    [SerializeField] Button retryBtn;
    [SerializeField] Button backToTileBtn;
    void Start()
    {
        BtnSubscribe();
        InitView();
        // HowToPlayPanelMove();
    }
    void InitView()
    {
        titlePanel.transform.localPosition = new Vector3(0,upCount,0);
        titlePanel.DOLocalMove (new Vector3(0,0,0),3.0f);
        firstHowToPlayPanel.transform.localPosition = new Vector3(rightCount,0,0);
        secondHowToPlayPanel.transform.localPosition = new Vector3( rightCount,0,0);
        resultPanel.transform.localPosition = new Vector3(rightCount,0,0);
        countDownText.gameObject.SetActive(false);
    }

    void BtnSubscribe()
    {
        subscriptions.Add
        (
            gameStartBtn.OnClickAsObservable()
                .Do(_ => titlePanel.DOLocalMove (new Vector3(0,underCount,0),1.0f))
                .Do(_ => StartCountDown())
                //ゲーム始まる。
                .Subscribe()
        );

        subscriptions.Add
        (
            enterHowToPlayBtn.OnClickAsObservable()
                .Do(_ => titlePanel.DOLocalMove (new Vector3(leftCount,0,0),1.0f))
                .Do(_ => firstHowToPlayPanel.DOLocalMove (new Vector3(0,0,0),1.0f))
                .Subscribe()
        );

        subscriptions.Add
        (
            firstNextBtn.OnClickAsObservable()
                .Do(_ => firstHowToPlayPanel.DOLocalMove (new Vector3(leftCount,0,0),1.0f))
                .Do(_ => secondHowToPlayPanel.DOLocalMove (new Vector3(0,0,0),1.0f))
                .Subscribe()
        );
        subscriptions.Add
        (
            firstBackBtn.OnClickAsObservable()
                .Do(_ => titlePanel.DOLocalMove (new Vector3(0,0,0),1.0f))
                .Do(_ => firstHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Subscribe()
        );

        subscriptions.Add
        (
            secondStartBtn.OnClickAsObservable()
                .Do(_ => secondHowToPlayPanel.DOLocalMove (new Vector3(leftCount,0,0),1.0f))
                .Do(_ => StartCountDown())
                //インゲームスタート
                .Subscribe()
        );
        subscriptions.Add
        (
            secondBackBtn.OnClickAsObservable()
                .Do(_ => firstHowToPlayPanel.DOLocalMove (new Vector3(0,0,0),1.0f))
                .Do(_ => secondHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Subscribe()
        );

        subscriptions.Add
        (
            retryBtn.OnClickAsObservable()
                .Do(_ => StartCountDown())
                .Do(_ => titlePanel.DOLocalMove (new Vector3(0,upCount,0),1.0f))
                .Do(_ => firstHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Do(_ => secondHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Do(_ => resultPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Subscribe()
        );

        subscriptions.Add
        (
            backToTileBtn.OnClickAsObservable()
                .Do(_ => titlePanel.DOLocalMove (new Vector3(0,0,0),1.0f))
                .Do(_ => firstHowToPlayPanel.DOLocalMove (new Vector3(0,upCount,0),1.0f))
                .Do(_ => secondHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Do(_ => resultPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f))
                .Subscribe()
        );
    }
    void StartCountDown()
    {
        subscriptions.Add
        (
            Observable.Timer(TimeSpan.FromMilliseconds(0f))
                .Do(_ => countDownText.gameObject.SetActive(true))
                .Do(_ => countDownText.text = "3")
                .Delay(TimeSpan.FromMilliseconds(1000))
                .Do(_ => countDownText.text = "2")
                .Delay(TimeSpan.FromMilliseconds(1000))
                .Do(_ => countDownText.text = "1")
                .Delay(TimeSpan.FromMilliseconds(1000))
                .Do(_ => countDownText.text = "Start!!!")
                .Delay(TimeSpan.FromMilliseconds(1000))
                .Do(_ => countDownText.gameObject.SetActive(false))
                .Subscribe()
        );
    }
}
