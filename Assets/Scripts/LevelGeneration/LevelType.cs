using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to store data on a level "style"
[CreateAssetMenu(fileName = "LevelType", menuName = "ScriptableObjects/LevelType", order = 1)]
public class LevelType : ScriptableObject
{
    public int levelWidth;
    public int levelHeight;
    public GameObject doorPrefab;
    public RoomDatabase rDB;
}
