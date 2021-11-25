using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    TileType tileType;
    float extent;
    [SerializeField]
    Transform top, right, bottom, left;
    bool isFirstFrame = true;
    private void Update()
    {
        if (isFirstFrame)
        {
            CheckNeighborhood(true);
            isFirstFrame = false;
        }
        
    }
    private void OnDestroy()
    {
        //CheckNeighborhood(true);
    }
    public void CheckNeighborhood(bool birthOrDeath)
    {
        
        bool _top = CheckNeighbour(top.position, birthOrDeath);
       
        bool _right = CheckNeighbour(right.position, birthOrDeath);

        bool _bottom = CheckNeighbour(bottom.position, birthOrDeath);

        bool _left = CheckNeighbour(left.position, birthOrDeath);
        
        GetComponent<SpriteRenderer>().sprite = tileType.GetTile(_top, _bottom, _right, _left);
    }
    bool CheckNeighbour(Vector2 pos, bool birthOrDeath)
    {
        Collider2D coll = Physics2D.OverlapBox(pos, Vector2.one * 0.1f, 0);
        if(coll != null)
        {
            
            Tile t = coll.GetComponent<Tile>();
            if (t != null)
            {
                if(t.GetTileType.id == tileType.id)
                {
                    if (birthOrDeath)
                    {

                        t.CheckNeighborhood(false);
                        
                    }

                    return true;
                }
            }
        }
        
        return false;
    }
    TileType GetTileType
    {
        get
        {
            return tileType;
        }
    }
}
