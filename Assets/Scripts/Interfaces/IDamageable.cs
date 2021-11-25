using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for objects that can take damage
public interface IDamageable
{
    void TakeDamage(float damage, Vector2 repulsion);
    void Knockback(Vector2 kbForce);
    void Stun();
}
