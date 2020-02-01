using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerView : BaseView
{
    public Button AnswerButton1, AnswerButton2, AnswerButton3;
    public Button EnterButton;

    public void SetButtonText(List<string> answers)
    {
        AnswerButton1.GetComponentInChildren<Text>().text = answers[0];
        AnswerButton2.GetComponentInChildren<Text>().text = answers[1];
        AnswerButton3.GetComponentInChildren<Text>().text = answers[2];
    }

    public void SetActiveAllButton(bool flag)
    {
        AnswerButton1.interactable = flag;
        AnswerButton2.interactable = flag;
        AnswerButton3.interactable = flag;
        EnterButton.interactable = flag;
    }
}
