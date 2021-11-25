using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InteractionEnterEventHandler(InteractionEventArgs args);
public delegate void InteractionExitEventHandler(InteractionEventArgs args);
public delegate void InteractionEventHandler(InteractionEventArgs args);

//Manages the interactions with InteractableObjects
public class PlayerInteractor : MonoBehaviour
{
    public InteractionEnterEventHandler InteractorEncountered;
    public InteractionExitEventHandler InteractorLeft;
    public InteractionEventHandler Interacted;

    //Interactor (last encountered and not left)
    private IInteractableObject currentInteractor;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractableObject interactor = collision.GetComponent<IInteractableObject>();
        if (interactor != null)
        {
            //When we encounter an interactor
            currentInteractor = interactor;
            InteractorEncountered?.Invoke(currentInteractor.Info());
            if(GameEvents.current != null)
            {
                GameEvents.current.PlayerEncounteredInteractor(currentInteractor.Info());
            }
           
        }
    }
    
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractableObject interactor = collision.GetComponent<IInteractableObject>();
        if(interactor != null)
        {
            //When we leave an interactor
            if (interactor == currentInteractor)
            {
                InteractorLeft?.Invoke(interactor.Info());
                if(GameEvents.current != null)
                {
                    GameEvents.current.PlayerLeftInteractor(currentInteractor.Info());
                }
                
                currentInteractor = null;

            }
        }
        
    }
    private void Update()
    {
        if(currentInteractor != null)
        {
            
            if (Input.GetKeyDown(currentInteractor.InteractionKey()))
            {
                Interacted?.Invoke(currentInteractor.Info());
                currentInteractor.Interact();
            }
            
        }
    }

}


