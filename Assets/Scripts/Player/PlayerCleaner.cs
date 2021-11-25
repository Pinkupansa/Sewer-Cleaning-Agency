using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to clean ICleanableObjects 
public class PlayerCleaner : MonoBehaviour
{
    float cleanSpeed = 3f;
    void Start()
    {
        GetComponent<PlayerInteractor>().Interacted += OnInteraction;
    }

    void OnInteraction(InteractionEventArgs args)
    {
        if(args.InteractionType == InteractionType.CleanObject)
        {
            
            ICleanableObject objectToClean = args.Interactor as ICleanableObject;
            if(objectToClean != null)
            {
                StartCoroutine(CleanObject(objectToClean));    
            }
        }
    }
    IEnumerator CleanObject(ICleanableObject objectToClean)
    {
        if(Input.GetKey(objectToClean.InteractionKey())){
            objectToClean.Clean(1f);
            yield return new WaitForSeconds(1f/cleanSpeed);
            StartCoroutine(CleanObject(objectToClean));
        }
        
    }
}
