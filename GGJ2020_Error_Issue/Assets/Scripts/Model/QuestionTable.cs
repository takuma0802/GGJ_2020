using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CreateQuestionList",
    menuName = "ScriptableObject/CreateQuestionList")]

public class QuestionTable : ScriptableObject
{
  public Answer Answer;
  public List<Question> Questions = new List<Question>();
}

[System.Serializable]
public class Answer
{
  public List<string> Answers;
}

//System.Serializableを設定しないと、データを保持できない(シリアライズできない)ので注意
[System.Serializable]
public class Question
{
  public string Qustion;
  public List<int> Answer;
}
