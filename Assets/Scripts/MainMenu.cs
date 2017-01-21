using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu> {

    public CanvasGroup mainScreen;
    public CanvasGroup credits;

    public delegate void StartGameDelegate();
    public event StartGameDelegate startGameEvent;

    private void ShowGroup(CanvasGroup group, bool show)
    {
        group.alpha = (show) ? 1 : 0;
        group.interactable = show;
        group.blocksRaycasts = show;
    }

    public void OnPlay()
    {
        ShowGroup(mainScreen, false);
        startGameEvent();
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

}
