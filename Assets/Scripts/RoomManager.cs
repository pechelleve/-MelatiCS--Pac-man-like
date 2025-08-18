using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<Room> _roomList = new List<Room>();
    [SerializeField] private Transform[] roomPositions; //Empty GameObjects as slots

    private void Start()
    {
        ShuffleRooms();
    }
    
    public void ShuffleRooms()
    {
        for (int i = 0; i < _roomList.Count; i++)
        {
            int randomIndex = Random.Range(i, roomPositions.Length);

            // Place room at slot
            _roomList[i].transform.position = roomPositions[randomIndex].position;
        }
    }
}
