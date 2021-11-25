using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public enum GenerationMode {OldMode, NewMode}
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator current;
    //Type of level to be generated
    [SerializeField] private LevelType levelType;
    [SerializeField] private int minNumberOfRooms;
    [SerializeField] private int maxNumberOfRooms;

    [SerializeField] private float scale;
    private Vector3 spawnPoint = Vector3.zero;

    private Dictionary<GameObject, Rectangle> rectMemo = new Dictionary<GameObject, Rectangle>();
    QuaternaryTree<GameObject> level = new QuaternaryTree<GameObject>();

    public bool levelGenerated = false;
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
    }
    private void Start()
    {
        GenerateLevel();
    }
    public void GenerateLevel()
    {
       
       
        
        QuaternaryTree<Exits> levelLayout = GetComponent<LevelLayoutGenerator>().GenerateLayout(maxNumberOfRooms);
        
        QuaternaryTree<GameObject> TreeTraversal(GameObject parent, RoomAlignmentMode roomAlignmentMode, QuaternaryTree<Exits> tree, bool rightToDestroy)
        {
            if(!tree.IsEmpty)
            {
                GameObject instance = CreateRoom(parent, tree.Root, roomAlignmentMode, new List<string>(), rightToDestroy);
                
                if(instance != null)
                {
                    instance.transform.SetParent(transform);
                    QuaternaryTree<GameObject> leftBranch = TreeTraversal(instance, RoomAlignmentMode.Right, tree.LeftSon, false);
                
                    QuaternaryTree<GameObject> rightBranch = TreeTraversal(instance, RoomAlignmentMode.Left, tree.RightSon, false);
                
                    QuaternaryTree<GameObject> downBranch = TreeTraversal(instance, RoomAlignmentMode.Top, tree.DownSon, false);
                
                    QuaternaryTree<GameObject> topBranch = TreeTraversal(instance, RoomAlignmentMode.Down, tree.TopSon, false);
                  
                    return new QuaternaryTree<GameObject>(instance, rightBranch, leftBranch,topBranch,downBranch);
                }
                else
                {
                    return new QuaternaryTree<GameObject>();
                }
                
            }
            return new QuaternaryTree<GameObject>();
        }
        GameObject CreateRoom(GameObject parent, Exits exits, RoomAlignmentMode ram, List<string> dontUse, bool rightToDestroy)
        {
            Room room = levelType.rDB.ProvideRoom(exits, dontUse);
            if(!(room.Name == ""))
            {
                GameObject roomInstance = Instantiate(room.RoomObject);
                roomInstance.transform.localScale *= scale;
                if(parent != null)
                {
                    roomInstance.GetComponent<RoomAligner>().Set(parent.GetComponent<RoomAligner>(), ram);
                }
                
                Rectangle rect = CustomUtilities.TranslateRectangle(room.RoomBox, roomInstance.transform.position)*scale;
                List<GameObject> intersectingRooms = CheckIntersection(rect);
                if((intersectingRooms.Count > 1) || (intersectingRooms.Count > 0 && !rightToDestroy))
                {
                    
                    dontUse.Add(room.Name);
                    Destroy(roomInstance);
                    return CreateRoom(parent, exits, ram, dontUse, rightToDestroy);
                }
                else if(intersectingRooms.Count == 1)
                {
                    rectMemo.Remove(intersectingRooms[0]);
                    Destroy(intersectingRooms[0]);
                }
                
                rectMemo.Add(roomInstance, rect);
                return roomInstance;
            }
            
            return null;
        }
        level = TreeTraversal(null, RoomAlignmentMode.Left,levelLayout, false);
        while (level.Size() < minNumberOfRooms)
        {
            QuaternaryTree<Exits> subLayout = GetComponent<LevelLayoutGenerator>().GenerateLayout(maxNumberOfRooms - level.Size());
            UnityEngine.Debug.Log("Relaunching");
            GameObject leaf = level.GetRandomLeaf();
            System.Array modes = System.Enum.GetValues(typeof(RoomAlignmentMode));
            level.Replace(leaf, TreeTraversal(level.GetParent(leaf),(RoomAlignmentMode)modes.GetValue(Random.Range(0, modes.Length)),subLayout, true));
        }
        AlignTree(level);
        spawnPoint = level.Root.transform.position;
        
        levelGenerated = true;
        GameEvents.current.OnLevelGenerated();
    }
    
    private void AlignTree(QuaternaryTree<GameObject> tree)
    {
        RoomAligner rootAligner = tree.Root.GetComponent<RoomAligner>();
        if (!tree.DownSon.IsEmpty)
        {
            tree.DownSon.Root.GetComponent<RoomAligner>().Set(rootAligner, RoomAlignmentMode.Top);
            AlignTree(tree.DownSon);
        }
        if (!tree.TopSon.IsEmpty)
        {
            tree.TopSon.Root.GetComponent<RoomAligner>().Set(rootAligner, RoomAlignmentMode.Down);
            AlignTree(tree.TopSon);
        }
        if (!tree.RightSon.IsEmpty)
        {
            tree.RightSon.Root.GetComponent<RoomAligner>().Set(rootAligner, RoomAlignmentMode.Left);
            AlignTree(tree.RightSon);
        }
        if (!tree.LeftSon.IsEmpty)
        {
            tree.LeftSon.Root.GetComponent<RoomAligner>().Set(rootAligner, RoomAlignmentMode.Right);
            AlignTree(tree.LeftSon);
        }
    }
    private List<GameObject> CheckIntersection(Rectangle rectangle)
    {
        return rectMemo.Where(x => CustomUtilities.IntersectionArea(x.Value, rectangle) > 0.1f).Select(x => x.Key).ToList();
    }

    
    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }
   
    
    
    public void OnDrawGizmos()
    {

        /*foreach(KeyValuePair<GameObject, Rectangle> rect in rectMemo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(rect.Value.Center, new Vector3(rect.Value.Size.x, rect.Value.Size.y,1));
            foreach(KeyValuePair<GameObject, Rectangle> rect2 in rectMemo)
            {
                if(rect.Key != rect2.Key)
                {
                    Gizmos.color = Color.red;
                    Rectangle rect3 = CustomUtilities.Intersection(rect.Value, rect2.Value);
                   
                    Gizmos.DrawCube(rect3.Center, new Vector3(rect3.Size.x, rect3.Size.y,1));
                }
                
            }

        }*/
        
    }
}
