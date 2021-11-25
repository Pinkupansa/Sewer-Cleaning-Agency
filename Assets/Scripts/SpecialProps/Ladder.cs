using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractableObject
{
    public InteractionEventArgs Info()
    {
        return new InteractionEventArgs(InteractionType.ClimbLadder, this, transform.position);
    }

    public void Interact()
    {
        
    }

    public KeyCode InteractionKey()
    {
        return KeyCode.Z;
    }

    public string Name()
    {
        return "Ladder";
    }
}
