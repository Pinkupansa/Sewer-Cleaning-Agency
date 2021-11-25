using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//Script managing the behaviour of a projectile
public class Projectile : MonoBehaviour
{
    float damage;
    Rigidbody2D rigidbody;
    bool useGravity;

    GameObject caster;
    public UnityEvent<Vector2, Rigidbody2D> OnHit = new UnityEvent<Vector2, Rigidbody2D>();

    //Used by the attack script to set the characteristics of the projectile
    public void SetParameters(float damage, float speed, bool useGravity, Vector2 direction, Vector2 casterSpeed, GameObject caster)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        this.damage = damage;
        
        if (useGravity)
        {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
        rigidbody.velocity = direction * speed + casterSpeed.x * Vector2.right;
        this.caster = caster;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject != caster && collision.gameObject.layer == 8)
        {
            
            IDamageable damaged = collision.gameObject.GetComponent<IDamageable>();
            if (damaged != null)
            {
                damaged.TakeDamage(damage, rigidbody.velocity.normalized);
            }

            OnHit.Invoke(transform.position, collision.GetComponent<Rigidbody2D>());
            Destroy(gameObject);
        }
    }
    
       
        
    
}
