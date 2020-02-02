using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

public class QuestionPagePresenter : MonoBehaviour
{
    [SerializeField] private GameObject questionViewPrefab;
    [SerializeField] private GameObject questionParentObject;
    [SerializeField] private AnswerView answerView;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private ShakeView shakeView;

    private List<QuestionView> questionViews = new List<QuestionView>();
    private List<QuestionMaster> questionMasters;
    private Answer selectableAnswer;
    public List<IDisposable> subscriptions = new List<IDisposable>();
    private ReactiveProperty<int> currentRow = new IntReactiveProperty(1);
    private ReactiveProperty<bool> canAnswer = new BoolReactiveProperty(false);
    private IDisposable takenAnswerTimeDisposable;
    private int correctNumInCurrentRow;
    private List<string> answeredTextsInCurrentRow = new List<string>();
    private System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    private Subject<AnswerResults> currentAnswerResult = new Subject<AnswerResults>();
    public IObservable<AnswerResults> CurrentAnswerResult
    {
        get { return currentAnswerResult;}
    }

    private ReactiveProperty<int> currentCombo = new IntReactiveProperty(0);
    private float answerTime;


    public void InitializeQuestionPage(int level)
    {
        DisposeAllStreams();

        // ランダムで取りたい => 問題的に無理やった
        var currentQuestionTable = QuestionRepository.Instance.GetQuestionTableByIndex(level);
        questionMasters = currentQuestionTable.Questions;
        selectableAnswer = currentQuestionTable.Answer;

        for (var i = 0; i < questionMasters.Count; i++)
        {
            var questionView = Instantiate(questionViewPrefab);
            questionView.transform.SetParent (questionParentObject.transform, false);
            questionViews.Add(questionView.GetComponent<QuestionView>());
            questionViews[i].SetQuestionTextAndRow(i, questionMasters[i].Qustion);
        }

        answerView.SetButtonText(selectableAnswer.Answers);
        ResisterStreams();
    }

    public void ResisterStreams()
    {
        var answerStream1 = answerView.AnswerButton1
            .OnClickAsObservable()
            .Where(_ => canAnswer.Value)
            .Select(_ => selectableAnswer.Answers[0])
            .Subscribe(answer => CheckAnswer(answer));

        var answerStream2 = answerView.AnswerButton2
            .OnClickAsObservable()
            .Where(_ => canAnswer.Value)
            .Select(_ => selectableAnswer.Answers[1])
            .Subscribe(answer => CheckAnswer(answer));

        var answerStream3 = answerView.AnswerButton3
            .OnClickAsObservable()
            .Where(_ => canAnswer.Value)
            .Select(_ => selectableAnswer.Answers[2])
            .Subscribe(answer => CheckAnswer(answer));

        var enterStream = answerView.EnterButton
            .OnClickAsObservable()
            .Where(_ => canAnswer.Value)
            .Subscribe(_ => OnClickEnterButton());

        subscriptions.Add(answerStream1);
        subscriptions.Add(answerStream2);
        subscriptions.Add(answerStream3);
        subscriptions.Add(enterStream);
    }

    public void StartGame()
    {
        questionViews[currentRow.Value - 1].ActivateRowColor();
        canAnswer.Value = true;
        stopWatch.Restart();
    }

    public void CheckAnswer(string answer)
    {
        AudioManager.Instance.PlaySE(SE.Anykey.ToString());
        canAnswer.Value = false;
        SetActiveAnswerButton(false);
        var correct = questionMasters[currentRow.Value - 1].Answer.Contains(answer) && !answeredTextsInCurrentRow.Contains(answer);
        if (correct)
        {
            CorrectAnswer();
            answeredTextsInCurrentRow.Add(answer);
        }
        else
        {
            MistakeAnswer();
        }
        canAnswer.Value = true;
    }

    public void CorrectAnswer()
    {
        currentCombo.Value++;
        var result = new AnswerResults(AnswerResult.SelectCorrectAnswer,currentCombo.Value, -1);
        currentAnswerResult.OnNext(result);
        SetCorrectQuestionText(currentRow.Value - 1, true);
        questionViews[currentRow.Value].PlayParticle(2);
        SetActiveAnswerButton(true);
        AudioManager.Instance.PlaySE(SE.Correct_Small.ToString());
    }

    public void MistakeAnswer()
    {
        currentCombo.Value = 0;
        var result = new AnswerResults(AnswerResult.Mistake,currentCombo.Value, -1);
        currentAnswerResult.OnNext(result);
        SetCorrectQuestionText(currentRow.Value - 1 , false);
        AudioManager.Instance.PlaySE(SE.Incorrect.ToString());
        questionViews[currentRow.Value].PlayParticle(1);
        shakeView.Shake();
        NextRow();
    }

    public void NextRow()
    {
        var nextRow = currentRow.Value + 1;
        if (nextRow > questionMasters.Count)
        {
            FinishGame();
            return;
        }

        if(currentRow.Value % 8 == 0)
        {
            StartCoroutine(ScrollPage());
        }

        questionViews[currentRow.Value - 1].NonActivateRowColor();
        currentRow.Value++;
        questionViews[currentRow.Value - 1].ActivateRowColor();
        answeredTextsInCurrentRow.Clear();
        SetActiveAnswerButton(true);
        canAnswer.Value = true;
        stopWatch.Restart();
    }

    private IEnumerator ScrollPage()
    {
        // いつか修正
        float rate = ((float)currentRow.Value + 1) / questionMasters.Count;
        scrollbar.value = 1.0f - rate;
        yield return null;
    }

    public void OnClickEnterButton()
    {
        stopWatch.Stop();
        var ts = stopWatch.Elapsed;
        answerTime = ts.Seconds + ts.Milliseconds / 1000;
        AudioManager.Instance.PlaySE(SE.Enter.ToString());
        canAnswer.Value = false;
        SetActiveAnswerButton(false);

        bool correct = true;
        foreach(var answer in questionMasters[currentRow.Value - 1].Answer)
        {
            correct = answeredTextsInCurrentRow.Contains(answer);
        }

        if (correct)
        {
            AudioManager.Instance.PlaySE(SE.Correct.ToString());
            currentCombo.Value++;
            var result = new AnswerResults(AnswerResult.CorrectAllAnswer,currentCombo.Value, answerTime);
            currentAnswerResult.OnNext(result);
            questionViews[currentRow.Value].PlayParticle(0);
            currentCombo.Value++;
        }
        else
        {
            AudioManager.Instance.PlaySE(SE.Incorrect.ToString());
            shakeView.Shake();
            currentCombo.Value = 0;
            var result = new AnswerResults(AnswerResult.NotEnoughAnswer,currentCombo.Value, answerTime);
            currentAnswerResult.OnNext(result);
            questionViews[currentRow.Value].PlayParticle(1);
            currentCombo.Value = 0;
        }

        SetCorrectQuestionText(currentRow.Value - 1, correct);
        NextRow();
    }

    public void SetCorrectQuestionText(int index, bool correct)
    {
        var text = questionMasters[index].CorrectText;
        questionViews[index].SetQuestionText(text,correct);
    }

    public void SetActiveAnswerButton(bool flag)
    {
        answerView.SetActiveAllButton(flag);
    }

    public void FinishGame()
    {
        DisposeAllStreams();
    }

    public void DisposeAllStreams()
    {
        subscriptions.ForEach(s => s.Dispose());
        correctNumInCurrentRow = 0;
        answerTime = 0;
        currentCombo.Value = 0;
        currentRow.Value = 1;
        canAnswer.Value = false;
        stopWatch.Reset();
    }
}

public struct AnswerResults
{
    public AnswerResult IsCorrect;
    public int CurrentCombo;
    public float TakenAnswerTime;

   public AnswerResults(AnswerResult isCorrect, int combo, float time)
   {
       this.IsCorrect = isCorrect;
       this.CurrentCombo = combo;
       this.TakenAnswerTime = time;
   }
}

public enum AnswerResult
{
    SelectCorrectAnswer, // 正解のボタンを押した
    Mistake,　　　　　　　 // 間違えた
    CorrectAllAnswer,    // 正解のボタンを押した上で、Enterを押した
    NotEnoughAnswer　　　// 正解が足りない状態で、Enterを押した
}