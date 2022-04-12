using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    //public AudioClip damageSound1;                                
    public Sprite dmgSprite;                    
    public int hp;                          

    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void DamageBox(int loss)
    {
        //SoundManager.instance(damageSound1);
        spriteRenderer.sprite = dmgSprite;
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
