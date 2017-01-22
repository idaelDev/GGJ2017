using UnityEngine;
using System.Collections;

public class EndAnimScript : MonoBehaviour {

    public Sprite[] sprites;
    public EggObject egg;

    SpriteRenderer spriteRenderer;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent <SpriteRenderer > ();
        ConversationManager.Instance.endGameEvent += SelectSprite;
    }

    void SelectSprite()
    {
        spriteRenderer.sprite = sprites[egg.EggState - 1];
        anim.SetTrigger("End");
    }

}
