using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AnswerView : BaseView
{
    public Button AnswerButton1, AnswerButton2, AnswerButton3;
    public Image AnswerImage1, AnswerImage2, AnswerImage3;
    public Button EnterButton;

    public List<Sprite> AnswerImages;

    public void SetButtonText(List<string> answers)
    {
        var answerNumber = answers.Select(ans =>
        {
            switch(ans)
            {
                case ";":
                    return 0;
                case "=":
                    return 1;
                case "{}":
                    return 2;
                default:
                    break;
            }
            return 0;
        }).ToList();

        AnswerImage1.sprite = AnswerImages[answerNumber[0]];
        AnswerImage2.sprite = AnswerImages[answerNumber[1]];
        AnswerImage3.sprite = AnswerImages[answerNumber[2]];
    }

    public void SetActiveAllButton(bool flag)
    {
        AnswerButton1.interactable = flag;
        AnswerButton2.interactable = flag;
        AnswerButton3.interactable = flag;
        EnterButton.interactable = flag;
    }
}
