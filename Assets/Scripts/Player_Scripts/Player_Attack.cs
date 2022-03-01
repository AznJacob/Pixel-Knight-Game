using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    //Get Set Components
    public Player_Controller playerController;

    //Player Atk Attributes
    #region
    public int attackDamage = 20;

    //Attack Ranges
    public float standingAttackRange = .75f;
    public float crouchingAttackRange = .5f;

    //LayerMask Decides what gets hit
    public LayerMask attackMask;
    #endregion

    //Player Attack Management
    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * playerController.standingAttackOffset.x;
        pos += transform.up * playerController.standingAttackOffset.y;

        Vector3 posC = transform.position;
        posC += transform.right * playerController.crouchingAttackOffset.x;
        posC += transform.up * playerController.crouchingAttackOffset.y;

        Collider2D[] hitObjects = null;
        FindObjectOfType<AudioManager>().Play("SwordSwing");

        if (playerController.isCrouching == false)
        {
            hitObjects = Physics2D.OverlapCircleAll(pos, standingAttackRange, attackMask);
        }
        else
        {
            hitObjects = Physics2D.OverlapCircleAll(posC, crouchingAttackRange, attackMask);
        }

        foreach (Collider2D Enemy in hitObjects)
        {
            Enemy.GetComponent<Enemy_Health>().isHit(attackDamage);
            Debug.Log("Enemy Hit!!!" + Enemy.name);
            FindObjectOfType<AudioManager>().Play("SwordHit");
        }
    }
}
