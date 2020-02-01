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

    // エフェクトとかもここでやって良さそう

    public override void InitializeByPresenter()
    {
        this.RowText.text = "";
        this.QuestionText.text = "";
        this.RowColorImage.color = NonActiveRowColor;
    }

    public void SetQuestionText(int row, string question)
    {
        this.RowText.text = row.ToString();
        this.QuestionText.text = question;
    }

    public void ActivateRowColor()
    {
        this.RowColorImage.color = ActiveRowColor;
        Debug.Log("Active:" + this.gameObject.name);
    }

    public void NonActivateRowColor()
    {
        this.RowColorImage.color = NonActiveRowColor;
        Debug.Log("NonActive:" + this.gameObject.name);
    }
}
