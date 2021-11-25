using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public enum LevelLayoutGeneratorMode{Start, Runtime}
public class LevelLayoutGenerator : MonoBehaviour
{
    
    
    public QuaternaryTree<Exits> GenerateLayout(int maxNumberOfRooms)
    {
        
        int roomCount = 0;
        QuaternaryTree<Exits> GenerateTree(Exits root, Side parentSide)
        { 
            QuaternaryTree<Exits> downSon = new QuaternaryTree<Exits>();
            QuaternaryTree<Exits> topSon = new QuaternaryTree<Exits>();
            QuaternaryTree<Exits> leftSon = new QuaternaryTree<Exits>();
            QuaternaryTree<Exits> rightSon = new QuaternaryTree<Exits>();
            
            
            if(roomCount < maxNumberOfRooms)
            {
                if(root.top && roomCount < maxNumberOfRooms && parentSide != Side.Top)
                {
                    roomCount ++;
                    bool[] exits = GenerateExits();
                    topSon = GenerateTree(new Exits(exits[0], true, exits[1], exits[2]), Side.Down);
                }
                if(root.down && roomCount < maxNumberOfRooms && parentSide != Side.Down)
                {
                    roomCount ++;
                    bool[] exits = GenerateExits();
                    downSon = GenerateTree(new Exits(true, exits[0], exits[2], exits[1]), Side.Top);
                }
                if(root.left && roomCount < maxNumberOfRooms && parentSide != Side.Left)
                {
                    roomCount ++;
                    bool[] exits = GenerateExits();
                    leftSon = GenerateTree(new Exits(exits[1], exits[2], exits[0], true), Side.Right);
                }
                if(root.right && roomCount < maxNumberOfRooms && parentSide != Side.Right)
                {
                    roomCount++;
                    bool[] exits = GenerateExits();
                    rightSon = GenerateTree(new Exits(exits[2], exits[1], true, exits[0]), Side.Left);
                }

            }
            
            
            return new QuaternaryTree<Exits>(root, rightSon, leftSon, topSon, downSon);
        }    
        QuaternaryTree<Exits> layout = GenerateTree(new Exits(true, true, true, true), Side.None);
        
        return layout;
    }
    
    bool[] GenerateExits()
    {
        bool[] exits = new bool[3];
        int exitsCount = 0;
        if(CustomUtilities.RandomBoolean(0.55F))
        {
            exits[0] = true;
            exitsCount ++;       
        }
        if(CustomUtilities.RandomBoolean(0.5f/4*(exitsCount+1)))
        {
            exits[1] = true;
            exitsCount ++;   
        }

        if(CustomUtilities.RandomBoolean(0.5f/4*(exitsCount+1)))
        {
            exits[2] = true;
        }
        return exits;
    }
}


[System.Serializable]
public struct Exits
{
    public bool top, right, down, left;

    public int index;
    public Exits(bool _top, bool _down, bool _right, bool _left)
    {
        top = _top;
        down = _down;
        right = _right;
        left = _left;
        index = -1;
    }
    public Exits AddExit()
    {
        if(!top)
        {
            return new Exits(true, down, right, left);
        }
        if(!right)
        {
            return new Exits(top, down, true,left);
        }
        if(!left)
        {
            return new Exits(top, down, right,true);
        }
        if(!down)
        {
            return new Exits(top, true, right, left);
        }
        return this;
    }
    public bool IsEmpty
    {
        get
        {
            return !(top||right||down||left);
        }
    }
    public static float MatchScore(Exits replacement, Exits model)
    {
        
        return Convert.ToInt16(replacement.top == model.top) + Convert.ToInt16(replacement.right == model.right) + Convert.ToInt16(replacement.left == model.left) + Convert.ToInt16(replacement.down == model.down) - 1.5f*(Convert.ToInt16(!replacement.top && model.top) + Convert.ToInt16(!replacement.down && model.down) + Convert.ToInt16(!replacement.left && model.left) +Convert.ToInt16(!replacement.right && model.right));
    }
    public static string Describe(Exits exits)
    {
        string s = "";
        if(exits.top)
        {
            s += "Top ";
        }
        if(exits.down)
        {
            s += "Down ";
        }
        if(exits.right)
        {
            s += "Right ";
        }
        if(exits.left)
        {
            s += "Left ";
        }
        return s;
    }
}
