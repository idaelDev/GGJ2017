using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogPrinterScript : MonoBehaviour {

    public Text textUI;
    public Button[] buttons;
    public Button goButton;

    public delegate void EndOfPrintDelegate();
    public event EndOfPrintDelegate endOfPrintEvent;

    private List<int> baseOrder;
    private List<int> answersOrder;

    public void setupAnswers(string neg, string neu, string pos, string go)
    {
        buttons[0].GetComponentInChildren<Text>().text = neg;
        buttons[1].GetComponentInChildren<Text>().text = neu;
        buttons[2].GetComponentInChildren<Text>().text = pos;

        int r = (int)Random.Range(1f, 2.99f);
        Vector2 buf = buttons[0].GetComponent<RectTransform>().position;
        buttons[0].GetComponent<RectTransform>().position = buttons[r].GetComponent<RectTransform>().position;
        buttons[r].GetComponent<RectTransform>().position = buf;

        r = (int)Random.Range(0f, 1.99f);
        buf = buttons[2].GetComponent<RectTransform>().position;
        buttons[2].GetComponent<RectTransform>().position = buttons[r].GetComponent<RectTransform>().position;
        buttons[r].GetComponent<RectTransform>().position = buf;

        goButton.GetComponentInChildren<Text>().text = go;

    }

    public void PrintDialog(string dial, double time, int state)
    {
        StartCoroutine(PrintCoroutine(dial, time));
    }

    IEnumerator PrintCoroutine(string text, double time)
    {
        textUI.text = text;
        yield return new WaitForSeconds((float)time);
        endOfPrintEvent();
    }
}
