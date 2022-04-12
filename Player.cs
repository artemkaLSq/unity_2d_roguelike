using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    public int damage = 1;
    public Text HPText;
    public Canvas gameoverCanvas;
    private Animator animator;
    private int hp;
    private Vector2 touchOrigin = -Vector2.one;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        hp = Game.instance.playerhp;
        HPText.text = "HP: " + hp;
        base.Start();
    }


    private void OnDisable()
    {
        Game.instance.playerhp = hp;
    }


    private void Update()
    {
        if (!Game.instance.playersTurn) return;
        int horizontal = 0;      
        int vertical = 0;
#if UNITY_STANDALONE || UNITY_WEBPLAYER

            horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
            vertical = (int) (Input.GetAxisRaw ("Vertical"));
            if(horizontal != 0)
            {
                vertical = 0;
            }
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];
                if (myTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = myTouch.position;
                }
                else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
                {
                    Vector2 touchEnd = myTouch.position;
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;
                    touchOrigin.x = -1;
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        horizontal = x > 0 ? 1 : -1;
                    else
                        vertical = y > 0 ? 1 : -1;
                }
            }

#endif



        if (horizontal != 0 || vertical != 0)
        {
            RaycastHit2D hitType = Physics2D.Linecast((Vector2)transform.position + new Vector2(horizontal, vertical), (Vector2)transform.position + new Vector2(horizontal+0.1f, vertical+0.1f), blockingLayer);
            
            if (hitType.transform == null)
                AttemptMove<Box>(horizontal, vertical);
            else
            {
                if (hitType.transform.tag == "Wall")
                {
                    return;
                }
                else if (hitType.transform.tag == "Enemy")
                {
                    AttemptMove<Enemy>(horizontal, vertical);
                }
                else
                {
                    AttemptMove<Box>(horizontal, vertical);
                }
            }
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //hp--;
        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;
        CheckIfGameOver();
        Game.instance.playersTurn = false;
    }


    protected override void OnCantMove<T>(T component)
    {
        if (component is Box)
        {
            Box hitBox = component as Box;
            hitBox.DamageBox(damage);
        }

        else if (component is Enemy)
        {
            Enemy hitEnemy = component as Enemy;
            hitEnemy.DamageEnemy(damage);
        }

        animator.SetTrigger("PlayerAttack");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", 1);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            hp++;
            HPText.text = "HP: " + hp;
            other.gameObject.SetActive(false);
        }
    }


    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }


    public void TakeDamage(int loss)
    {
        animator.SetTrigger("PlayerHit");
        hp -= loss;
        HPText.text = "HP: " + hp;
        CheckIfGameOver();
    }


    private void CheckIfGameOver()
    {
        if (hp <= 0)
        {
            gameoverCanvas.enabled = true;
            enabled = false;
            Game.instance.GameOver();
        }
    }

}
