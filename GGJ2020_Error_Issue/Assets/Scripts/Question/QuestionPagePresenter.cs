using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class QuestionPagePresenter : MonoBehaviour
{
    [SerializeField] private List<QuestionView> questionViews;
    [SerializeField] private AnswerView answerView;


    private List<QuestionMaster> questionMasters;
    private Answer selectableAnswer;
    private ReactiveProperty<int> currentRow = new IntReactiveProperty(1);
    private ReactiveProperty<bool> canAnswer = new BoolReactiveProperty(true);
    private int correctNumInCurrentRow;

    private Subject<AnswerResult> isCorrectAnswer = new Subject<AnswerResult>();
    public IObservable<AnswerResult> IsCorrectAnswer
    {
        get { return isCorrectAnswer;}
    }

    void Start()
    {
        InitializeQuestionPage(1);
    }

    public void InitializeQuestionPage(int level)
    {
        DisposeAllStreams();

        var currentQuestionTable = QuestionRepository.Instance.GetQuestionTableByIndex(level);
        questionMasters = currentQuestionTable.Questions;
        selectableAnswer = currentQuestionTable.Answer;

        for (var i = 0; i < questionMasters.Count; i++)
        {
            questionViews[i].SetQuestionText(i, questionMasters[i].Qustion);
        }

        currentRow.Value = 1;
        questionViews[currentRow.Value - 1].ActivateRowColor();

        answerView.SetButtonText(selectableAnswer.Answers);
        ResisterStreams();
    }

    public void DisposeAllStreams()
    {

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
    }

    public void CheckAnswer(string answer)
    {
        canAnswer.Value = false;
        SetActiveAnswerButton(false);
        var correct = questionMasters[currentRow.Value - 1].Answer.Contains(answer);
        if (correct)
        {
            CorrectAnswer();
        }
        else
        {
            MistakeAnswer();
        }
        canAnswer.Value = true;
    }

    public void CorrectAnswer()
    {
        Debug.Log("Correct");
        isCorrectAnswer.OnNext(AnswerResult.SelectCorrectAnswer);
        correctNumInCurrentRow++;
        SetActiveAnswerButton(true);
    }

    public void MistakeAnswer()
    {
        Debug.Log("Mistake");
        isCorrectAnswer.OnNext(AnswerResult.Mistake);
        NextRow();
    }

    public void NextRow()
    {
        var nextRow = currentRow.Value + 1;
        if (nextRow > questionMasters.Count)
        {
            Debug.Log("しゅうりょう");
            return;
        }

        questionViews[currentRow.Value - 1].NonActivateRowColor();
        currentRow.Value++;
        questionViews[currentRow.Value - 1].ActivateRowColor();
        correctNumInCurrentRow = 0;
        SetActiveAnswerButton(true);
        canAnswer.Value = true;
    }

    public void OnClickEnterButton()
    {
        canAnswer.Value = false;
        SetActiveAnswerButton(false);
        if (correctNumInCurrentRow == questionMasters[currentRow.Value - 1].Answer.Count)
        {
            isCorrectAnswer.OnNext(AnswerResult.CorrectAllAnswer);
            Debug.Log("CorrectEnter");
        }
        else
        {
            isCorrectAnswer.OnNext(AnswerResult.NotEnoughAnswer);
            Debug.Log("MisEnter");
        }

        NextRow();
    }

    public void SetActiveAnswerButton(bool flag)
    {
        answerView.SetActiveAllButton(flag);
    }

}


public enum AnswerResult
{
    SelectCorrectAnswer, // 正解のボタンを押した
    Mistake,　　　　　　　 // 間違えた
    CorrectAllAnswer,    // 正解のボタンを押した上で、Enterを押した
    NotEnoughAnswer　　　// 正解が足りない状態で、Enterを押した
}