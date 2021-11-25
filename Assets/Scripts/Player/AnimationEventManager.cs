using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void AttackEvent();
//Used on the sprite to communicate animation events to the player
public class AnimationEventManager : MonoBehaviour
{
    public AttackEvent AttackDone;
    public UnityEvent Stepped;
    public void Attack()
    {
        
        AttackDone?.Invoke();
    }
    public void Step()
    {
        Stepped.Invoke();
    }
}
