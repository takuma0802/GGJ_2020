using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionRepository : MonoBehaviour
{
  List<QuestionTable> QuestionTables = new List<QuestionTable>();
  List<List<QuestionMaster>> Questions = new List<List<QuestionMaster>>();
  List<Answer> Answers = new List<Answer>();

  void Start()
  {
      Initialize();
      Debug.Log("Count:" + Questions[0].Count + "0:" + Questions[0][0].Qustion);
  }

  public bool Initialize()
  {
    for (var i = 1; i < 2; i++)
    {
      QuestionTable questionTable = Resources.Load<QuestionTable>($"QuestionTable{i}");
      if (questionTable == null) {
          return false;
      }
      QuestionTables.Add(questionTable);
      Questions.Add(questionTable.Questions);
      Answers.Add(questionTable.Answer);
    }
    return true;
  }
}
