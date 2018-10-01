//--------------------------------------------------------------------------------------
// Purpose: Handles all world generation logic
//
// Description:  Handles creation and logic behind world generation
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{

    [SerializeField] private int m_nArrayHeight; // Array height
    [SerializeField] private int m_nArrayWidth; // Array width
    public Room[,] m_aRoomArray; // Room array
    private Vector2Int m_v2StartRoom; // Start room position

    [SerializeField] private int m_nRoomCount; // Rooms to spawn
    public int m_nRoomsSpawned; // Number of rooms spawned
    [SerializeField] private GameObject[] m_agRooms; // list of rooms possible to spawn
    [SerializeField] private GameObject m_gStartRoom; // Start room prefab
    [SerializeField] private GameObject m_gBossRoom; // Start room prefab
    [SerializeField] private GameObject m_gItemRoom; // Start room prefab
    [SerializeField] private GameObject m_gGameObjects; // All gameobjects in scene
    [SerializeField] private GameObject m_gA; // A star game object
    [SerializeField] private Vector2 m_v2RoomSize; // Size of room
    public Vector2Int m_v2CurrentIndex; // Current index in array
    private bool m_bCorridors = false; // if corridors
    private bool m_bAreRoomsSpawned = false; // if all rooms spawned
    private bool m_bASpawned = false; // If astar spawned

    private Room m_rBossRoom = null;
    private Room m_rItemRoom = null;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1; // Time scale set to 1
        CreateGrid(); // Creates grid
    }

    private void CreateGrid()
    {
        m_aRoomArray = new Room[m_nArrayWidth, m_nArrayHeight]; // Creates 2D array based on array width and height
        for (int i = 0; i < m_nArrayWidth; i++) // Loops through based on array width
        {
            for (int j = 0; j < m_nArrayHeight; j++) // loops through based on array height
            {
                m_aRoomArray[i, j] = new Room(i, j, new Vector2(i * m_v2RoomSize.x, j * m_v2RoomSize.y)); // Sets array index to new room with current index and position
            }
        }
        m_v2StartRoom.x = (m_nArrayWidth - 1) / 2; // Start room set to middle of the array
        m_v2StartRoom.y = (m_nArrayHeight - 1) / 2; // Start room set to middle of the array
        m_v2CurrentIndex = m_v2StartRoom; // Current index set to start room
        m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].isSpawn = true; // Sets spawn room to true
        m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].isRoom = true; // Sets spawn room to true
        // Instantiates start room
        m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_gStartRoom, m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].worldPosition, new Quaternion(), this.transform);
        m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().isSpawn = true;
        // Sets gameObjects transform to middle of array 
        m_gGameObjects.transform.position = m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.transform.position + new Vector3(20.5f, -13.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_nRoomsSpawned < m_nRoomCount - 3) // Until rooms spawned is greater than room count
        {
            SpawnRooms(); // Spawn rooms
        }
        else if (m_nRoomsSpawned >= m_nRoomCount - 3 && m_nRoomsSpawned <= m_nRoomCount)
        {
            SpawnSpecialRooms();
        }
        else if (!m_bASpawned) // If all rooms spawned
        {
            SpawnA(); // Spawn a star
            m_bASpawned = true; // Sets a star to true
            m_gGameObjects.SetActive(true); // Sets game objects to active
        }


    }

    private void SpawnRooms()
    {
        // 0 = center, 1 = up, 2 = down, 3 = left, 4 = right
        int dir = Random.Range(0, 4); // Gets random number between 0-4
        switch (dir) // Switch statement based on random number
        {
            case 0: // if dir = 0
                m_v2CurrentIndex = m_v2StartRoom; // Current index set to start room
                break;
            case 1: // if dir = up
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1); // Sets current index to 1 above the current index
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true) // If current index isn't a room
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true; // Sets to room
                    // Instantiate random room from list of rooms
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_agRooms[Random.Range(0, m_agRooms.Length)], m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    // Sets corridors and doors active states
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                    m_nRoomsSpawned += 1; // Rooms spawned + 1
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn) // If current index is spawn room
                {
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                }
                else
                {

                }
                break;
            case 2: // If dir = down
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1); // Sets current index to 1 below the current index
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true) // If current index isn't a room
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true; // Sets current index to true
                    // Instantiate random room from list of rooms
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_agRooms[Random.Range(0, m_agRooms.Length)], m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn) // If current index is spawn room
                {
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topWall.SetActive(false);
                }
                else
                {

                }
                break;
            case 3: // if dir = left
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y); // Sets current index to 1 left of the current index
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true) // If current index isn't a room
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true; // Sets current index to room 
                    // Instantiate random room from list of rooms
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_agRooms[Random.Range(0, m_agRooms.Length)], m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn) // If current index is spawn room
                {
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                }
                else
                {

                }
                break;
            case 4: // if dir = right
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y); // Sets current index to 1 right of the current index
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true) // If current index isn't a room
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true; // Sets to room
                    // Instantiate random room from list of rooms
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_agRooms[Random.Range(0, m_agRooms.Length)], m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn) // If current index is spawn room
                {
                    // Set active states of corridors and doors
                    m_aRoomArray[m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                }
                else
                {

                }
                break;
            default:
                break;
        }
        m_bAreRoomsSpawned = true; // if rooms spawned
    }

    private void SpawnSpecialRooms()
    {
        m_rBossRoom = m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y];
        m_rItemRoom = m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y];
        Debug.Log("Boss and item room");
        for (int i = 0; i < m_nArrayWidth; i++)
        {
            for (int j = 0; j < m_nArrayHeight; j++)
            {
                if (m_aRoomArray[i, j].isRoom)
                {
                    float tempDist = Vector2.Distance(m_aRoomArray[i, j].worldPosition, m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].worldPosition);
                    if (tempDist > Vector2.Distance(m_rBossRoom.worldPosition, m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].worldPosition))
                    {
                        m_rBossRoom = m_aRoomArray[i, j];
                    }
                }
            }
        }
        for (int i = 0; i < m_nArrayWidth; i++)
        {
            for (int j = 0; j < m_nArrayHeight; j++)
            {
                if (m_aRoomArray[i, j].isRoom)
                {
                    float tempDistToCenter = Vector2.Distance(m_aRoomArray[i, j].worldPosition, m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].worldPosition);
                    float tempDistToBoss = Vector2.Distance(m_aRoomArray[i, j].worldPosition, m_rBossRoom.worldPosition);
                    if (tempDistToBoss > Vector2.Distance(m_rItemRoom.worldPosition, m_rBossRoom.worldPosition) && tempDistToCenter > Vector2.Distance(m_rItemRoom.worldPosition, m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].worldPosition))
                    {
                        m_rItemRoom = m_aRoomArray[i, j];
                    }
                }
            }
        }
        Debug.Log(m_rBossRoom.worldPosition);
        if (!m_aRoomArray[m_rBossRoom.indexX + 1, m_rBossRoom.indexY].isRoom)
        {
            m_aRoomArray[m_rBossRoom.indexX + 1, m_rBossRoom.indexY].room = Instantiate(m_gBossRoom, m_aRoomArray[m_rBossRoom.indexX + 1, m_rBossRoom.indexY].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rBossRoom.indexX + 1, m_rBossRoom.indexY].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX + 1, m_rBossRoom.indexY].room.GetComponent<Corridors>().leftWall.SetActive(false);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().rightWall.SetActive(false);
        }
        else if(!m_aRoomArray[m_rBossRoom.indexX - 1, m_rBossRoom.indexY].isRoom)
        {
            m_aRoomArray[m_rBossRoom.indexX - 1, m_rBossRoom.indexY].room = Instantiate(m_gBossRoom, m_aRoomArray[m_rBossRoom.indexX - 1, m_rBossRoom.indexY].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rBossRoom.indexX - 1, m_rBossRoom.indexY].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX - 1, m_rBossRoom.indexY].room.GetComponent<Corridors>().rightWall.SetActive(false);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().leftWall.SetActive(false);
        }
        else if(!m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY + 1].isRoom)
        {
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY + 1].room = Instantiate(m_gBossRoom, m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY + 1].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY + 1].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().topCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY + 1].room.GetComponent<Corridors>().bottomWall.SetActive(false);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().topWall.SetActive(false);
        }
        else if(!m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY - 1].isRoom)
        {
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY - 1].room = Instantiate(m_gBossRoom, m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY - 1].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY - 1].room.GetComponent<Corridors>().topCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY - 1].room.GetComponent<Corridors>().topWall.SetActive(false);
            m_aRoomArray[m_rBossRoom.indexX, m_rBossRoom.indexY].room.GetComponent<Corridors>().bottomWall.SetActive(false);
        }
        else
        {
            Debug.Log("Bad World Generation");
        }

        if (!m_aRoomArray[m_rItemRoom.indexX + 1, m_rItemRoom.indexY].isRoom)
        {
            m_aRoomArray[m_rItemRoom.indexX + 1, m_rItemRoom.indexY].room = Instantiate(m_gItemRoom, m_aRoomArray[m_rItemRoom.indexX + 1, m_rItemRoom.indexY].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rItemRoom.indexX + 1, m_rItemRoom.indexY].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX + 1, m_rItemRoom.indexY].room.GetComponent<Corridors>().leftWall.SetActive(false);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().rightWall.SetActive(false);
        }
        else if (!m_aRoomArray[m_rItemRoom.indexX - 1, m_rItemRoom.indexY].isRoom)
        {
            m_aRoomArray[m_rItemRoom.indexX - 1, m_rItemRoom.indexY].room = Instantiate(m_gItemRoom, m_aRoomArray[m_rItemRoom.indexX - 1, m_rItemRoom.indexY].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rItemRoom.indexX - 1, m_rItemRoom.indexY].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX - 1, m_rItemRoom.indexY].room.GetComponent<Corridors>().rightWall.SetActive(false);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().leftWall.SetActive(false);
        }
        else if (!m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY + 1].isRoom)
        {
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY + 1].room = Instantiate(m_gItemRoom, m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY + 1].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY + 1].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().topCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY + 1].room.GetComponent<Corridors>().bottomWall.SetActive(false);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().topWall.SetActive(false);
        }
        else if (!m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY - 1].isRoom)
        {
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY - 1].room = Instantiate(m_gItemRoom, m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY - 1].worldPosition, new Quaternion(), this.transform);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY - 1].room.GetComponent<Corridors>().topCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY - 1].room.GetComponent<Corridors>().topWall.SetActive(false);
            m_aRoomArray[m_rItemRoom.indexX, m_rItemRoom.indexY].room.GetComponent<Corridors>().bottomWall.SetActive(false);
        }
        else
        {
            Debug.Log("Bad World Generation");
        }
        m_nRoomsSpawned += 4;
    }

    void SpawnA()
    {
        Instantiate(m_gA); // Instantiates a Star
    }
}
