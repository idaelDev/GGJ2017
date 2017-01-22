using UnityEngine;
using System.Collections;

public class PhoneAnimationScript : MonoBehaviour {

    Animator anim;
    public Animator momAnimator;
    public Animator endAnimator;
    public SpriteRenderer sr;
    public SpriteRenderer momRenderer;
    public SpriteRenderer call;

    public Sprite[] momsHead;

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
        call.enabled = false;
        anim.enabled = true;
        momAnimator.enabled = true;
    }

    void endWaves()
    {
        anim.enabled = false;
        momAnimator.enabled = false;
        sr.enabled = false;
        endAnimator.SetTrigger("End");

        momRenderer.sprite = momsHead[ConversationManager.Instance.GetScoreEvaluation() + 1];
    }


	void StatChange (int val) {
        anim.SetInteger("negociatorState", val);
        momAnimator.SetInteger("State", val);
    }
}
