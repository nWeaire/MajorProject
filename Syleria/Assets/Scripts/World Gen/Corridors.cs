//--------------------------------------------------------------------------------------
// Purpose: Handles all Corridor logic for rooms
//
// Description:  Holds all references to corridors and walls for rooms
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridors : MonoBehaviour {

   public GameObject topCorridor; // Reference to top corridor
   public GameObject bottomCorridor; // Reference to bottom corridor
   public GameObject leftCorridor; // Reference to left corridor
   public GameObject rightCorridor; // Reference to right corridor

   public GameObject topWall; // Reference to top wall
   public GameObject bottomWall; // Reference to bottom wall
   public GameObject leftWall; // Reference to left wall
   public GameObject rightWall; // Reference to right wall

    public bool isSpawn; // if spawn room
     
}
