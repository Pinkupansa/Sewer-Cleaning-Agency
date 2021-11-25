using UnityEngine;
[System.Serializable]
public class Navlink
{
    [SerializeField] private Navpoint startPoint;
    [SerializeField] private Navpoint endPoint;

    [SerializeField] private int weight;
    [SerializeField] private bool isJumpLink;
    public Navlink(Navpoint _start, Navpoint _end, bool _isJumpLink, int _weight)
    {
        startPoint = _start;
        endPoint = _end;
        isJumpLink = _isJumpLink;
        weight = _weight;
    }
    public Navpoint Start
    {
        get
        {
            return startPoint;
        }
    }
    public Navpoint End
    {
        get
        {
            return endPoint;
        }
    }
    public bool IsJumpLink
    {
        get
        {
            return isJumpLink;
        }
    }
    public int Weight
    {
        get
        {
            return weight;
        }
    }
    public static Navlink Default
    {
        get
        {
            return new Navlink(Navpoint.Default, Navpoint.Default, false, -1);
        }
    }
    public bool Contains(Navpoint x)
    {
        return x == startPoint || x == endPoint;
    }
    public static bool operator ==(Navlink n1, Navlink n2) => (n1.Start == n2.Start) && (n1.End == n2.End);
    public static bool operator !=(Navlink n1, Navlink n2) => !((n1.Start == n2.Start) && (n1.End == n2.End));
}