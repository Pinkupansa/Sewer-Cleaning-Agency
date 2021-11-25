using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractableObject
{
    public void Interact(){
        GameEvents.current.EndDoorEnter();
    }
    public KeyCode InteractionKey(){
        return KeyCode.E;
    }
    public InteractionEventArgs Info(){
        return new InteractionEventArgs(InteractionType.OpenDoor,this,transform.position);
    }
    public string Name(){
        return "Door";
    }
}
