using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlastProjectile : MonoBehaviour
{
    private float repellForce;
    private Rigidbody2D rb;

    private float spellDuration;
    private GameObject caster;
    public void Set(float _repellForce, float _spellDuration, GameObject caster, Vector2 speed)
    {
        repellForce = _repellForce;
        Debug.Log(repellForce);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        spellDuration = _spellDuration;
        StartCoroutine(WaitForDestroy());
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != caster)
        {
            if(collision.GetComponent<IDamageable>() != null)
            {
                collision.GetComponent<IDamageable>().Stun();
                collision.GetComponent<IDamageable>().Knockback(repellForce*(rb.velocity.normalized));
            }
            
        }
    }
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(spellDuration);
        Destroy(gameObject);
    }
}
