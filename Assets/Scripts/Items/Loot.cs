using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script for pickable items on ground
[RequireComponent(typeof(BoxCollider2D))]
public class Loot : MonoBehaviour, IInteractableObject
{
    [SerializeField]
    string itemName;
    public void Interact()
    {
        Destroy(gameObject);
    }
    public string ItemName
    {
        get
        {
            return itemName;
        }
    }
    public string Name()
    {
        return itemName;
    }
    public KeyCode InteractionKey()
    {
        return KeyCode.E;
    }
    public InteractionEventArgs Info()
    {
        return new InteractionEventArgs(InteractionType.EquipObject, this, transform.position);
    }
    
}
