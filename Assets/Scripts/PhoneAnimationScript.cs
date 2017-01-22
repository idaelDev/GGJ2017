using UnityEngine;
using System.Collections;

public class PhoneAnimationScript : MonoBehaviour {

    Animator anim;
    public Animator momAnimator;
    public SpriteRenderer sr;
    public SpriteRenderer momRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        momAnimator.enabled = false;
        MainMenu.Instance.startGameEvent += startWaves;
        ConversationManager.Instance.stateChangeEvent += StatChange;
        ConversationManager.Instance.endGameEvent += endWaves;
    }

    void startWaves()
    {
        anim.enabled = true;
        momAnimator.enabled = true;
    }

    void endWaves()
    {
        anim.enabled = false;
        momAnimator.enabled = false;
        sr.enabled = false;
        momRenderer.enabled = false;

    }


	void StatChange (int val) {
        anim.SetInteger("negociatorState", val);
        momAnimator.SetInteger("State", val);
    }
}
