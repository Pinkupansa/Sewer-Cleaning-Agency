using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for object that can be cleaned using a broom
public interface ICleanableObject : IInteractableObject
{
    //"Health" of the object"
    float CurrentDirtyness();
    float MaxDirtyness();

    //Method to call on the object when we're cleaning it
    void Clean(float amount);
}
