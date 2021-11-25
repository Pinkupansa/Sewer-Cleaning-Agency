using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileType", menuName = "ScriptableObjects/TileType", order = 1)]
public class TileType : ScriptableObject
{
    public int id;
    [SerializeField]
    Sprite topTile, topRightTile, rightTile, bottomRightTile, bottomTile, bottomLeftTile, leftTile, topLeftTile, fullTile, emptyTile;

    public Sprite GetTile(bool top, bool bottom, bool right, bool left)
    {
        (bool, bool, bool, bool) TBRL = (top, bottom, right, left);
        if (TBRL == (false, false, false, false))
        {
            return fullTile;
        }
        else if (TBRL == (false, false, false, true))
        {
            return rightTile;
        }
        else if (TBRL == (false, false, true, false))
        {
            return leftTile;
        }
        else if (TBRL == (false, false, true, true))
        {
            return topTile;
        }
        else if (TBRL == (false, true, false, false))
        {
            return topTile;
        }
        else if (TBRL == (false, true, false, true))
        {
            return topRightTile;
        }
        else if (TBRL == (false, true, true, false))
        {
            return topLeftTile;
        }
        else if (TBRL == (false, true, true, true))
        {
            return topTile;
        }
        else if (TBRL == (true, false, false, false))
        {
            return bottomTile;
        }
        else if (TBRL == (true, false, false, true))
        {
            return bottomRightTile;
        }
        else if (TBRL == (true, false, true, false))
        {
            return bottomLeftTile;
        }
        else if (TBRL == (true, false, true, true))
        {
            return bottomTile;
        }
        else if (TBRL == (true, true, false, false))
        {
            return rightTile;
        }
        else if(TBRL == (true, true, false, true))
        {
            return rightTile;
        }
        else if(TBRL == (true, true, true, false))
        {
            return leftTile;
        }
        else
        {
            return emptyTile;
        }
             
    }
}
