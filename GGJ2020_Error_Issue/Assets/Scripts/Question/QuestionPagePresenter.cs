﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

public class QuestionPagePresenter : MonoBehaviour
{
    [SerializeField] private List<QuestionView> questionViews;
    [SerializeField] private AnswerView answerView;

    private List<QuestionMaster> questionMasters;
    private Answer selectableAnswer;
    public List<IDisposable> subscriptions = new List<IDisposable>();
    private ReactiveProperty<int> currentRow = new IntReactiveProperty(1);
    private ReactiveProperty<bool> canAnswer = new BoolReactiveProperty(false);
    private int correctNumInCurrentRow;
    private List<string> answeredTextsInCurrentRow = new List<string>();

    private Subject<AnswerResult> isCorrectAnswer = new Subject<AnswerResult>();
    public IObservable<AnswerResult> IsCorrectAnswer
    {
        get { return isCorrectAnswer;}
    }

    public void InitializeQuestionPage(int level)
    {
        DisposeAllStreams();

        // ランダムで取りたい
        var currentQuestionTable = QuestionRepository.Instance.GetQuestionTableByIndex(level);
        questionMasters = currentQuestionTable.Questions;
        selectableAnswer = currentQuestionTable.Answer;

        for (var i = 0; i < questionMasters.Count; i++)
        {
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
        isCorrectAnswer.OnNext(AnswerResult.SelectCorrectAnswer);
        SetCorrectQuestionText(currentRow.Value - 1, true);
        SetActiveAnswerButton(true);
        AudioManager.Instance.PlaySE(SE.Correct_Small.ToString());
    }

    public void MistakeAnswer()
    {
        isCorrectAnswer.OnNext(AnswerResult.Mistake);
        SetCorrectQuestionText(currentRow.Value - 1 , false);
        AudioManager.Instance.PlaySE(SE.Incorrect.ToString());
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

        questionViews[currentRow.Value - 1].NonActivateRowColor();
        currentRow.Value++;
        questionViews[currentRow.Value - 1].ActivateRowColor();
        answeredTextsInCurrentRow.Clear();
        SetActiveAnswerButton(true);
        canAnswer.Value = true;
    }

    public void OnClickEnterButton()
    {
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
            isCorrectAnswer.OnNext(AnswerResult.CorrectAllAnswer);
        }
        else
        {
            AudioManager.Instance.PlaySE(SE.Incorrect.ToString());
            isCorrectAnswer.OnNext(AnswerResult.NotEnoughAnswer);
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
        currentRow.Value = 1;
        canAnswer.Value = false;
    }
}


public enum AnswerResult
{
    SelectCorrectAnswer, // 正解のボタンを押した
    Mistake,　　　　　　　 // 間違えた
    CorrectAllAnswer,    // 正解のボタンを押した上で、Enterを押した
    NotEnoughAnswer　　　// 正解が足りない状態で、Enterを押した
}