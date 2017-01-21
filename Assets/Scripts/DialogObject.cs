using UnityEngine;
using System.Collections;

public class DialogObject {

    public float TimeOnScreen {get; private set; }
    public string Text { get; private set; }

    public DialogObject(string text, float timeOnScreen)
    {
        Text = text;
        TimeOnScreen = timeOnScreen;
    }
}
