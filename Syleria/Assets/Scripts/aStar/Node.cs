//--------------------------------------------------------------------------------------
// Purpose: Creation and variables for nodes
//
// Description:  Holds all information of nodes for the a* grid
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool Walkable; // If the nodes is available for pathing
    public int IndexX; // X index of node in array
    public int IndexY; // Y index of node in array
    public Vector2 WorldPosition; // World position of node

    public Node ParentNode; // Nodes parent

    public int igCost; // The cost of moving to the next square.
    public int ihCost; // The distance to the goal from this node.

    public int FCost { get { return igCost + ihCost; } } // Get function for f Cost

    //--------------------------------------------------------------------------------------
    // Base constructor for nodes in array, assigning all basic variables
    //--------------------------------------------------------------------------------------
    public Node(Vector2 worldPosition, bool walkable, int indexX, int indexY)
    {
        Walkable = walkable;
        IndexX = indexX;
        IndexY = indexY;
        WorldPosition = worldPosition;
    }
}
