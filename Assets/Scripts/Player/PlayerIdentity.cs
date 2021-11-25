using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton to get the player object from other objects
public class PlayerIdentity : MonoBehaviour
{
    #region Singleton

    public static PlayerIdentity instance;
    private void Awake()
    {
        if(instance == null)
        {
            player = gameObject;
            instance = this;
        }
        
    }
    private void Start()
    {
        if(GameEvents.current != null)
        {
            
            //GameEvents.current.PlayerInstantiated();
        }
        
    }
    #endregion
    public GameObject player;
}
