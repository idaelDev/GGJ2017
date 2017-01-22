using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu> {

    public CanvasGroup mainScreen;
    public CanvasGroup credits;
    public CanvasGroup TutoGroup;

    public delegate void StartGameDelegate();
    public event StartGameDelegate startGameEvent;

    public AudioSource egg;
    public DialogPrinterScript dial;

    private void ShowGroup(CanvasGroup group, bool show)
    {
        group.alpha = (show) ? 1 : 0;
        group.interactable = show;
        group.blocksRaycasts = show;
    }

    public void OnPlay()
    {
        ShowGroup(mainScreen, false);
        ShowGroup(TutoGroup, true);
    }

    public void OnOK()
    {
        ShowGroup(TutoGroup, false);
        dial.ShowGroup();
        StartCoroutine(fadeCoroutine());
    }

    public void OnCredits()
    {
        ShowGroup(mainScreen, false);
        ShowGroup(credits, true);
    }


    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnBack()
    {
        ShowGroup(credits, false);
        ShowGroup(mainScreen, true);
    }

    IEnumerator fadeCoroutine()
    {
        float fadeTime = 1.0f;
        float timer = 0;
        egg.volume = 0.25f;
        egg.Play();
        while(timer < fadeTime)
        {
            float r = Mathf.Lerp(1, 0, timer / fadeTime);
            MusicManager.Instance.source.volume = r;
            timer += Time.deltaTime;
            yield return 0;
        }
        MusicManager.Instance.source.volume = 0;
        startGameEvent();
    }

}
