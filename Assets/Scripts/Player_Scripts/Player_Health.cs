using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Health : MonoBehaviour
{
    //Player Health Attributes
    #region
    //Player Maximum Health
    public int maxHealth = 100;
    //Player's Current Health
    public int currentHealth;

    public Health_Bar healthBar;
    #endregion

    //Get Set Components
    public Player_Controller playerController;
    public Animator playerAnimator;

    private LevelLoader levelLoader;

    //For Invincibility Frames
    private float invincibilityTime = 1f;
    private float invincibilityCounter;

    bool isInvincible = false;

    void Awake()
    {
        //Set Player Health
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);

        //Get References
        playerController = GetComponent<Player_Controller>();
        //Get Set Player Animator
        playerAnimator = GetComponent<Animator>();
        //Get LevelLoader
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    void Update()
    {
        if(invincibilityCounter <= 0)
        {
            isInvincible = false;
        }
        else
        {
            invincibilityCounter -= Time.deltaTime;
        }
    }

    //Player Health Management
    public void TakeDamage(int Damage)
    {
        if(isInvincible == false)
        {
            currentHealth -= Damage;
            healthBar.SetHealth(currentHealth);
            if (currentHealth > 0)
            {
                StartCoroutine(DamageAnimation());
                playerAnimator.SetTrigger("takeDamage");
                invincibilityCounter = invincibilityTime;
                isInvincible = true;
                FindObjectOfType<AudioManager>().Play("playerHit");
            }
            else
            {
                playerController.canMoveFunction();
                playerController.isAliveFunction();
                playerAnimator.SetTrigger("hasDied");
                PlayerHasDied();
            }
        }
        
    }

    //For colliding with Enemy
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "SkeletonEnemy" || col.gameObject.tag == "BatEnemy" || col.gameObject.tag == "MushroomEnemy" || col.gameObject.tag == "GoblinEnemy")
        {
            int damage = col.gameObject.GetComponent<Enemy_Health>().returnDamVal();
            TakeDamage(damage);
        }
        else if(col.gameObject.tag == "KillPlane")
        {
            PlayerHasDied();
        }
    }

    //For Dying
    void PlayerHasDied()
    {
        levelLoader.LoadNextLevel(SceneManager.GetActiveScene().buildIndex);
    }

    //Player Flashes When Hit
    IEnumerator DamageAnimation()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 0;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);

            foreach (SpriteRenderer sr in srs)
            {
                Color c = sr.color;
                c.a = 1;
                sr.color = c;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D Collectable)
    {
        if (Collectable.tag == "Heart")
        {
            currentHealth += 20;
            healthBar.SetHealth(currentHealth);
            Destroy(Collectable.gameObject);
            FindObjectOfType<AudioManager>().Play("heartCollect");
        }
    }

    }
