using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopicObject{

    public int TopicID { get; private set; }
    private List<DialogObject> Dialogs;

    public string PositiveAnswer { get; private set; }
    public string NeutralAnswer { get; private set; }
    public string NegativeAnswer { get; private set; }
    public string LeaveAnswer { get; private set; }

    public string PositiveTransition { get; private set; }
    public string NeutralTransition { get; private set; }
    public string NegativeTransition { get; private set; }
    public string LeaveTransition { get; private set; }

    public int Count
    {
        get { return Dialogs.Count; }
    }

    public DialogObject getDialog(int id)
    {
        return Dialogs[id];
    }

    public TopicObject(int id)
    {
        TopicID = id;
        Dialogs = new List<DialogObject>();
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

    public void addTransitions(string pos, string neg, string neutral, string go)
    {
        PositiveTransition = pos;
        NegativeTransition = neg;
        NeutralTransition = neutral;
        LeaveTransition = go;
    }

    public override string ToString()
    {
        string s = "Topic no : "+TopicID +"\n";
        for (int i = 0; i < Dialogs.Count; i++)
        {
            s += Dialogs[i].ToString();
        }
        s += "\n";
        s += "Neutral : " + NeutralAnswer + "\n";
        s += "Positiv : " + PositiveAnswer + "\n";
        s += "Negativ : " + NegativeAnswer + "\n";
        s += "Leave : " + LeaveAnswer + "\n";
        return s;
    }
}
