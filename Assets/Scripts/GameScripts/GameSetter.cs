using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Initializes the level

public class GameSetter : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;
    
    GameObject camera;
    private void Start()
    {
        GameEvents.current.levelGenerated.AddListener(OnLevelGenerated);
        OnLevelGenerated();
    }
    public void OnLevelGenerated()
    {   
        if(LevelGenerator.current.levelGenerated)
        {
            camera = GameObject.Find("Main Camera");
            //GameObject playerInstance = Instantiate(playerPrefab, LevelGenerator.current.GetSpawnPoint(), Quaternion.identity);
            //camera.GetComponent<FollowTween>().target = playerInstance.transform ;
            //GameEvents.current.PlayerInstantiated();
        }
        
    }
}
