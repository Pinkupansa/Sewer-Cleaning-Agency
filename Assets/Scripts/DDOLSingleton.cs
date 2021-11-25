using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Put on a singleton that shouldn't be destroyed between scenes
public class DDOLSingleton : MonoBehaviour
{
    public static List<int> alreadyInScene = new List<int>();
    [SerializeField]
    int id;
    
    void Awake()
    {
        if (alreadyInScene.Contains(id))
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        alreadyInScene.Add(id);
    }
}
    
