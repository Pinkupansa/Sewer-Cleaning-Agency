using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WindBlast : MonoBehaviour, ISkill
{
    [SerializeField] private GameObject windParticleEffect;
    [SerializeField] private GameObject windBlastProjectilePrefab;
    [SerializeField] private float repellForce = 1000f;

    [SerializeField] private float spellDuration;
    public string Description()
    {
        return "Blows away all enemies in front of you";
    }

    public string Name()
    {
        return "Wind Blast";
    }

    public void Use(Vector2 direction, Transform caster)
    {

        RaycastHit2D[] _hit = Physics2D.BoxCastAll(caster.position, 5f* Vector2.one,0, direction,20f);
       
        GameObject partInstance = Instantiate(windParticleEffect, caster.position, Quaternion.identity);
        partInstance.transform.localScale = Vector3.one*direction.x;
        
        ParticleSystem parts = partInstance.GetComponent<ParticleSystem>();
        float totalDuration = parts.duration + parts.startLifetime;
        Destroy(partInstance, totalDuration);
        /*for (int i = 0; i < _hit.Length; i++)
        {
            Debug.Log(_hit[i].collider.name);
            
            if (_hit[i].collider.GetComponent<IDamageable>() != null)
            {
                _hit[i].collider.GetComponent<IDamageable>().Knockback(repellForce*direction);                
            }
        }*/
        GameObject projectile = Instantiate(windBlastProjectilePrefab, caster.position,Quaternion.identity);
        Debug.Log(repellForce);
        projectile.GetComponent<WindBlastProjectile>().Set(repellForce, spellDuration, gameObject, direction/10f);
    }

    public string UsingButton()
    {
        return "Fire2";
    }
}
