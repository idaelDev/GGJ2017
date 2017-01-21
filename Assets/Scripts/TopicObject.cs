using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopicObject{

    public int TopicID { get; private set; }
    public List<DialogObject> Dialogs { get; private set; }

    public string PositiveAnswer { get; private set; }
    public string NeutralAnswer { get; private set; }
    public string NegativeAnswer { get; private set; }
    public string LeaveAnswer { get; private set; }

    public TopicObject(int id)
    {
        TopicID = id;
    }

    public void AddDialog(DialogObject obj)
    {
        Dialogs.Add(obj);
    }
    
    public void addAnswers(string pos, string neg, string neutral, string go)
    {
        PositiveAnswer = pos;
        NegativeAnswer = neg;
        NeutralAnswer = neutral;
        LeaveAnswer = go;
    }
}
