using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionRepository
{
    public static QuestionRepository Instance = new QuestionRepository();
    Dictionary<int,QuestionTable> QuestionTables = new Dictionary<int, QuestionTable> ();
    // Dictionary<List<QuestionMaster>> Questions = new List<List<QuestionMaster>> ();
    // List<Answer> Answers = new List<Answer> ();

    public QuestionRepository () {
        Initialize ();
    }

    private void Initialize () {
        for (var i = 1; i < 2; i++) {
            QuestionTable questionTable = Resources.Load<QuestionTable> ($"QuestionTable{i}");
            if (questionTable == null) {
                continue;
            }
            QuestionTables[i] = questionTable;
            // Questions.Add (questionTable.Questions);
            // Answers.Add (questionTable.Answer);
        }
    }

    public QuestionTable GetQuestionTableByIndex(int index)
    {
        if(!QuestionTables.TryGetValue(index, out var questionTable))
        {
            throw new System.Exception("指定されたindexのQuestionTableは存在しません");
        }
        return questionTable;
    }
}