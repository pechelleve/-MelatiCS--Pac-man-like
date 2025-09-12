using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<Room> _roomList = new List<Room>();
    [SerializeField] private Transform[] roomPositions; //Empty GameObjects as slots
    public bool[] availableRoomPositions;
    [SerializeField] private NavMeshSurface navMeshSurface;

    private void Start()
    {
        availableRoomPositions = new bool[roomPositions.Length];

        //mark all slots as available at start
        for (int i = 0; i< availableRoomPositions.Length; i++)
            availableRoomPositions[i] = true;

        ShuffleRooms();
    }

    public void ShuffleRooms()
    {
        foreach (Room room in _roomList)
        {
            //pick a random available slot
            int slotIndex = GetRandomAvailableSlot();
            if (slotIndex == -1)
            {
                Debug.LogWarning("No available slots left!");
                return;
            }

            //Place Room at that slot
            room.transform.position = roomPositions[slotIndex].position;
            room.transform.rotation = roomPositions[slotIndex].rotation;
            room.roomIndex = slotIndex;

            availableRoomPositions[slotIndex] = false;
        }
        navMeshSurface.BuildNavMesh();
    }

    private int GetRandomAvailableSlot()
    {
        List<int> available = new List<int>();
        for (int i = 0; i < availableRoomPositions.Length; i++)
        { 
            if (availableRoomPositions[i]) available.Add(i);
        }

        if (available.Count == 0) return -1; //No slots left
        return available[Random.Range(0, available.Count)];
    }
}
