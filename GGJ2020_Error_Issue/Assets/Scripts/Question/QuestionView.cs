using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class QuestionView : BaseView
{
    [SerializeField] private Text RowText, QuestionText;
    [SerializeField] private Image RowColorImage;

    [SerializeField] private Color NonActiveRowColor;
    [SerializeField] private Color ActiveRowColor;
    [SerializeField] private List<ParticleSystem> feedBackParticles;

    // エフェクトとかもここでやって良さそう

    public override void InitializeByPresenter()
    {
        this.RowText.text = "";
        this.QuestionText.text = "";
        this.RowColorImage.color = NonActiveRowColor;
    }

    public void SetQuestionTextAndRow(int row, string question)
    {
        this.RowText.text = row.ToString();
        SetQuestionText(question, false);
    }

    public void SetQuestionText(string question, bool correct)
    {
        this.QuestionText.text = question;
    }

    public void ActivateRowColor()
    {
        this.RowColorImage.color = ActiveRowColor;
    }

    public void NonActivateRowColor()
    {
        this.RowColorImage.color = NonActiveRowColor;
    }

    public void PlayParticle(int index)
    {
        feedBackParticles[index].Play();
    }
}
