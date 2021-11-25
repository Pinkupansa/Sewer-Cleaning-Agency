using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {Melee, Range}
[System.Serializable]
[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Item/Weapon", order = 1)]
public class Weapon : Item
{
    [Header("General weapon settings")]
    [SerializeField]
    float damages;


    [SerializeField]
    float cooldown;

    [SerializeField]
    bool twoHanded;

    [SerializeField]
    AudioClip[] attackSounds;

    [SerializeField]
    AudioClip[] hitSounds;

    [SerializeField]
    WeaponType weaponType;

    [Header("Melee weapon settings")]
    //If the weapon is a melee weapon
    [SerializeField]
    float range;
    [Header("Range weapon settings")]
    //If the weapon is a range weapon
    [SerializeField]
    GameObject bullets;

    [SerializeField]
    bool bulletsUseGravity;

    [SerializeField]
    float bulletSpeed;


    [SerializeField]
    bool isGrapplingGun;

    [Header("Grappling gun settings")]
    [SerializeField]
    Material grapplingRopeMaterial;
    [SerializeField]
    float grapplingRopeWidth;
    public float Damage
    {
        get
        {
            return damages;
        }
    }
    public bool TwoHanded
    {
        get
        {
            return twoHanded;
        }
    }
    public float BulletSpeed
    {
        get
        {
            return bulletSpeed;
        }
    }
    public GameObject Bullets
    {
        get
        {
            return bullets;
        }
    }
    public bool BulletsUseGravity
    {
        get
        {
            return bulletsUseGravity;
        }
    }
    public WeaponType Type
    {
        get
        {
            return weaponType;
        }
    }
    public float Cooldown
    {
        get
        {
            return cooldown;
        }
    }
    public float Range
    {
        get
        {
            return range;
        }
    }
    public AudioClip AttackSound(int i)
    {
        return attackSounds[i % attackSounds.Length];
    }
    public AudioClip HitSound(int i)
    {
        return hitSounds[i % hitSounds.Length];
    }
    public bool IsGrapplingGun
    {
        get
        {
            return isGrapplingGun;
        }
       
    }
    public Material GrapplingRopeMaterial
    {
        get
        {
            return grapplingRopeMaterial;
        }
    }
    public float GrapplingRopeWidth
    {
        get
        {
            return grapplingRopeWidth;
        }
    }

}

