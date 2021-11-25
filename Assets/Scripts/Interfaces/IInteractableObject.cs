using UnityEngine;

//Communication interface for interactions
public interface IInteractableObject
{
    void Interact();
    
    KeyCode InteractionKey();
   
    InteractionEventArgs Info();
    string Name();
}

public enum InteractionType { EquipObject, ClimbLadder, OpenDoor, CleanObject}

//Struct carrying all infos for an interaction event
public struct InteractionEventArgs
{

    InteractionType interactionType;
    IInteractableObject interactor;
    Vector2 interactorPosition;

    public InteractionEventArgs(InteractionType interactionType, IInteractableObject interactor, Vector2 interactorPosition)
    {
        this.interactionType = interactionType;
        this.interactor = interactor;
        this.interactorPosition = interactorPosition;
    }
    public InteractionType InteractionType
    {
        get
        {
            return interactionType;
        }
    }
    public IInteractableObject Interactor
    {
        get
        {
            return interactor;
        }

    }
    public Vector2 InteractorPosition
    {
        get
        {
            return interactorPosition;
        }
    }
}