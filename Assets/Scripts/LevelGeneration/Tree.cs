using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side{Left, Right, Top, Down, None}
public class QuaternaryTree<T>
{
    
    private T root;
    private QuaternaryTree<T> rightSon, leftSon, topSon, downSon;

    public bool IsEmpty{get; private set;}
    public QuaternaryTree(T root, QuaternaryTree<T> rightSon, QuaternaryTree<T> leftSon, QuaternaryTree<T> topSon, QuaternaryTree<T> downSon)
    {
        this.root = root;
        this.rightSon = rightSon;
        this.leftSon = leftSon;
        this.topSon = topSon;
        this.downSon = downSon;
        IsEmpty = false;
    }
    public T GetRandomLeaf()
    {
        if(IsLeaf)
        {
            return Root;
        }
        else 
        {
            
            List<QuaternaryTree<T>> sons = new List<QuaternaryTree<T>>();
            if(!downSon.IsEmpty)
            {
                sons.Add(downSon);
            }
            if(!topSon.IsEmpty)
            {
                sons.Add(topSon);

            }
            if(!rightSon.IsEmpty)
            {
                sons.Add(rightSon);
                
            }
            if(!leftSon.IsEmpty)
            {
                sons.Add(leftSon);
                
            }
            
            return sons[Random.Range(0, sons.Count)].GetRandomLeaf();
            
            
            
        }
    }

    public T GetParent(T node)
    {
        T parent = root;
        void Aux(QuaternaryTree<T> tree)
        {
            if(!tree.IsEmpty)
            {
                if(!tree.DownSon.IsEmpty && tree.DownSon.Root.Equals(node) || !tree.TopSon.IsEmpty && tree.TopSon.Root.Equals(node) || !tree.RightSon.IsEmpty && tree.RightSon.Root.Equals(node) || !tree.LeftSon.IsEmpty && tree.LeftSon.Root.Equals(node) )
                {
                    parent = tree.Root;
                    return;
                }
                else
                {
                    Aux(tree.TopSon);
                    Aux(tree.RightSon);
                    Aux(tree.LeftSon);
                    Aux(tree.DownSon);
                }
            }
        }
        Aux(this);
        return parent;
    }
    
    
    public void Replace(T node, QuaternaryTree<T> tree)
    {
        if(!IsEmpty)
        {
            if(root.Equals(node))
            {
                this.root = tree.Root;
                this.topSon = tree.TopSon;
                this.downSon = tree.DownSon;
                this.leftSon = tree.LeftSon;
                this.rightSon = tree.RightSon;

                this.IsEmpty = tree.IsEmpty;
            }
            else
            {
                rightSon.Replace(node, tree);
                leftSon.Replace(node, tree);
                topSon.Replace(node, tree);
                downSon.Replace(node, tree);

            }
        }
    }
    public QuaternaryTree()
    {
        IsEmpty = true;
    }
    
    public int Size()
    {
        if(IsEmpty)
        {
            return 0;
        }
        else
        {
            return leftSon.Size() + rightSon.Size() + topSon.Size() + downSon.Size() + 1;    
        }
    }
    
    public T Root
    {
        get
        {
            return root;
        }
    }
    public QuaternaryTree<T> RightSon
    {
        get
        {
            return rightSon;
        }
    }
    public QuaternaryTree<T> LeftSon
    {
        get
        {
            return leftSon;
        }
    }
    public QuaternaryTree<T> TopSon
    {
        get
        {
            return topSon;
        }
    }
    public QuaternaryTree<T> DownSon
    {
        get
        {
            return downSon;
        }
    }
    public bool IsLeaf
    {
        get
        {
            return (!IsEmpty) && leftSon.IsEmpty && rightSon.IsEmpty && topSon.IsEmpty && downSon.IsEmpty;
        }
    }

    
}
