using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Manages all combat and special gun methods
public class PlayerCombat : MonoBehaviour
{
    //Point from which attacks are made
    private Transform caster;

    private Player player;
    private float cooldown;

    //If a damageable object is hit
    public UnityEvent HitEvent;

    private bool isGrappling;

    private LineRenderer grapplingRope;

    private Transform grappledObject;
    private void Awake()
    {
        
    }
    private void Start()
    {
        player = GetComponent<Player>();
        player.Attacked.AddListener(Attack);
        player.WeaponChanged.AddListener(OnWeaponChanged);
        OnWeaponChanged();
        
    }
    private void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            switch (player.WeaponInHand)
            {
                case (WeaponInHand.Melee):
                    GetComponentInChildren<Animator>().SetTrigger("AttackOne");
                    break;
                case (WeaponInHand.Range):
                    Attack();
                    break;
            }
        }
        if (isGrappling)
        {
            grapplingRope.SetPosition(0, (Vector2)transform.position);
            if(grappledObject != null)
            {
                grapplingRope.SetPosition(1, (Vector2)grappledObject.position);
            }
            
            if (Input.GetButtonDown("Fire1"))
            {
                isGrappling = false;
                Destroy(gameObject.GetComponent<SpringJoint2D>());
                Destroy(grapplingRope);
                grappledObject = null;
            }
        }
        ISkill weaponSkill = player.GetCurrentWeapon().ItemInGame.GetComponent<ISkill>();
        if(weaponSkill != null)
        {
            if (Input.GetButtonDown(weaponSkill.UsingButton()))
            {
                
                weaponSkill.Use(((caster.position.x - transform.position.x)*Vector2.right).normalized, caster);
                Debug.Log("Skill used" + weaponSkill.Name());
            }
        }
    }
    void OnWeaponChanged()
    {
        Weapon w = new Weapon();
        switch (player.WeaponInHand)
        {
            case WeaponInHand.Melee:
                w = player.GetMeleeWeapon();
                break;
            case WeaponInHand.Range:
                w = player.GetRangeWeapon();
                break;
        }
        if(w != null)
        {
            //Sets the caster
            StartCoroutine(WaitBeforeSettingCaster());
        }
        
    }

    //Decides what to do according to the weapon in hand
    void Attack()
    {
        if(cooldown <= 0 && !isGrappling)
        {
            switch (player.WeaponInHand)
            {
                case WeaponInHand.Range:
                    if (player.GetRangeWeapon() != null)
                    {

                        Shoot();
                        cooldown = player.GetRangeWeapon().Cooldown;
                    }
                    break;

                case WeaponInHand.Melee:
                    if (player.GetMeleeWeapon() != null)
                    {
                        MeleeAttack();
                        cooldown = player.GetMeleeWeapon().Cooldown;
                    }
                    break;
            }
            
        }
        
               
        
    }
    
    void Shoot()
    {

        if (caster != null)
        {
            //Instance of the projectile
            GameObject bulletInstance = Instantiate(player.GetRangeWeapon().Bullets, caster.position, Quaternion.identity);
            Projectile projectile = bulletInstance.AddComponent<Projectile>();
            projectile.SetParameters(CalculateDamage(player.GetRangeWeapon().Damage), player.GetRangeWeapon().BulletSpeed, player.GetRangeWeapon().BulletsUseGravity, (MousePointer.instance.transform.position - caster.position).normalized, GetComponent<Rigidbody2D>().velocity, gameObject);
            if (player.GetRangeWeapon().IsGrapplingGun)
            {
                GetComponent<PlayerMovement>().SetCanMove(false);
                projectile.OnHit.AddListener(Grapple);
                bulletInstance.GetComponent<GrapplingRope>().SetParameters(player.GetRangeWeapon().GrapplingRopeMaterial, caster, player.GetRangeWeapon().GrapplingRopeWidth);
            }
        }
        
        
    }
    void Grapple(Vector2 position, Rigidbody2D objectToGrapple)
    {
        
        GetComponent<PlayerMovement>().SetCanMove(true);
        if (!isGrappling)
        {
            SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();

            joint.autoConfigureConnectedAnchor = false;
            joint.autoConfigureDistance = false;
            joint.distance = 1f;
            
            joint.dampingRatio = 10f;
            joint.frequency = 1f;
            joint.enableCollision = true;
            grapplingRope = gameObject.AddComponent<LineRenderer>();

            if(objectToGrapple != null)
            {
                joint.connectedBody = objectToGrapple;
                grapplingRope.SetPosition(1, objectToGrapple.position);
                grappledObject = objectToGrapple.transform;
            }
            else
            {
                joint.connectedAnchor = position;
                grapplingRope.SetPosition(1, position);
                grappledObject = null;
            }
            
            
            //lR.widthCurve = new AnimationCurve(new Keyframe[1] { new Keyframe(0,player.GetMeleeWeapon().GrapplingRopeWidth) });
            grapplingRope.positionCount = 2;
            grapplingRope.startWidth = player.GetRangeWeapon().GrapplingRopeWidth;
            
           
            grapplingRope.sharedMaterial = player.GetRangeWeapon().GrapplingRopeMaterial;

            
            isGrappling = true;
        }
        
    }
    void MeleeAttack()
    {
        
            Collider2D[] _hit = Physics2D.OverlapBoxAll(caster.position, Vector2.one*player.GetMeleeWeapon().Range,0);

            if(_hit != null)
            {
                for (int i = 0; i < _hit.Length; i++)
                {
                    try
                    {
                        if (_hit[i].gameObject != gameObject && _hit[i].gameObject.GetComponent<IDamageable>() != null)
                        {
                            IDamageable damageable = _hit[i].gameObject.GetComponent<IDamageable>();
                            HitEvent.Invoke();
                            damageable.TakeDamage(CalculateDamage(player.GetMeleeWeapon().Damage),(caster.position - transform.position).normalized * 100f);
                            break;
                        }

                    }
                    catch
                    {
                        
                        continue;
                    }
                }
            }
            
        
        
    }
    float CalculateDamage(float damage)
    {
        return Mathf.Round(damage + Random.Range(-damage/3f,damage/3f));
    }
    //Il faut attendre que les paramètres de l'arme soient reglés avant de set le caster
    IEnumerator WaitBeforeSettingCaster()
    {
        yield return new WaitForSeconds(0.5f);


        caster = CustomUtilities.TransformFind(transform, "Caster");
        if(caster == null)
        {
            Debug.LogError("NO CASTER");
        }
            
        
    }

}
