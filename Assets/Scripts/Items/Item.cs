using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    [SerializeField]
    string name;
    [SerializeField]
    string description;
    [SerializeField]
    Sprite spriteInInventory;
    [SerializeField]
    GameObject itemInGame;

   

    public Sprite SpriteInInventory
    {
        get
        {
            return spriteInInventory;
        }
    }
    public GameObject ItemInGame
    {
        get
        {
            return itemInGame;
        }
    }
    public string Name
    {
        get
        {
            return name;
        }
    }
    public string Description
    {
        get
        {
            return description;
        }
    }

}