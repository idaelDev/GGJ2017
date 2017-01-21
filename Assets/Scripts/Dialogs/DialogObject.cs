using UnityEngine;
using System.Collections;

public class DialogObject {

    public double TimeOnScreen {get; private set; }
    public string Text { get; private set; }

    public DialogObject(string text, double timeOnScreen)
    {
        Text = text;
        TimeOnScreen = timeOnScreen;
    }

    public override string ToString()
    {
        return "Phrase : " + Text + "; Time : " + TimeOnScreen + "\n";
    }
}
