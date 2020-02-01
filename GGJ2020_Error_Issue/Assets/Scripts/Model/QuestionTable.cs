using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CreateQuestionList",
    menuName = "ScriptableObject/CreateQuestionList")]

public class QuestionTable : ScriptableObject
{
  public Answer Answer;
  public List<QuestionMaster> Questions = new List<QuestionMaster>();
}

[System.Serializable]
public class Answer
{
  public List<string> Answers;
}

//System.Serializableを設定しないと、データを保持できない(シリアライズできない)ので注意
[System.Serializable]
public class QuestionMaster
{
  public string Qustion;
  public List<int> Answer;
}
