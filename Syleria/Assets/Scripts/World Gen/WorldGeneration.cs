using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {

    private int m_nArrayHeight;
    private int m_nArrayWidth;
    private Room[,] m_aRoomArray;

    [SerializeField] private int m_nRoomCount;
    [SerializeField] private GameObject m_gRoom;
    [SerializeField] private GameObject m_gStartRoom;
    enum RoomType
    {
        NONE,
        BASIC,
        SHOP,
        BOSS,
        START
    }
    struct Room
    {
        public RoomType roomType;


    }


	// Use this for initialization
	void Start ()
    {
        m_nArrayHeight = Mathf.RoundToInt(m_nRoomCount * 0.5f);
        m_nArrayWidth = Mathf.RoundToInt(m_nRoomCount * 0.5f);
        m_aRoomArray = new Room[m_nArrayWidth, m_nArrayHeight];
        m_aRoomArray[Mathf.RoundToInt(m_nArrayWidth * 0.5f), Mathf.RoundToInt(m_nArrayHeight * 0.5f)].roomType = RoomType.START;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitialSetup();
            SpawnRooms();
        }

	}

    public void InitialSetup()
    {
        for (int i = 0; i < m_nArrayWidth; i++)
        {
            for (int j = 0; j < m_nArrayHeight; j++)
            {
                if (m_aRoomArray[i, j].roomType == RoomType.NONE)
                {
                    m_aRoomArray[i, j].roomType = RoomType.BASIC;
                }
                Debug.Log(m_aRoomArray[i,j].roomType);
            }
        }
    }
    public void SpawnRooms()
    {
        for (int i = 0; i < m_nArrayWidth; i++)
        {
            for (int j = 0; j < m_nArrayHeight; j++)
            {
                switch (m_aRoomArray[i,j].roomType)
                {
                    case RoomType.NONE:

                        break;
                    case RoomType.BASIC:
                        Instantiate(m_gRoom, new Vector2(i * 40, j * 25), new Quaternion());
                        break;
                    case RoomType.SHOP:

                        break;
                    case RoomType.BOSS:

                        break;
                    case RoomType.START:
                        Instantiate(m_gStartRoom, new Vector2(i * 40, j * 25), new Quaternion());
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
