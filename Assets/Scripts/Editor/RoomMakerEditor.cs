using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomMaker))]
public class RoomMakerEditor : Editor
{
    private RoomMaker roomMaker;
    private bool mouseDown;

    


    private void OnEnable()
    {
        roomMaker = (RoomMaker)target;

        SceneView.duringSceneGui += OnSceneGUI;
        
    }
    public override void OnInspectorGUI()
    {
        if(roomMaker != null)
        {
            DrawDefaultInspector();
            switch(roomMaker.mode)
            {
                case RoomMakerMode.ObjectMode:
                    foreach(GameObject g in roomMaker.AvailableObjects)
                    {
                        if(GUILayout.Button(g.name))
                        {
                            roomMaker.ChangeObject(g);
                        }
                    }
                    break;
                case RoomMakerMode.ExitMode:
                    
                    break;
            }
            
            if(GUILayout.Button("Clear"))
            {
                roomMaker.ClearRoom();
            }
            if(GUILayout.Button("Save Room"))
            {
                roomMaker.SaveRoom();
            }

            foreach(Room r in roomMaker.RoomDatabase.Rooms)
            {
                if(GUILayout.Button("Load Room " + r.Name) )
                {
                    roomMaker.LoadRoom(r);
                }
            }
        }
        
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        if(roomMaker != null)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        
            Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            roomMaker.UpdateMouse(mousePosition);
            if(Event.current.type == EventType.MouseDown)
            {
                roomMaker.OnClick(Event.current.button);
                mouseDown = true;
            }
            if(Event.current.type == EventType.MouseUp)
            {
                mouseDown = false;
            }
            if(mouseDown)
            {
                roomMaker.OnHold(Event.current.button);
            }
            
            Handles.color = Color.gray;
            if(roomMaker.TileMode)
            {
                
                for(int x = 0; x < roomMaker.Width; x++)
                {
                    for(int y = 0; y < roomMaker.Height; y++)
                    {
                        Handles.DrawWireCube(new Vector3(x*roomMaker.TileSize,-y*roomMaker.TileSize,0), Vector3.one * roomMaker.TileSize);
                    }
                }       
            }
            else
            {
                Handles.DrawWireCube(new Vector3(roomMaker.TileSize * (roomMaker.Width-1)/2f,-roomMaker.TileSize*(roomMaker.Height-1)/2f,0), new Vector3(roomMaker.TileSize * roomMaker.Width, roomMaker.TileSize*roomMaker.Height,1f));
            }
            
            roomMaker.RenderExits();
            
        }
       
    }

    private void OnDrawGizmos()
    {
            
        
    }
    
}
