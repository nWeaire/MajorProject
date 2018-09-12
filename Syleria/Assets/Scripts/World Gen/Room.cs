using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int indexX;
    public int indexY;
    public Vector2 worldPosition;
    public bool isBoss = false;
    public bool isShop = false;
    public bool isRoom = false;
    public bool isSpawn = false;

    public GameObject room;

    public Room(int indX, int indY, Vector2 worldpos)
    {
        indexX = indX;
        indexY = indY;
        worldPosition = worldpos;
    }

}
