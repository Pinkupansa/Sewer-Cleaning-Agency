
using System.Collections.Generic;


//implementation of the Heap struct for Dijkstra's pathfinding
public class Heap
{
    private List<HeapElement> nodes;
    private int[] index;

    public Heap(int maxSize)
    {
        nodes = new List<HeapElement>();
        nodes.Add(new HeapElement(Navpoint.Default, Navpoint.Default, 0));
        index = new int[maxSize];
        for(int i = 0; i < maxSize; i++)
        {
            index[i] = -1;
        }
    }

    public HeapElement Root
    {
        get
        {
            return nodes[1];
        }
    }
    public int Size
    {
        get
        {
            return nodes[0].weight;
        }
    }

    private void Switch(int i, int j)
    {
        index[nodes[i].point.ID] = j;
        index[nodes[j].point.ID] = i;
        HeapElement tampon = nodes[i];
        nodes[i] = nodes[j];
        nodes[j] = tampon;
    }
    private void AscendingPercolation(int i)
    {
        if (i == 1)
        {
            return;
        }
        else
        {
            int pere = i / 2;
            if (nodes[pere].weight > nodes[i].weight)
            {
                Switch(i, pere);
                AscendingPercolation(pere);
            }
            else
            {
                return;
            }

        }
    }
    private void DescendingPercolation(int i)
    {
        int filsGauche = 2 * i;
        int filsDroit = 2 * i + 1;
        if (filsGauche > Size)
        {
            return;
        }
        else if (filsDroit > Size)
        {
            if (nodes[filsGauche].weight < nodes[i].weight)
            {
                Switch(i, filsGauche);
                DescendingPercolation(filsGauche);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (nodes[filsDroit].weight < nodes[filsGauche].weight)
            {
                if (nodes[filsDroit].weight < nodes[i].weight)
                {
                    Switch(i, filsDroit);
                    DescendingPercolation(filsDroit);
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (nodes[filsGauche].weight < nodes[i].weight)
                {
                    Switch(i, filsGauche);
                    DescendingPercolation(filsGauche);
                }
                else
                {
                    return;
                }
            }
        }
    }
    private void Add(Navpoint point, Navpoint predecessor, int weight)
    {
        nodes[0].weight++;
        nodes.Add(new HeapElement(point, predecessor, weight));
        index[point.ID] = Size;
        AscendingPercolation(Size);
    }
    public void Update(Navpoint point, Navpoint predecessor, int weight)
    {
        if(point != Navpoint.Default)
        {
            int i = index[point.ID];
            if (i != -1)
            {
                HeapElement n = nodes[i];
                if (n.weight > weight)
                {
                    n.weight = weight;
                    n.predecessor = predecessor;
                    AscendingPercolation(i);
                }
            }
            else
            {

                Add(point, predecessor, weight);
            }
        }
        
    }
    public void DeleteRoot()
    {
        Switch(1, Size);
        index[nodes[Size].point.ID] = -1;
        nodes.Remove(nodes[Size]);
        nodes[0].weight -= 1;
        if (Size != 0)
        {
            DescendingPercolation(1);
        }
    }
   
}

public class HeapElement
{
    public int weight;
    public Navpoint point { get; }
    public Navpoint predecessor;
    public HeapElement(Navpoint _point, Navpoint _predecessor, int _weight)
    {
        weight = _weight;
        point = _point;
        predecessor = _predecessor;
    }
}