using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool Walkable;
    public int IndexX;
    public int IndexY;
    public Vector2 WorldPosition;

    public Node ParentNode;//For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

    public int igCost;//The cost of moving to the next square.
    public int ihCost;//The distance to the goal from this node.

    public int FCost { get { return igCost + ihCost; } }//Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.


    public Node(Vector2 worldPosition, bool walkable, int indexX, int indexY)
    {
        Walkable = walkable;
        IndexX = indexX;
        IndexY = indexY;
        WorldPosition = worldPosition;
    }
}
