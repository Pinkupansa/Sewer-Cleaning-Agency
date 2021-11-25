using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System;

public enum RoomMakerMode{ObjectMode, ExitMode, NavpointMode, NavlinkMode}
public enum ExitPlaced{Left, Right, Top, Down} 
public class RoomMaker : MonoBehaviour, INavmeshHolder
{
    [SerializeField] private Transform mouseObject;
    [SerializeField] private Transform roomObject;
  
    [SerializeField] private bool tileMode;

    [SerializeField] private GameObject[] availableObjects;
    
    [SerializeField] private RoomDatabase roomDatabase;

    [SerializeField] private string roomFolderPath;

    [SerializeField] private float tileSize = 1f;
    [SerializeField] private string roomName;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    private Transform topExit, rightExit, leftExit, downExit;

    public RoomMakerMode mode;

    public ExitPlaced exitPlaced;
    public NavpointType placedNavpointType;

    private GameObject currentObject;
    private Vector2 lastClick = Vector2.one * (-1);
    
    private bool placingExit;

    
    
    void OnValidate()
    {
        lastClick = new Vector2(-1, -1);
    }
    public void OnClick(int button)
    {
        if(button == 0)
        {
            switch (mode)
            {
                case RoomMakerMode.NavpointMode:
                    PlaceNavpoint();
                    lastClick = mouseObject.position;
                    break;
                case RoomMakerMode.NavlinkMode:
                    PlaceNavlink(false);
                    break;
            }
        }
        
        if (button == 1)
        {
            if (mode == RoomMakerMode.NavlinkMode)
            {
                PlaceNavlink(true);
            }
            if(mode == RoomMakerMode.NavpointMode)
            {
                DestroyPointedObject();
            }
        }

    }
    public void OnHold(int button)
    {
        if(button == 0)
        {
            if(Vector2.Distance(mouseObject.position, lastClick) > 0.1f)
            {
                switch(mode)
                {
                    case RoomMakerMode.ObjectMode:
                        PlaceObject();
                        lastClick = mouseObject.position;
                        break;
                    case RoomMakerMode.ExitMode:
                        PlaceExit();
                        lastClick = mouseObject.position;
                        break;
                    
                }
                
            }
        }
        if(button == 1)
        {
            if(mode != RoomMakerMode.NavlinkMode && mode != RoomMakerMode.NavpointMode)
            {
                DestroyPointedObject();
            }
            
        } 
    }

    
    public void RenderExits()
    {
        if(topExit != null && topExit.gameObject != null)
        {
            Handles.color = Color.white;
            Handles.DrawCube(0, topExit.position, Quaternion.identity, tileSize/2f);
        }
        if(rightExit != null && rightExit.gameObject != null)
        {
            Handles.color = Color.magenta;
            Handles.DrawCube(0, rightExit.position, Quaternion.identity, tileSize/2f);
        }
        if(leftExit != null && leftExit.gameObject != null)
        {
            Handles.color = Color.green;
            Handles.DrawCube(0, leftExit.position, Quaternion.identity, tileSize/2f);
        }
        if(downExit != null && downExit.gameObject != null)
        {
            Handles.color = Color.black;
            Handles.DrawCube(0, downExit.position, Quaternion.identity, tileSize/2f);
        }
    }
    
    private void DestroyPointedObject()
    {
        if(mode == RoomMakerMode.ExitMode || mode == RoomMakerMode.ObjectMode)
        {
            Transform obj = GetClosestTransformInRadius(0.2f, true);
            if (obj != null)
            {
                DestroyImmediate(obj.gameObject);
            }
        }
        else
        {
            Navmesh navmesh = roomObject.GetComponent<Navmesh>();
            
            if(navmesh != null)
            {
                Navpoint navpoint = navmesh.GetClosestNavpoint(mouseObject.position);
                navmesh.RemoveNavpoint(navpoint);
                
            }
        }
    }
    public void UpdateMouse(Vector3 mousePosition)
    {
        if(mouseObject != null)
        {
            UpdateMousePosition(mousePosition);
            DrawMouse();
            
        }           
    }
    private void UpdateMousePosition(Vector3 mousePosition)
    {
        if (mode == RoomMakerMode.ObjectMode)
        {
            mouseObject.position = new Vector3(Mathf.Clamp(mousePosition.x / tileSize, 0, Width - 1), Mathf.Clamp(mousePosition.y / tileSize, -Height + 1, 0), 0) * tileSize;
        }

        if (mode == RoomMakerMode.ExitMode)
        {
            tileMode = false;
            mouseObject.position = new Vector3(Mathf.Clamp(mousePosition.x / tileSize, -1 / 2f, Width - 1 / 2f), Mathf.Clamp(mousePosition.y / tileSize, -Height + 1 / 2f, 1 / 2f), 0) * tileSize;

        }
        if (mode == RoomMakerMode.NavpointMode || mode == RoomMakerMode.NavlinkMode)
        {
            
            mouseObject.position = new Vector3(Mathf.Clamp(mousePosition.x / tileSize, 0, Width - 1), Mathf.Clamp(mousePosition.y / tileSize, -Height + 1, 0), 0) * tileSize;
        }

        if (tileMode)
        {
            mouseObject.position = new Vector3(Mathf.RoundToInt(mouseObject.position.x / tileSize) * tileSize, Mathf.RoundToInt(mouseObject.position.y / tileSize) * tileSize, 0);
        }
    }
    private void DrawMouse()
    {
        if((currentObject == null  || mode != RoomMakerMode.ObjectMode) && mouseObject.childCount > 0)
        {
            DestroyImmediate(mouseObject.GetChild(0).gameObject);
        }
        if(mode == RoomMakerMode.ObjectMode)
        {
            if(currentObject != null && mouseObject.childCount == 0)
            {
                Instantiate(currentObject, mouseObject.position, Quaternion.identity).transform.SetParent(mouseObject);
            }
        }
        if (mode == RoomMakerMode.NavpointMode || mode == RoomMakerMode.NavlinkMode)
        {
            switch (placedNavpointType)
            {
                case (NavpointType.Ladder):
                    Handles.color = Color.gray;
                    break;
                case (NavpointType.Platform):
                    Handles.color = Color.cyan;
                    break;
            }
            Handles.DrawCube(0, mouseObject.position, Quaternion.identity, tileSize / 2f);
        }
        if (mode == RoomMakerMode.ExitMode)
        {
            switch (exitPlaced)
            {
                case ExitPlaced.Left:
                    Handles.color = Color.green;
                    break;
                case ExitPlaced.Right:
                    Handles.color = Color.magenta;
                    break;
                case ExitPlaced.Top:
                    Handles.color = Color.white;
                    break;
                case ExitPlaced.Down:
                    Handles.color = Color.black;
                    break;
            }
            Handles.DrawCube(0, mouseObject.position, Quaternion.identity, tileSize / 2f);
        }
    }
    public void ChangeObject(GameObject obj)
    {
        CustomUtilities.Clear(mouseObject, DestroyImmediate);
        GameObject instance = Instantiate(obj, mouseObject.position,Quaternion.identity);
        instance.transform.SetParent(mouseObject);
        currentObject = obj;
    }
    
    private void PlaceObject()
    {
        
        Transform objectToDestroy = GetClosestTransformInRadius(0.2f, false);
        if(objectToDestroy != null)
        {
            DestroyImmediate(objectToDestroy.gameObject);
        }
        GameObject instance = Instantiate(currentObject, mouseObject.position,Quaternion.identity);
        instance.transform.SetParent(roomObject);
        lastClick = mouseObject.position;
    }

    private void PlaceNavpoint()
    {
       
        GameObject point = new GameObject();
        point.transform.SetParent(roomObject);
        point.transform.position = mouseObject.position;
        Navmesh navmesh = roomObject.GetComponent<Navmesh>();
        if(navmesh == null)
        {
            navmesh = roomObject.gameObject.AddComponent<Navmesh>();
        }
        navmesh.AddNavpoint(point.transform, placedNavpointType);
    }

    private void PlaceExit()
    {
       
        switch(exitPlaced)
        {
            case ExitPlaced.Left:
                if (leftExit != null && leftExit.gameObject != null)
                {
                    DestroyImmediate(leftExit.gameObject);
                }
                leftExit = (new GameObject()).transform;
                leftExit.gameObject.name = "Left";
                leftExit.position = mouseObject.position;
                leftExit.parent = roomObject;
                break;
            case ExitPlaced.Right:
                if (rightExit != null && rightExit.gameObject != null)
                {
                    DestroyImmediate(rightExit.gameObject);
                }
                rightExit = (new GameObject()).transform;
                rightExit.gameObject.name = "Right";
                rightExit.position = mouseObject.position;
                rightExit.parent = roomObject;
                break;
            case ExitPlaced.Top:
                if (topExit != null && topExit.gameObject != null)
                {
                    DestroyImmediate(topExit.gameObject);
                }
                topExit = (new GameObject()).transform;
                topExit.gameObject.name = "Top";
                topExit.position = mouseObject.position;
                topExit.parent = roomObject;
                break;
            case ExitPlaced.Down:
                if (downExit != null && downExit.gameObject != null)
                {
                    DestroyImmediate(downExit.gameObject);
                }
                downExit = (new GameObject()).transform;
                downExit.gameObject.name = "Down";
                downExit.position = mouseObject.position;
                downExit.parent = roomObject;
                break;
        }
    }

    private void PlaceNavlink(bool isJump)
    {
        Navmesh navmesh = roomObject.GetComponent<Navmesh>();
        if(navmesh != null)
        {
            if(lastClick != new Vector2(-1, -1))
            {
                Navpoint navpoint1 = navmesh.GetClosestNavpoint(lastClick);
                Navpoint navpoint2 = navmesh.GetClosestNavpoint(mouseObject.position);
                navmesh.AddLink(navpoint1, navpoint2,isJump);
                navmesh.AddLink(navpoint2, navpoint1, isJump);
                lastClick = new Vector2(-1, -1);
            }
            else
            {
                lastClick = mouseObject.position;
            }
        }
        
    }
    private Transform GetClosestTransformInRadius(float radius, bool takeExitsInAccount)
    {
        List<Transform> allElements = new List<Transform>();
        foreach(Transform t in roomObject)
        {
            allElements.Add(t);
        }
        List<Transform> eligible = allElements.Where(x => Vector2.Distance(x.position, mouseObject.position) < radius && (takeExitsInAccount||(x.gameObject.name != "Top" && x.gameObject.name != "Down" && x.gameObject.name != "Right" && x.gameObject.name != "Left")) && !x.gameObject.name.Contains("Navpoint")).ToList();
        Transform closest = eligible.OrderBy(x => Vector2.Distance(x.position, mouseObject.position)).FirstOrDefault();
        return closest;

    }
   
    public void ClearRoom()
    {
        CustomUtilities.Clear(roomObject, DestroyImmediate);
        Navmesh navmesh = roomObject.GetComponent<Navmesh>();
        if(navmesh != null)
        {
            DestroyImmediate(navmesh);
        }
        
    }
    public void SaveRoom()
    {
        string path = Application.dataPath + "/" + roomFolderPath + "/" + roomName + ".prefab";
      //  path = AssetDatabase.GenerateUniqueAssetPath(path);
        if(path != "")
        {
            Rectangle roomBox = CalculateRoomBox();
            GameObject roomClone = Instantiate(roomObject.gameObject);
            RoomAligner aligner = roomClone.AddComponent<RoomAligner>();
            
            aligner.top = roomClone.transform.Find("Top");
            aligner.down = roomClone.transform.Find("Down");
            aligner.right = roomClone.transform.Find("Right");
            aligner.left = roomClone.transform.Find("Left");
            
            GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(roomClone, path, InteractionMode.UserAction);
          
            if(roomDatabase != null)
            {
               
                roomDatabase.SaveRoom(roomName, new Exits(topExit != null && topExit.gameObject != null, downExit != null && downExit.gameObject != null, rightExit != null && rightExit.gameObject != null, leftExit != null && leftExit.gameObject != null), prefab, roomBox);
            }
            DestroyImmediate(roomClone);
        }
    }

    public void LoadRoom(Room room)
    {
        ClearRoom();
        
        GameObject roomInstance = Instantiate(room.RoomObject,roomObject.position,Quaternion.identity);
        DestroyImmediate(roomObject.gameObject);
        roomObject = roomInstance.transform;
        roomObject.name = room.Name;
        roomName = room.Name;

        RoomAligner aligner = roomInstance.GetComponent<RoomAligner>();
        topExit = aligner.top;
        rightExit = aligner.right;
        downExit = aligner.down;
        leftExit = aligner.left;

        DestroyImmediate(aligner);
        
        
        gridWidth = (int)((room.RoomBox.maxX - room.RoomBox.minX)/tileSize) + 1;
        gridHeight = (int)((room.RoomBox.maxY - room.RoomBox.minY)/tileSize) + 1;
       
    }
    
    private Rectangle CalculateRoomBox()
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;
        foreach(Transform t in roomObject)
        {
            minX = Mathf.Min(minX, t.position.x);
            minY = Mathf.Min(minY, t.position.y);
            maxX = Mathf.Max(maxX, t.position.x);
            maxY = Mathf.Max(maxY, t.position.y);
        }
        
        
        return new Rectangle(minX,minY,maxX,maxY);
    }

    
    private Vector2Int GetMouseGridPosition()
    {
        return GetGridPosition(mouseObject.position);
    }

    private Vector2Int GetGridPosition(Vector2 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x / tileSize), Mathf.RoundToInt(-pos.y / tileSize));
    }
    public int Width
    {
        get
        {
            return gridWidth;
        }
    }
    public int Height
    {
        get
        {
            return gridHeight;
        }
    }
    public bool TileMode
    {
        get
        {
            return tileMode;
        }
    }
    public GameObject[] AvailableObjects
    {
        get
        {
            return availableObjects;
        }
    }
    public float TileSize
    {
        get
        {
            return tileSize;
        }
    }
    public RoomDatabase RoomDatabase
    {
        get
        {
            return roomDatabase;
        }
    }
    public Navmesh GetNavmesh()
    {
        return roomObject.gameObject.GetComponent<Navmesh>();
    }
   
    
}
