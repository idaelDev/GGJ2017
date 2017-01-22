using UnityEngine;
using System.Collections;

public class MotherAnimationScript : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        ConversationManager.Instance.stateChangeEvent += onStateChange;
	}
	
    void GameStart()
    {

    }

	void onStateChange(int val)
    {
        anim.SetInteger("State", val);
    }
}
