using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField]
    private float damages, walkSpeed, sightRange, attackRange, runSpeed, jumpForce, maxHealth, currentHealth, attackPeriod;
    //private bool knockback;
    //private Vector2 knockbackForce;

    private bool canMove = true;
    public float Damages
    {
        get
        {
            return damages;
        }
    }
    public float AttackPeriod
    {
        get
        {
            return attackPeriod;
        }
    }
    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
    }
    public float WalkSpeed
    {
        get
        {
            return walkSpeed;
        }
    }
    public float RunSpeed
    {
        get
        {
            return runSpeed;
        }
    }
    public float SightRange
    {
        get
        {
            return sightRange;
        }
    }
    public float AttackRange
    {
        get
        {
            return attackRange;
        }
    }
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }
    public bool CanMove
    {
        get
        {
            return canMove;
        }
    }
    public void TakeDamage(float damage, Vector2 repulsion)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        GameEvents.current.EnemyTookDamage(gameObject, damage);
        if (currentHealth <= 0)
        {
            Die();
        }
        
    }
    public void Knockback(Vector2 force)
    {
        //knockback = true;
        GetComponent<Rigidbody2D>().AddForce(force);
        //knockbackForce = force;
    }
    private void FixedUpdate()
    {
       /* if(knockback)
        {
            GetComponent<Rigidbody2D>().AddForce(knockbackForce);
            knockback = false;
        }*/
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    public void Stun()
    {
        canMove = false;
        StartCoroutine(WaitForEndOfStun());
    }
    IEnumerator WaitForEndOfStun()
    {
        yield return new WaitForSeconds(2);
        canMove = true;

    }
}

