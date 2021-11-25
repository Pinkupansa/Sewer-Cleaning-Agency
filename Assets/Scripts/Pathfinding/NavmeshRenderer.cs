using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavmeshRenderer : MonoBehaviour
{
    const float POINT_SIZE = 3F;

    private void OnDrawGizmos()
    {

        INavmeshHolder navHold = GetComponent<INavmeshHolder>();
        if(navHold != null)
        {
            RenderNavmesh(navHold.GetNavmesh());
        }
        
    }
    private void RenderNavmesh(Navmesh navmesh)
    {
        
        if(navmesh != null)
        {
            
            foreach(Navpoint n in navmesh.Navpoints)
            {
                if(n.Type == NavpointType.Platform)
                {
                    Gizmos.color = Color.cyan;
                }
                if(n.Type == NavpointType.Ladder)
                {
                    Gizmos.color = Color.gray;
                }
                Gizmos.DrawCube(n.Transform.position,Vector3.one * 3f);
            }
            foreach(Navlink n in navmesh.Navlinks)
            {
                if(n.IsJumpLink)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawLine(n.Start.Transform.position, n.End.Transform.position);
            }
        }
    }

}

public interface INavmeshHolder
{
    Navmesh GetNavmesh();
}