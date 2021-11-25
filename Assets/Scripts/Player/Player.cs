using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public delegate void InventoryEventHandler();
public delegate void PlayerEventHandler();


public enum WeaponInHand { Melee, Range }

public class Player : MonoBehaviour, IDamageable
{
    public UnityEvent InventoryChanged;
    public UnityEvent WeaponChanged;
    public UnityEvent TookDamage;
    public UnityEvent Attacked;

    private PlayerInteractor playerInteractor;


    private WeaponInHand weaponInHand = WeaponInHand.Melee;

    [SerializeField] private float jumpForce;
    
    [SerializeField] private float runSpeed;

    [SerializeField] private float climbSpeed;
    
    [SerializeField] private float maxHealth;
    
    [SerializeField] private float currentHealth;

    [SerializeField] private PlayerInventory inventory = new PlayerInventory();
    
    private bool knockback;
    private Vector2 knockbackForce;
    public void Start()
    {
        playerInteractor = GetComponent<PlayerInteractor>();
        playerInteractor.Interacted += OnInteraction;
        weaponInHand = WeaponInHand.Melee;
        WeaponChanged?.Invoke();
        InventoryChanged?.Invoke();
        GetComponentInChildren<AnimationEventManager>().AttackDone += Attack;

    }

    #region Getters
    public WeaponInHand WeaponInHand
    {
        get
        {
            return weaponInHand;
        }
    }

    public float RunSpeed 
    {
        get
        {
            return runSpeed;
        }
    }

    public float ClimbSpeed
    {
        get
        {
            return climbSpeed;
        }
    }
    public float JumpForce
    {
        get
        {
            return jumpForce;
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
    #endregion

    public void DropWeapon(int weaponToDrop)
    {
        switch (weaponToDrop)
        {
            case 0:
                //Drop the loot
                Instantiate(ItemDatabase.instance.ProvideLoot(inventory.MeleeWeapon.Name), transform.position, Quaternion.identity);
                //Remove it from the inventory
                inventory.DropMeleeWeapon();

                break;
            case 1:
                Instantiate(ItemDatabase.instance.ProvideLoot(inventory.RangeWeapon.Name), transform.position, Quaternion.identity);
                inventory.DropRangeWeapon();

                break;
        }
        WeaponChanged?.Invoke();
        InventoryChanged?.Invoke();
    }
    public Weapon GetCurrentWeapon()
    {
        switch (weaponInHand)
        {
            case (WeaponInHand.Melee):
                return GetMeleeWeapon();
            case (WeaponInHand.Range):
                return GetRangeWeapon();
            default:
                return null;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }

    }
    //Updates the weaponinhand enum with the given int
    public void SwitchWeapon(int weapon)
    {
        if (weapon == 0)
        {
            weaponInHand = WeaponInHand.Melee;
        }
        if (weapon == 1)
        {
            weaponInHand = WeaponInHand.Range;
        }
        WeaponChanged?.Invoke();
    }

    void OnInteraction(InteractionEventArgs info)
    {
        //Equipping an object
        if (info.InteractionType == InteractionType.EquipObject)
        {
            Loot loot = info.Interactor as Loot;

            EquipItem(ItemDatabase.instance.ProvideItem(loot.ItemName));
        }
    }
    public void EquipItem(Item item)
    {


        Weapon w = item as Weapon;
        //Is it is a weapon
        if (w != null)
        {
            //Checks if the weapon type is the same that the one in hand, and equips the weapon
            switch (w.Type)
            {
                case (WeaponType.Melee):
                    if (inventory.MeleeWeapon != null)
                    {
                        DropWeapon(0);
                    }
                    inventory.EquipMeleeWeapon(w);

                    InventoryChanged?.Invoke();
                    if (weaponInHand == WeaponInHand.Melee)
                    {
                        WeaponChanged?.Invoke();
                    }
                    return;
                case (WeaponType.Range):
                    if (inventory.RangeWeapon != null)
                    {
                        DropWeapon(1);
                    }

                    inventory.EquipRangeWeapon(w);

                    InventoryChanged?.Invoke();
                    if (weaponInHand == WeaponInHand.Range)
                    {
                        WeaponChanged?.Invoke();
                    }
                    return;
            }
        }

    }

    //Communication with the inventory
    public Weapon GetMeleeWeapon()
    {
        return inventory.MeleeWeapon;
    }
    public Weapon GetRangeWeapon()
    {
        return inventory.RangeWeapon;
    }
    public int GetNumberOfPotions()
    {
        return inventory.NumberOfPotions;
    }

    public void Attack()
    {
        Attacked.Invoke();
    }


    public void TakeDamage(float damage, Vector2 repulsion)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        TookDamage?.Invoke();
    }


    //Inventory
    [System.Serializable]
    class PlayerInventory
    {
        [SerializeField] private Weapon meleeWeapon;
        [SerializeField] private Weapon rangeWeapon;
        private int numberOfPotions;

        #region Getters
        public Weapon MeleeWeapon
        {
            get
            {
                return meleeWeapon;
            }
        }
        public Weapon RangeWeapon
        {
            get
            {
                return rangeWeapon;
            }
        }
        public int NumberOfPotions
        {
            get
            {
                return numberOfPotions;
            }
        }
        #endregion

        public void EquipMeleeWeapon(Weapon w)
        {
            if (w.Type == WeaponType.Melee)
            {
                meleeWeapon = w;
            }
        }
        public void EquipRangeWeapon(Weapon w)
        {
            if (w.Type == WeaponType.Range)
            {
                rangeWeapon = w;
            }
        }
        public void DropMeleeWeapon()
        {
            meleeWeapon = null;
        }
        public void DropRangeWeapon()
        {
            rangeWeapon = null;
        }

    }
    public void Knockback(Vector2 force)
    {
        knockback = true;
        knockbackForce = force;
    }
    private void FixedUpdate()
    {
        if(knockback)
        {
            GetComponent<Rigidbody2D>().AddForce(knockbackForce);
            knockback = false;
        }
    }
    public void Stun()
    {
        
    }
}
