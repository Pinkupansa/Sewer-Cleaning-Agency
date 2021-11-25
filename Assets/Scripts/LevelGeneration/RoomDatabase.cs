using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDatabase", menuName = "ScriptableObjects/RoomDatabase", order = 1)]
public class RoomDatabase : ScriptableObject
{
    
    [SerializeField] private List<Room> rooms;
    public Room ProvideRoom(Exits exits, List<string> dontUse)
    {
        List<Room> usable = rooms.Where(x => !dontUse.Contains(x.Name)).ToList();
        float bestScore = usable.Select(x => Exits.MatchScore(x.RoomExits,exits)).OrderBy(x => -x).FirstOrDefault();
        List<Room> bestRooms = usable.Where(x => Mathf.Abs(Exits.MatchScore(x.RoomExits,exits) - bestScore) < 0.5f).ToList();
        if(bestRooms.Count > 0)
        {
            return bestRooms[Random.Range(0, bestRooms.Count())];
        }
        else
        {
            return new Room(null,new Exits(false,false,false,false),"",new Rectangle(0,0,0,0));
        }
        
    }

    public void SaveRoom(string name, Exits exits, GameObject room, Rectangle roomBox)
    {
        List<Room> matchingRooms = rooms.Where(x => x.Name == name).ToList();
        if(matchingRooms.Count == 0)
        {
            rooms.Add(new Room(room,exits,name,roomBox));
        }
        else
        {
            Room matchingRoom = matchingRooms.FirstOrDefault();
            rooms[rooms.FindIndex(x => x.Name == matchingRoom.Name)] = new Room(room,exits,name,roomBox);
        }
        
    }

    public void RemoveRoom(Room r)
    {
        rooms.Remove(r);
    }
    public List<Room> Rooms
    {
        get
        {
            return rooms;
        }
    }
}

[System.Serializable]
public struct Room
{
    [SerializeField] private string name;
    [SerializeField] private GameObject room;
    [SerializeField] private Exits exits;

    [SerializeField] private Rectangle roomBox;
   
    public Room (GameObject room, Exits exits, string name, Rectangle roomBox)
    {
        this.room = room;
        this.exits = exits;
        this.name = name;
        this.roomBox = roomBox;
        
    }
    public Exits RoomExits
    {
        get
        {
            return exits;
        }
    }
    public string Name
    {
        get
        {
            return name;
        }
    }
    public GameObject RoomObject
    {
        get
        {
            return room;                    
        }
        
    }
    public Rectangle RoomBox
    {
        get
        {
            return roomBox;
        }
    }
    

}