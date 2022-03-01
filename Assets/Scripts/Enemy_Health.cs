using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{

    //Enemy Current Health
    public int Health = 100;

    //The Enemy Object
    public GameObject itself;

    //Enemy Collider
    public Collider2D m_Collider;

    //For if it is dead
    public bool isDead = false;

    //Player Animator
    public Animator enemyAnimator;

    //Enemy Attack Value
    public int AtkVal = 20;

    public GameObject player;

    //Rigidbody
    public Rigidbody2D rb;

    bool isInvincible = false;

    void Awake()
    {
        m_Collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (this.tag == "Enemy" || this.tag == "SkeletonEnemy" || this.tag == "BatEnemy" || this.tag == "MushroomEnemy" || this.tag == "GoblinEnemy")
        {
            enemyAnimator = GetComponent<Animator>();
        }
    }

    //For Taking Damage
    public void isHit(int atkValue)
    {
        if(isDead == false)
        {
            if(isInvincible == false)
            {
                if(Health > 0)
                {
                    enemyAnimator.SetTrigger("isHit");
                    Health -= atkValue;
                }
                if(Health <= 0)
                {
                    //player.GetComponent<ScoreScript>().ScoreNum = player.GetComponent<ScoreScript>().ScoreNum + 20;
                    enemyAnimator.SetTrigger("isDead");
                    isDead = true;
                    rb.constraints = RigidbodyConstraints2D.FreezePosition;
                    m_Collider.enabled = false;
                    if (itself.tag == "SkeletonEnemy")
                    {
                        FindObjectOfType<AudioManager>().Play("SkeletonDeath");
                    }
                    if (itself.tag == "BatEnemy")
                    {
                        FindObjectOfType<AudioManager>().Play("flyeyeDeath");
                    }
                    if (itself.tag == "MushroomEnemy")
                    {
                        FindObjectOfType<AudioManager>().Play("mushroomDeath");
                    }
                    if (itself.tag == "GoblinEnemy")
                    {
                        FindObjectOfType<AudioManager>().Play("goblinDeath");
                    }
                    if (itself.tag == "Boss")
                    {
                        FindObjectOfType<AudioManager>().Play("bossDeath");
                    }
                    Destroy(itself,2f);
                    //FindObjectOfType<AudioManager>().Play("enemyDeath");
                }
            }
        }
    }

    //returns damage value
    public int returnDamVal()
    {
        return AtkVal;
    }

    public void InvincibleSwitch()
    {
        isInvincible = !isInvincible;
    }
}
