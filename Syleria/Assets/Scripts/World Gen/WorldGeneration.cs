using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {

    public class Room
    {
        int indexX;
        int indexY;
        Vector2 worldPosition;
        bool isBoss;
        bool isShop;

        public Room(int indX, int indY, Vector2 worldpos)
        {
            indexX = indX;
            indexY = indY;
            worldPosition = worldpos;
        }

    };

    private int m_nArrayHeight;
    private int m_nArrayWidth;
    private Room[,] m_aRoomArray;

    [SerializeField] private int m_nRoomCount;
    [SerializeField] private GameObject m_gRoom;
    [SerializeField] private GameObject m_gStartRoom;
    [SerializeField] private Vector2 m_nRoomSize;


	// Use this for initialization
	void Start ()
    {
        

        Instantiate(m_gStartRoom, new Vector2(0, 0), new Quaternion(), this.transform);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

}
