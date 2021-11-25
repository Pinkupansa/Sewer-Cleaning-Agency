using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Holds general progression data on the game
public class GameData : MonoBehaviour
{
    public static GameData current;
    [SerializeField]
    int level = -20;
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
    }
    private void Start()
    {
        GameEvents.current.onLevelEnded += OnLevelEnded;
    }

    private void OnLevelEnded(bool nextLevel)
    {
        if (nextLevel)
        {
            LevelUp();
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }
    public void LevelUp()
    {
        level++;
    }
    
    
}
