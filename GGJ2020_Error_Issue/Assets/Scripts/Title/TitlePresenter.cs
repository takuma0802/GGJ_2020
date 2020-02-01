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
    private int upCount = 2600;
    private int underCount = -2600;
    private int rightCount = 1400;
    private int leftCount = -1400;

    //[SerializeField] private RectTransform upCount, underCount, rightCount, leftCount ;
    [SerializeField] Text countDownText;

    [Header("Panel")]
    [SerializeField] RectTransform titlePanel;
    [SerializeField] RectTransform firstHowToPlayPanel;
    [SerializeField] RectTransform secondHowToPlayPanel;
    [SerializeField] RectTransform resultPanel;
    [SerializeField] RectTransform inGamePanel;

    [Header("Button")]
    public Button GameStartBtn;
    public Button EnterHowToPlayBtn;
    public Button FirstNextBtn;
    public Button FirstBackBtn;
    public Button SecondStartBtn;
    public Button SecondBackBtn;
    public Button RetryBtn;
    public Button BackToTitleBtn;

    public void InitView()
    {

        titlePanel.transform.localPosition = new Vector3(0,upCount,0);
        titlePanel.DOLocalMove (new Vector3(0,0,0),3.0f);

        firstHowToPlayPanel.transform.localPosition = new Vector3(rightCount,0,0);
        secondHowToPlayPanel.transform.localPosition = new Vector3( rightCount,0,0);
        resultPanel.transform.localPosition = new Vector3(0,upCount,0);
        inGamePanel.transform.localPosition = new Vector3(0,upCount,0);

        countDownText.gameObject.SetActive(false);
        GameStartBtn.interactable = true;
        EnterHowToPlayBtn.interactable = true;
    }

    public void ShowTitleFromResult()
    {
        titlePanel.transform.localPosition = new Vector3(0,upCount,0);
        titlePanel.DOLocalMove (new Vector3(0,0,0),1.0f);
        resultPanel.DOLocalMove (new Vector3(0,underCount,0),1.0f);

        GameStartBtn.interactable = true;
        EnterHowToPlayBtn.interactable = true;
        RetryBtn.interactable = false;
        BackToTitleBtn.interactable = false;
    }

    public void ShowTitleFromFirst()
    {
        titlePanel.transform.localPosition = new Vector3(leftCount,0,0);
        titlePanel.DOLocalMove (new Vector3(0,0,0),1.0f);
        firstHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f);

        GameStartBtn.interactable = true;
        EnterHowToPlayBtn.interactable = true;
        FirstBackBtn.interactable = false;
        FirstNextBtn.interactable = false;
    }

    public void ShowFirstHowToPlayFromTitle()
    {
        titlePanel.DOLocalMove (new Vector3(leftCount,0,0),1.0f);
        firstHowToPlayPanel.transform.localPosition = new Vector3(rightCount,0,0);
        firstHowToPlayPanel.DOLocalMove (new Vector3(0,0,0),1.0f);

        GameStartBtn.interactable = false;
        EnterHowToPlayBtn.interactable = false;
        FirstBackBtn.interactable = true;
        FirstNextBtn.interactable = true;
    }

    public void ShowFirstHowToPlayFromSecond()
    {
        secondHowToPlayPanel.DOLocalMove (new Vector3(rightCount,0,0),1.0f);
        firstHowToPlayPanel.transform.localPosition = new Vector3(leftCount,0,0);
        firstHowToPlayPanel.DOLocalMove (new Vector3(0,0,0),1.0f);

        SecondBackBtn.interactable = false;
        SecondStartBtn.interactable = false;
        FirstBackBtn.interactable = true;
        FirstNextBtn.interactable = true;
    }

    public void ShowSecondHowToPlay()
    {
        firstHowToPlayPanel.DOLocalMove (new Vector3(leftCount,0,0),1.0f);
        secondHowToPlayPanel.transform.localPosition = new Vector3(rightCount,0,0);
        secondHowToPlayPanel.DOLocalMove (new Vector3(0,0,0),1.0f);

        SecondBackBtn.interactable = true;
        SecondStartBtn.interactable = true;
        FirstBackBtn.interactable = false;
        FirstNextBtn.interactable = false;
    }

    public void ShowInGameFromTitle()
    {
        inGamePanel.transform.localPosition = new Vector3(0,upCount,0);
        inGamePanel.DOLocalMove (new Vector3(0,0,0),1.0f);
        titlePanel.DOLocalMove (new Vector3(0,underCount,0),1.0f);

        GameStartBtn.interactable = false;
        EnterHowToPlayBtn.interactable = false;
    }

    public void ShowInGameFromSecond()
    {
        inGamePanel.transform.localPosition = new Vector3(0,upCount,0);
        inGamePanel.DOLocalMove (new Vector3(0,0,0),1.0f);
        secondHowToPlayPanel.DOLocalMove (new Vector3(0,underCount,0),1.0f);

        SecondBackBtn.interactable = false;
        SecondStartBtn.interactable = false;
    }

    public void ShowInGameFromResult()
    {
        inGamePanel.transform.localPosition = new Vector3(0,upCount,0);
        inGamePanel.DOLocalMove (new Vector3(0,0,0),1.0f);
        resultPanel.DOLocalMove (new Vector3(0,underCount,0),1.0f);

        RetryBtn.interactable = false;
        BackToTitleBtn.interactable = false;
    }

    public void ShowResult()
    {
        resultPanel.transform.localPosition = new Vector3(0,upCount,0);
        resultPanel.DOLocalMove (new Vector3(0,0,0),1.0f);
        inGamePanel.DOLocalMove (new Vector3(0,underCount,0),1.0f);

        RetryBtn.interactable = true;
        BackToTitleBtn.interactable = true;
    }

    public void StartCountDown()
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
