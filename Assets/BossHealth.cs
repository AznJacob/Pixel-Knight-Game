using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    //Boss currentHealth Attributes
    #region
    //Boss Maximum currentHealth
    public int maxHealth;
    //Boss's Current currentHealth
    public int currentHealth;

    public Health_Bar healthBar;
    #endregion

    public Enemy_Health enemy_Health;

    void Awake()
    {
        currentHealth = enemy_Health.Health;
        healthBar.SetMaxHealth(currentHealth);
    }

    void Update()
    {
        healthBar.SetHealth(enemy_Health.Health);
    }
}
