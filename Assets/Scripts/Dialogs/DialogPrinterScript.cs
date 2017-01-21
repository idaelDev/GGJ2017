using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogPrinterScript : MonoBehaviour {

    public Text textUI;
    public Button[] buttons;
    public Button goButton;
    public CanvasGroup group;
    public Image timerImg;

    public float timer = 5f;

    public delegate void EndOfPrintDelegate();
    public event EndOfPrintDelegate endOfPrintEvent;

    private Coroutine routineTimer;

    void Start()
    {
        MainMenu.Instance.startGameEvent += ShowGroup;
        ConversationManager.Instance.endGameEvent += HideGroup;
    }

    private void ShowGroup()
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void HideGroup()
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void HideAnswer()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        goButton.interactable = false;
    }

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

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
        goButton.interactable = true;

        routineTimer = StartCoroutine(timerCoroutine());
    }


    public void PrintDialog(string dial, double time, int state)
    {
        StartCoroutine(PrintCoroutine(dial, time));
    }

    public void Answer(int val)
    {
        ConversationManager.Instance.Answer(val, false);
        StopCoroutine(routineTimer);
    }

    IEnumerator PrintCoroutine(string text, double time)
    {
        textUI.text = text;
        yield return new WaitForSeconds((float)time);
        endOfPrintEvent();
    }

    IEnumerator timerCoroutine()
    {
        float t = 0;
        while(t < timer)
        {
            float r = Mathf.Lerp(1, 0, t / timer);
            t += Time.deltaTime;
            timerImg.fillAmount = r;
            yield return 0;
        }
        ConversationManager.Instance.Answer(-1, true);
    }
}
