//--------------------------------------------------------------------------------------
// Purpose: Handles all room logic
//
// Description:  Handles room logic for world generation
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public int indexX; // X index in array
    public int indexY; // Y index in array
    public Vector2 worldPosition; // World position of room
    public bool isBoss = false; // Is boss room
    public bool isShop = false; // Is shop room
    public bool isRoom = false; // Is a room
    public bool isSpawn = false; // Is spawn room

    public GameObject room; // GameObject for room

    public Room(int indX, int indY, Vector2 worldpos) // base constructor
    {
        indexX = indX;
        indexY = indY;
        worldPosition = worldpos;
    }

}
