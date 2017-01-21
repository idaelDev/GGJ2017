using UnityEngine;
using System.Collections;

public class EggObject : MonoBehaviour {

    [SerializeField]
    private float cookPercent = 0;

    [SerializeField]
    private float cookBaseSpeed = 1f;
    [SerializeField]
    private float cookSpeedMultiplierUp = 2f;
    [SerializeField]
    private float cookSpeedMultiplierDown = 0.5f;

    Animator anim;

    public AudioSource wellCookedAudio;
    public AudioClip[] audioclips;


    private bool isCookingStarted = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        MainMenu.Instance.startGameEvent += StartCooking;
        ConversationManager.Instance.endGameEvent += endCook;
    }

    // Update is called once per frame
    void Update () {
        if (isCookingStarted)
        {
            CookEgg();
            anim.SetFloat("CookePercentage", cookPercent);
        }
    }

    public void CookEgg()
    {
        float cookSpeed = cookBaseSpeed;
        if (ConversationManager.Instance.NegociatorState < 0)
        {
            cookSpeed *=cookSpeedMultiplierUp;
        }
        else if (ConversationManager.Instance.NegociatorState > 0)
        {
            cookSpeed *= cookSpeedMultiplierDown;
        }

        cookPercent += cookSpeed * Time.deltaTime;
    }

    private void StartCooking()
    {
        isCookingStarted = true;
        GetComponent<AudioSource>().Play();
    }

    private void endCook()
    {
        isCookingStarted = false;
        GetComponent<AudioSource>().Stop();
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("EggStep4"))
        {
            wellCookedAudio.clip = audioclips[1];
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("EggStep5") || anim.GetCurrentAnimatorStateInfo(0).IsName("EggStep6"))
        {
            wellCookedAudio.clip =  audioclips[2];
        }
        else
        {
            wellCookedAudio.clip = audioclips[0];
        }
        GetComponent<AudioSource>().Stop();
        wellCookedAudio.Play();
    }
}
