using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
//All Events except internal player logic
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    bool firstFrame = true;
    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        
    }
    
    public UnityEvent levelGenerated;
    public void OnLevelGenerated()
    {
        levelGenerated.Invoke();
    }
    public UnityEvent onPlayerInstantiated;
    public void PlayerInstantiated()
    {
        onPlayerInstantiated.Invoke();
    }
    public event Action onEndDoorEnter;
    
    public void EndDoorEnter()
    {
        onEndDoorEnter();
    }
    public event Action<InteractionEventArgs> onPlayerEncounteredInteractor;
    public void PlayerEncounteredInteractor(InteractionEventArgs args)
    {
        
         onPlayerEncounteredInteractor(args);
        
    }
    public event Action onPlayerLeftInteractor;
    public void PlayerLeftInteractor(InteractionEventArgs args)
    {
        onPlayerLeftInteractor();
    }

    public event Action<bool> onLevelEnded;
    public void UIButtonClicked(string button)
    {
        switch (button)
        {
            case "EndDoorMenu_GoDown":
                onLevelEnded(false);
                break;
            case "EndDoorMenu_GoUp":
                onLevelEnded(true);
                break; 
        }
    }
    public event Action<GameObject, float> onEnemyTookDamage;
    public void EnemyTookDamage(GameObject enemy, float damage)
    {
        onEnemyTookDamage(enemy, damage);
    }
    
}
