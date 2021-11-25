using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Manages the HUD
public class PlayerUIManager : MonoBehaviour
{
    
    private Player player;
    private float fluidity = 100f;

    #region InventoryHUD
    [SerializeField] private Sprite meleeWeaponEmptySprite, rangeWeaponEmptySprite, potionEmptySprite;
    [SerializeField] private Transform meleeWeaponSlot,rangeWeaponSlot, potionsSlot, healthBar;
    #endregion

    public void Start()
    {
        player = GetComponent<Player>();
        player.InventoryChanged.AddListener(OnItemEquipped);
        player.TookDamage.AddListener(OnDamageTaken);
        ResetSlots();
        
    }
    #region Stats
    void OnDamageTaken()
    {
        healthBar.localScale = new Vector3(player.CurrentHealth / player.MaxHealth, healthBar.localScale.y, healthBar.localScale.z);
    }
    
    #endregion

    #region Inventory
    public void OnItemEquipped()
    {
        ResetSlots();
    }
    private void ResetSlots()
    {
        Weapon meleeWeapon = player.GetMeleeWeapon();
        Weapon rangeWeapon = player.GetRangeWeapon();
        int numberOfPotions = player.GetNumberOfPotions();
        if (meleeWeapon != null)
        {
            meleeWeaponSlot.GetComponent<Image>().sprite = meleeWeapon.SpriteInInventory;
        }
        else
        {
            meleeWeaponSlot.GetComponent<Image>().sprite = meleeWeaponEmptySprite;
        }
        if(rangeWeapon != null)
        {
            rangeWeaponSlot.GetComponent<Image>().sprite = rangeWeapon.SpriteInInventory;
        }
        else
        {
            rangeWeaponSlot.GetComponent<Image>().sprite = rangeWeaponEmptySprite;
        }
        potionsSlot.GetComponent<Image>().sprite = potionEmptySprite;

    }
    #endregion
}
