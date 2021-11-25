using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Holds items available for the game, makes the link between an object and his name
public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private GameObject[] loots;
    
    public Item ProvideItem(string name)
    {
        return weapons.Where(x => x.Name == name).FirstOrDefault();
    }
    public GameObject ProvideLoot(string name)
    {
        return loots.Where(x => x.GetComponent<Loot>().ItemName == name).FirstOrDefault();
    }
    public GameObject RandomLoot()
    {
        return loots[Random.Range(0, loots.Length)];
    }
}

