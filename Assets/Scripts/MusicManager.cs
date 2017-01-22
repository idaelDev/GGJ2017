using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip intro;
    public AudioClip main;

    AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        MainMenu.Instance.startGameEvent += StartGame;
        ConversationManager.Instance.endGameEvent += EndGame;
	}

    void StartGame()
    {
        source.clip = main;
        source.volume = 0.2f;
        source.Play();
    }

    void EndGame()
    {
        source.clip = intro;
        source.volume = 0.5f;
        source.Play();
    }
}
