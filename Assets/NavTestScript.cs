using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTestScript : MonoBehaviour
{
    Navmesh nvm;
    Path path = new Path(new List<Navlink>());
    private void Start()
    {
       
        nvm =  GetComponent<Navmesh>();
        foreach(Transform t in transform)
        {
            nvm.AddNavpoint(t, NavpointType.Platform);
        }
        foreach(Navpoint n1 in nvm.Navpoints)
        {
            foreach(Navpoint n2 in nvm.Navpoints)
            {
                if(Random.Range(0,5) == 0)
                {
                    nvm.AddLink(n1, n2, false);
                    nvm.AddLink(n2, n1, false);
                }
            }
        }
    }
    
    public void FindPath()
    {
        int i = Random.Range(0, nvm.Navpoints.Count);
        int j = Random.Range(0, nvm.Navpoints.Count);
        
        path = PathFinder.FindPath(nvm.Navpoints[i], nvm.Navpoints[j], nvm);

    }


}
