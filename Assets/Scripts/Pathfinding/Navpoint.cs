
using UnityEngine;

public enum NavpointType {None, Platform, Ladder}

[System.Serializable]
public class Navpoint 
{
    [SerializeField] private int id;
    [SerializeField] private Transform transform;
    [SerializeField] private NavpointType type;
    
    public Navpoint(int _ID, Transform _transform, NavpointType _type)
    {
        id = _ID;
        transform = _transform;
        type = _type;
    }

    public int ID
    {
        get
        {
            return id;
        }
    }
    public Transform Transform
    {
        get
        {
            return transform;
        }
    }
    public NavpointType Type
    {
        get
        {
            return type;
        }
    }
    public static Navpoint Default
    {
        get
        {
            return new Navpoint(-1,null,NavpointType.None);
        }
    }
    public static bool operator ==(Navpoint nav1, Navpoint nav2) => nav1.ID == nav2.ID;
    public static bool operator !=(Navpoint nav1, Navpoint nav2) => nav1.ID != nav2.ID;
}
