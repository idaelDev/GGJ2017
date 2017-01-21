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

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        CookEgg();
        anim.SetFloat("CookePercentage", cookPercent);
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
        Debug.Log(cookSpeed);
        cookPercent += cookSpeed * Time.deltaTime;
    }
}
