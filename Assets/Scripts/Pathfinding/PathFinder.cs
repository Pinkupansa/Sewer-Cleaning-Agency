
using System.Collections.Generic;
using System.Linq;

public static class PathFinder
{
    public static Path FindPath(Navpoint start, Navpoint end, Navmesh navmesh)
    {
        int maxSize = navmesh.GetMaxID() + 1;
        
        Heap file = new Heap(maxSize);
        List<Navpoint> closeList = new List<Navpoint>();
        Navpoint[] predecessors = new Navpoint[maxSize];
        for(int i = 0; i< maxSize;i++)
        {
            predecessors[i] = Navpoint.Default;
        }
        file.Update(start, Navpoint.Default, 0);
        
        while(!closeList.Contains(end) && file.Size > 0)
        {
            HeapElement currentElement = file.Root;
            
            foreach(Navlink n in navmesh.GetLinks(currentElement.point))
            {
                if(!closeList.Contains(n.End))
                {
                    file.Update(n.End, currentElement.point, currentElement.weight + n.Weight);
                }
            }
            predecessors[currentElement.point.ID] = currentElement.predecessor;
            
            closeList.Add(currentElement.point);

            file.DeleteRoot();
        }

        List<Navpoint> pathNavpoints = new List<Navpoint>();

        Navpoint currentPoint = end;
        while(currentPoint != Navpoint.Default)
        {
            pathNavpoints.Add(currentPoint);
            currentPoint = predecessors[currentPoint.ID];
        }
        pathNavpoints.Reverse();
        List<Navlink> pathNavlinks = new List<Navlink>();
        for(int i = 0; i < pathNavpoints.Count-1; i++)
        {
            pathNavlinks.Add(navmesh.FindNavlink(pathNavpoints[i], pathNavpoints[i+1]));
        }
        return new Path(pathNavlinks);

    }

}
