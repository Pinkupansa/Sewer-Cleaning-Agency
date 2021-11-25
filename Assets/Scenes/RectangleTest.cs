using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleTest : MonoBehaviour
{
    List<Rectangle> rectangles;
    // Start is called before the first frame update
    void Start()
    {
        rectangles = new List<Rectangle>();
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Rectangle r1 = new Rectangle(0,0,10,10);
        Rectangle r2 = new Rectangle(5,5,15,15);
        Gizmos.DrawWireCube(r1.Center, new Vector3(r1.Size.x, r1.Size.y, 1));
        Gizmos.DrawWireCube(r2.Center, new Vector3(r2.Size.x, r2.Size.y, 1));

        Gizmos.color = Color.red;
        Rectangle intersection = CustomUtilities.Intersection(r1,r2);
        Gizmos.DrawCube(intersection.Center, new Vector3(intersection.Size.x, intersection.Size.y, 1));

        

    }
}
