using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
//Manages the appearance of the character and the weapons in its hands (animations, instantiating weapons)
public class PlayerGraphics : MonoBehaviour
{
    private Player player;
    [SerializeField] Transform playerGraphics, rightHand, leftHand, rightArm, leftArm;
    private float attackCounterReset = 1f;
    private float attackCounterResetTimer;
    private Animator anim;
    private GameObject objectInHand;
    private bool objectInHandIsTwoHanded;
    private Transform mouseObject;

    [SerializeField] private MultiAimConstraint rightArmConstraint, leftArmConstraint, headConstraint;
   
    public void Start()
    {
        mouseObject = MousePointer.instance.transform;
        player = GetComponent<Player>();
        player.WeaponChanged.AddListener(OnWeaponChanged);
        player.TookDamage.AddListener(AnimateDamage);
        GetComponent<CharacterController2D>().OnLandEvent.AddListener(OnLand);
        anim = GetComponentInChildren<Animator>();
        OnWeaponChanged();
        attackCounterResetTimer = 0;
        
    }

    private void Update()
    {
        if(attackCounterResetTimer > 0)
        {
            attackCounterResetTimer -= Time.deltaTime;
        }
        else
        {
            ResetAttackCounter();
        }
        if (objectInHandIsTwoHanded && !anim.GetBool("isJumping"))
        {
            objectInHand.transform.up = (rightHand.position - leftHand.position).normalized;
        }

        

    }
    
    void ResetAttackCounter()
    {
       // anim.SetInteger("AttackCounter", 0);
    }
    void OnWeaponChanged()
    {
        ResetSlots();
        SetStance();
    }

    void ResetSlots()
    {
        ClearSlots();
        FillSlots();
    }
    void FillSlots()
    {
        GameObject weaponToInstantiate = null;
        objectInHandIsTwoHanded = false;
        Weapon w = player.GetCurrentWeapon();
        if (w != null)
        {
            weaponToInstantiate = w.ItemInGame;
            objectInHandIsTwoHanded = w.TwoHanded;
            if (weaponToInstantiate != null)
            {
                GameObject instance = Instantiate(weaponToInstantiate, rightHand);
               
                instance.transform.position = rightHand.position;
                objectInHand = instance;
            }
            else
            {
                Debug.LogError("The object in hand doesn't have graphics");
            }
        }
        else
        {
            Debug.LogError("The player doesn't carry an object");
        }
        
        
        
    }
    void OnLand()
    {
        SetStance();
    }

    void AnimateDamage()
    {
        anim.SetTrigger("Hurt");
    }
    void ClearSlots()
    {
        if(rightHand.childCount > 0)
        {
            Destroy(rightHand.GetChild(0).gameObject);
        }
    }
   
    public void AnimateJump(bool b)
    {
        ResetAttackCounter();
        GetComponentInChildren<Rig>().weight = 0;
        anim.SetBool("isJumping", b);
        
    }
    public void AnimateClimb(bool b)
    {
        anim.SetBool("isClimbing", b);
    }
    public void SetSpeed(float speed)
    {
        anim.SetFloat("Speed", speed);
    }
    void SetStance()
    {
        Weapon w = player.GetCurrentWeapon();
        if(w != null)
        {
            anim.SetBool("TwoHanded", w.TwoHanded);
            anim.SetBool("EmptyHand", false);
        }
        else
        {
            anim.SetBool("TwoHanded", false);
            anim.SetBool("EmptyHand", true);
            GetComponentInChildren<Rig>().weight = 0;
        }
        if(w.Type == WeaponType.Melee)
        {
            GetComponentInChildren<Rig>().weight = 0;
        }
        else
        {
            GetComponentInChildren<Rig>().weight = 1;
        }
    }
    public void AnimateAttack()
    {
        anim.Play("ShittyHenry_TwoHandedAttack");

    }
}
