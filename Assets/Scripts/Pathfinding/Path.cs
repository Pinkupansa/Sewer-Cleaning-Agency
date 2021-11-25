using System.Collections.Generic;
public class Path
{
    private List<Navlink> path;

    public Navlink this[int i]
    {
        get
        {
            return path[i];
        }
    }
    public int Length
    {
        get
        {
            return path.Count;
        }
    }
    public Path(List<Navlink> links)
    {
        for(int i = 0; i < links.Count-1; i++)
        {
            if(links[i].End != links[i+1].Start)
            {
                throw new System.Exception("The path is not valid");
            }
        }
        path = links;
    }
}
