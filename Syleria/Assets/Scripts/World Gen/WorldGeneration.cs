using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{

    [SerializeField] private int m_nArrayHeight;
    [SerializeField] private int m_nArrayWidth;
    public Room[,] m_aRoomArray;
    private Vector2Int m_v2StartRoom;

    [SerializeField] private int m_nRoomCount;
    public int m_nRoomsSpawned;
    [SerializeField] private GameObject m_gRoom;
    [SerializeField] private GameObject m_gStartRoom;
    [SerializeField] private GameObject m_gGameObjects;
    [SerializeField] private GameObject m_gA;
    [SerializeField] private Vector2 m_v2RoomSize;
    public Vector2Int m_v2CurrentIndex;
    private bool m_bCorridors = false;
    private bool m_bAreRoomsSpawned = false;
    private bool m_bASpawned = false;

    // Use this for initialization
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        m_aRoomArray = new Room[m_nArrayWidth, m_nArrayHeight];
        for (int i = 0; i < m_nArrayWidth; i++)
        {
            for (int j = 0; j < m_nArrayHeight; j++)
            {
                m_aRoomArray[i, j] = new Room(i, j, new Vector2(i * m_v2RoomSize.x, j * m_v2RoomSize.y));
            }
        }
        m_v2StartRoom.x = (m_nArrayWidth - 1) / 2;
        m_v2StartRoom.y = (m_nArrayHeight - 1) / 2;
        m_v2CurrentIndex = m_v2StartRoom;
        m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].isSpawn = true;
        m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].isRoom = true;
        m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_gStartRoom, m_aRoomArray[m_v2StartRoom.x, m_v2StartRoom.y].worldPosition, new Quaternion(), this.transform);
        m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().isSpawn = true;
        m_gGameObjects.transform.position = m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.transform.position + new Vector3(20.5f, -13.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_nRoomsSpawned < m_nRoomCount - 1)
        {
            SpawnRooms();
        }
        else if (!m_bASpawned)
        {
            SpawnA();
            m_bASpawned = true;
            m_gGameObjects.SetActive(true);
        }


    }

    private void SpawnRooms()
    {
        // 0 = center, 1 = up, 2 = down, 3 = left, 4 = right
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0:
                m_v2CurrentIndex = m_v2StartRoom;
                break;
            case 1:
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1);
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true)
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true;
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_gRoom, m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if(m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn)
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1].room.GetComponent<Corridors>().topWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                }
                else
                {

                }
                break;
            case 2:
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x, m_v2CurrentIndex.y - 1);
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true)
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true;
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_gRoom, m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn)
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y + 1].room.GetComponent<Corridors>().bottomWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().topWall.SetActive(false);
                }
                else
                {

                }
                break;
            case 3:
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y);
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true)
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true;
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_gRoom, m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn)
                {
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                }
                else
                {

                }
                break;
            case 4:
                m_v2CurrentIndex = new Vector2Int(m_v2CurrentIndex.x + 1, m_v2CurrentIndex.y);
                if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom != true)
                {
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isRoom = true;
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room = Instantiate(m_gRoom, m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].worldPosition, new Quaternion(), this.transform);
                    m_aRoomArray[m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftCorridor.SetActive(true);
                    m_aRoomArray[m_v2CurrentIndex.x - 1, m_v2CurrentIndex.y].room.GetComponent<Corridors>().rightWall.SetActive(false);
                    m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].room.GetComponent<Corridors>().leftWall.SetActive(false);
                    m_nRoomsSpawned += 1;
                }
                else if (m_aRoomArray[m_v2CurrentIndex.x, m_v2CurrentIndex.y].isSpawn)
                {
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
        m_bAreRoomsSpawned = true;
    }

    void SpawnA()
    {
        Instantiate(m_gA);
    }
}
