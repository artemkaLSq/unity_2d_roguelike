using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public int damage = 1;                             
    private Animator animator;                            
    private Transform target;                            
    public int hp = 3;

    protected override void Start()
    {
        Game.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    public void DamageEnemy(int loss)
    {
        //SoundManager.instance(damageSound1);
        animator.SetTrigger("EnemyHit");
        hp -= loss;
        if (hp <= 0)
            gameObject.SetActive(false);
    }


    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);
    }


    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;
        AttemptMove<Player>(xDir, yDir);
    }


    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.TakeDamage(damage);
        animator.SetTrigger("EnemyAttack");
    }
}

