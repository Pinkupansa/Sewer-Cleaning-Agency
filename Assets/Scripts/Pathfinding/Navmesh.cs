
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Used for AI navigation

public class Navmesh : MonoBehaviour, INavmeshHolder
{
    [SerializeField] private List<Navpoint> navpoints = new List<Navpoint>();
    [SerializeField] private List<Navlink> navlinks = new List<Navlink>();
    
    private Dictionary<int, HashSet<Navlink>> neighbourDictionary;

    private void Start()
    {
        neighbourDictionary = new Dictionary<int, HashSet<Navlink>>();
        
        foreach(Navpoint point in navpoints)
        {   
            
            neighbourDictionary.Add(point.ID, new HashSet<Navlink>());
        }
        
        foreach(Navlink link in navlinks)
        {
            
            neighbourDictionary[link.Start.ID].Add(link);
        }
    }
    
    public void AddNavpoint(Transform _transform, NavpointType navpointType)
    {
        if(navpoints.Count == 0 || GetClosestNavpoint(_transform.position) == Navpoint.Default || Vector2.Distance(GetClosestNavpoint(_transform.position).Transform.position, _transform.position) > 0.5f )
        {
            Navpoint navpoint = new Navpoint(GenerateID(), _transform, navpointType);
            navpoint.Transform.name = "Navpoint " + navpoint.ID;
            navpoints.Add(navpoint);
            
        }
        else
        {
            DestroyImmediate(_transform.gameObject);
        }
        
    }

    public void RemoveNavpoint(Navpoint navpoint)
    {
        if(navpoints.Contains(navpoint))
        {
            
            
            navlinks = navlinks.Where(x => !x.Contains(navpoint)).ToList();

            navpoints.Remove(navpoint);
            DestroyImmediate(navpoint.Transform.gameObject);
            
        }
    }
    public void AddLink(Navpoint start, Navpoint end, bool isJumpLink)
    {
        if(start != end && start != Navpoint.Default  && end != Navpoint.Default)
        {
            Navlink link = FindNavlink(start, end);
            if(link != Navlink.Default)
            {
                navlinks.Remove(link);
                
            }
            
            link = new Navlink(start, end, isJumpLink, (int)Vector2.Distance(start.Transform.position, end.Transform.position));
            navlinks.Add(link);
            
        }
        
    }
    public Navlink FindNavlink(Navpoint start, Navpoint end)
    {
        foreach(Navlink n in navlinks)
        {
            if(n.Start == start && n.End == end)
            {
                return n;
            }
        }
        return Navlink.Default;
    }

    public HashSet<Navlink> GetLinks(Navpoint navpoint)
    {
        return neighbourDictionary[navpoint.ID];
    }
    public Navpoint GetClosestNavpoint(Vector2 position)
    {
        if(navpoints.Count > 0)
        {
            float dist = navpoints.Min(x => Vector2.Distance(x.Transform.position, position));
            
            return navpoints.Where(x => Mathf.Abs(Vector2.Distance(x.Transform.position,position) - dist) < 0.01f).FirstOrDefault();
            
        }
        else
        {
            throw new System.Exception("Navmesh is empty");
        }
    }
    
    private int GenerateID()
    {
        int id = 1;
        List<int> ids = navpoints.Select(x => x.ID).OrderBy(x => x).ToList();
        for (int i = 0; i < ids.Count; i++)
        {
            if(ids[i] == id)
            {
                id++;
            }
        }
        return id;
    }

    public List<Navpoint> Navpoints
    {
        get
        {
            return navpoints;
        }
    }
    public List<Navlink> Navlinks
    {
        get
        {
            return navlinks;
        }
    }
    public int GetMaxID()
    {
                
        return navpoints.Select(x => x.ID).Max();
    }
    public Navmesh GetNavmesh()
    {
        return this;
    }
}


