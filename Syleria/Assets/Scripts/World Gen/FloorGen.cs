using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomStruct
{
    public bool hasRoom;
    public int preX;
    public int preY;
    public Room room;
}

public class FloorGen : MonoBehaviour
{

    public GameObject cursorPrefab;
    public GameObject roomPrefab;

    public int numberOfRooms;
    public int arraySize;
    private int arrayCentre;
    public int roomSize;
    [Range(0, 100)]
    public int chanceReturnToCenter;

    private GameObject cursor;
    private int cursorX;
    private int cursorY;

    private int preX;
    private int preY;

    private RoomStruct[,] roomArray = new RoomStruct[1, 1];

    private int roomCounter = 0;
    //1 = place rooms. 2 = boss. 3 = store.
    private int phase = 0;
    private bool justPlaced = false;

    private int shopCorner;
    private bool shopReady = false;

    private bool bossReady = false;

    private int move;

    public int slowMo;
    private int currentSlowMo;
    public GameObject error;

    private void Start()
    {
        
        //Check array size is odd
        if(arraySize % 2 == 0)
        {
            arraySize++;
        }
        //Set arrayCentre
        arrayCentre = (arraySize - 1) / 2;

        error.transform.localPosition = new Vector3(arrayCentre/2 * roomSize, -arrayCentre * roomSize, 0);

        //Make new array
        roomArray = new RoomStruct[arraySize, arraySize];

        //Set each room to be empty
        for (int forX = 0; forX < arraySize; forX++)
        {
            for (int forY = 0; forY < arraySize; forY++)
            {
                roomArray[forX, forY].hasRoom = false;
            }
        }

        //Spawn cursor
        cursorX = arrayCentre;
        cursorY = arrayCentre;
        cursor = Instantiate(cursorPrefab, new Vector3((cursorX - arrayCentre) * roomSize, (cursorY - arrayCentre) * roomSize, 0) + transform.position, Quaternion.identity, transform) as GameObject;


        //Spawn Center Room
        roomArray[arrayCentre, arrayCentre].hasRoom = true;
        GameObject centreRoom = Instantiate(roomPrefab, new Vector3(0, 0, 0) + transform.position, Quaternion.identity, transform) as GameObject;
        roomArray[arrayCentre, arrayCentre].room = centreRoom.GetComponent<Room>();
        roomArray[arrayCentre, arrayCentre].room.centreOn = true;
        roomCounter++;
        phase = 1;
    }

    private void Update()
    {
        if (currentSlowMo>=slowMo)
        {
            currentSlowMo = 0;
            //Still placing normal rooms
            if (phase == 1)
            {
                //If there's a room under the cursor, move the cursor
                if (roomArray[cursorX, cursorY].hasRoom)
                {
                    if (!justPlaced)
                    {
                        preX = cursorX;
                        preY = cursorY;
                        move = Random.Range((int)0, (int)4);
                        switch (move)
                        {
                            case 0:
                                if (cursorX + 1 > arraySize-1)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorX++;
                                break;
                            case 1:
                                if (cursorX - 1 <0)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorX--;
                                break;
                            case 2:
                                if (cursorY + 1 > arraySize-1)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY++;
                                break;
                            case 3:
                                if (cursorY - 1 < 0)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY--;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        justPlaced = false;
                    }
                }
                else
                {
                    //Make New Room
                    roomArray[cursorX, cursorY].hasRoom = true;
                    GameObject newRoom = Instantiate(roomPrefab, new Vector3((cursorX - arrayCentre) * roomSize, (cursorY - arrayCentre) * roomSize, 0) + transform.position, Quaternion.identity, transform) as GameObject;
                    roomArray[cursorX, cursorY].room = newRoom.GetComponent<Room>();
                    roomArray[cursorX, cursorY].preX = preX;
                    roomArray[cursorX, cursorY].preY = preY;
                    switch (move)
                    {
                        case 0:
                            roomArray[cursorX, cursorY].room.openWest = true;
                            roomArray[preX, preY].room.openEast = true;
                            break;
                        case 1:
                            roomArray[cursorX, cursorY].room.openEast = true;
                            roomArray[preX, preY].room.openWest = true;
                            break;
                        case 2:
                            roomArray[cursorX, cursorY].room.openSouth = true;
                            roomArray[preX, preY].room.openNorth = true;
                            break;
                        case 3:
                            roomArray[cursorX, cursorY].room.openNorth = true;
                            roomArray[preX, preY].room.openSouth = true;
                            break;
                        default:
                            break;
                    }

                    //Move Cursor
                    if (Random.Range(0, 100) < chanceReturnToCenter)
                    {
                        cursorX = arrayCentre;
                        cursorY = arrayCentre;
                    }
                    justPlaced = true;

                    roomCounter++;
                    if (roomCounter >= numberOfRooms)
                    {
                        phase = 2;
                        shopCorner = Random.Range((int)0, (int)4);
                        switch (shopCorner)
                        {
                            case 0:
                                cursorX = 0;
                                cursorY = 0;
                                break;
                            case 1:
                                cursorX = arraySize - 1;
                                cursorY = 0;
                                break;
                            case 2:
                                cursorX = 0;
                                cursorY = arraySize - 1;
                                break;
                            case 3:
                                cursorX = arraySize - 1;
                                cursorY = arraySize - 1;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else if (phase == 2)
            {
                if (shopReady)
                {
                    if (roomArray[cursorX, cursorY].hasRoom)
                    {
                        preX = cursorX;
                        preY = cursorY;
                        switch (shopCorner)
                        {
                            case 0:
                                if (cursorY - 1 < 0)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY--;
                                break;
                            case 1:
                                if (cursorY - 1 < 0)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY--;
                                break;
                            case 2:
                                if (cursorY + 1 > arraySize-1)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY++;
                                break;
                            case 3:
                                if (cursorY + 1 > arraySize-1)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY++;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        roomArray[cursorX, cursorY].hasRoom = true;
                        GameObject newRoom = Instantiate(roomPrefab, new Vector3((cursorX - arrayCentre) * roomSize, (cursorY - arrayCentre) * roomSize, 0) + transform.position, Quaternion.identity, transform) as GameObject;
                        roomArray[cursorX, cursorY].room = newRoom.GetComponent<Room>();
                        roomArray[cursorX, cursorY].preX = preX;
                        roomArray[cursorX, cursorY].preY = preY;
                        switch (shopCorner)
                        {
                            case 0:
                                roomArray[cursorX, cursorY].room.openNorth = true;
                                roomArray[preX, preY].room.openSouth = true;
                                break;
                            case 1:
                                roomArray[cursorX, cursorY].room.openNorth = true;
                                roomArray[preX, preY].room.openSouth = true;
                                break;
                            case 2:
                                roomArray[cursorX, cursorY].room.openSouth = true;
                                roomArray[preX, preY].room.openNorth = true;
                                break;
                            case 3:
                                roomArray[cursorX, cursorY].room.openSouth = true;
                                roomArray[preX, preY].room.openNorth = true;
                                break;
                            default:
                                break;
                        }
                        roomArray[cursorX, cursorY].room.shopOn = true;

                        phase = 3;
                        switch (shopCorner)
                        {
                            case 0:
                                cursorX = arraySize - 1;
                                cursorY = arraySize - 1;
                                break;
                            case 1:
                                cursorX = 0;
                                cursorY = arraySize - 1;
                                break;
                            case 2:
                                cursorX = arraySize - 1;
                                cursorY = 0;
                                break;
                            case 3:
                                cursorX = 0;
                                cursorY = 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    if (!roomArray[cursorX, cursorY].hasRoom)
                    {
                        switch (shopCorner)
                        {
                            case 0:
                                cursorX++;
                                cursorY++;
                                break;
                            case 1:
                                cursorX--;
                                cursorY++;
                                break;
                            case 2:
                                cursorX++;
                                cursorY--;
                                break;
                            case 3:
                                cursorX--;
                                cursorY--;
                                break;
                        }
                    }
                    else
                    {
                        shopReady = true;
                    }
                }
            }
            else if (phase == 3)
            {
                if (bossReady)
                {
                    if (roomArray[cursorX, cursorY].hasRoom)
                    {
                        preX = cursorX;
                        preY = cursorY;
                        switch (shopCorner)
                        {
                            case 0:
                                if (cursorY + 1 > arraySize - 1)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY++;
                                break;
                            case 1:
                                if (cursorY + 1 > arraySize - 1)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY++;
                                break;
                            case 2:
                                if (cursorY - 1 < 0)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY--;
                                break;
                            case 3:
                                if (cursorY - 1 < 0)
                                {
                                    Debug.Log("ERROR");
                                    error.SetActive(true);
                                    phase = 4;
                                    break;
                                }
                                cursorY--;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        roomArray[cursorX, cursorY].hasRoom = true;
                        GameObject newRoom = Instantiate(roomPrefab, new Vector3((cursorX - arrayCentre) * roomSize, (cursorY - arrayCentre) * roomSize, 0) + transform.position, Quaternion.identity, transform) as GameObject;
                        roomArray[cursorX, cursorY].room = newRoom.GetComponent<Room>();
                        roomArray[cursorX, cursorY].room.bossOn = true;
                        roomArray[cursorX, cursorY].preX = preX;
                        roomArray[cursorX, cursorY].preY = preY;
                        switch (shopCorner)
                        {
                            case 0:
                                roomArray[cursorX, cursorY].room.openSouth = true;
                                roomArray[preX, preY].room.openNorth = true;
                                break;
                            case 1:
                                roomArray[cursorX, cursorY].room.openSouth = true;
                                roomArray[preX, preY].room.openNorth = true;
                                break;
                            case 2:
                                roomArray[cursorX, cursorY].room.openNorth = true;
                                roomArray[preX, preY].room.openSouth = true;
                                break;
                            case 3:
                                roomArray[cursorX, cursorY].room.openNorth = true;
                                roomArray[preX, preY].room.openSouth = true;
                                break;
                            default:
                                break;
                        }

                        phase = 4;
                    }
                }
                else
                {
                    if (!roomArray[cursorX, cursorY].hasRoom)
                    {
                        switch (shopCorner)
                        {
                            case 0:
                                cursorX--;
                                cursorY--;
                                break;
                            case 1:
                                cursorX++;
                                cursorY--;
                                break;
                            case 2:
                                cursorX--;
                                cursorY++;
                                break;
                            case 3:
                                cursorX++;
                                cursorY++;
                                break;
                        }
                    }
                    else
                    {
                        bossReady = true;
                    }
                }
            }
            else if (phase == 4)
            {
                cursorX = 0;
                cursorY = 0;
            }
        }
        else
        {
            currentSlowMo++;
        }
        cursor.transform.position = Vector3.Lerp(cursor.transform.position, new Vector3((cursorX - arrayCentre) * roomSize, (cursorY - arrayCentre) * roomSize, 0) + transform.position,0.7f);
    }


}
