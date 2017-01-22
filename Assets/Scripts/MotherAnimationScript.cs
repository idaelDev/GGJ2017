using UnityEngine;
using System.Collections;

public class MotherAnimationScript : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        ConversationManager.Instance.stateChangeEvent += onStateChange;
	}
	
	void onStateChange(int val)
    {
        anim.SetInteger("State", val);
    }
}
