using UnityEngine;
using System.Collections;

public class PhoneWavesScript : MonoBehaviour {

    Animator anim;
    bool start = false;
    public SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        MainMenu.Instance.startGameEvent += startWaves;
        ConversationManager.Instance.stateChangeEvent += StatChange;
        ConversationManager.Instance.endGameEvent += endWaves;
    }

    void startWaves()
    {
        anim.enabled = true;
    }

    void endWaves()
    {
        anim.enabled = false;
        sr.enabled = false;

    }


	void StatChange (int val) {
        anim.SetInteger("negociatorState", val);
	}
}
