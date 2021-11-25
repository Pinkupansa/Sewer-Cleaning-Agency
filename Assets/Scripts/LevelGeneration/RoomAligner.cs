using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomAlignmentMode{Top,Right,Down,Left}
public class RoomAligner : MonoBehaviour
{
    public Transform top, right, down, left;
    private Room room;
    public void Set(RoomAligner parent, RoomAlignmentMode alignmentMode)
    {
        switch(alignmentMode)
        {
            case RoomAlignmentMode.Top:
                transform.position = parent.down.position + (transform.position - top.position);
                break;
            case RoomAlignmentMode.Down:
                transform.position = parent.top.position + (transform.position - down.position);
                break;
            case RoomAlignmentMode.Right:
                transform.position = parent.left.position + (transform.position - right.position);
                break;
            case RoomAlignmentMode.Left:
                transform.position = parent.right.position + (transform.position - left.position);
                break;
        }
    }
    public Room Room
    {
        get
        {
            return room;
        }
    }
}
