//--------------------------------------------------------------------------------------
// Purpose: Creation and handling of a* grid
//
// Description:  Handles all creation of grid, nodes and assigning neighbours 
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStarGrid : MonoBehaviour
{

    public List<Node> FinalPath; // Final path to target
    private Node[,] m_grid; // 2d array of nodes to represent grid
    public int m_nGridHeight = 1; // Grid height
    public int m_nGridWidth = 1; // Grid width
    public LayerMask m_WalkableLayer; // Layers of all walkable tiles
    public GameObject m_MinimapSprite;
    private List<Node> NeighborList;
    private Vector2 m_nStartPos;
    private GameObject m_gMinimapParent;
    // Use this for initialization
    void Awake()
    {
        SetPosition(); // Sets position of grid based on rooms positions
        CreateGrid(); // creates grid on wake up
        NeighborList = new List<Node>(1);
        m_nStartPos = new Vector2();
    }

    public void CreateGrid()
    {
        m_grid = new Node[m_nGridWidth, m_nGridHeight];
        m_gMinimapParent = new GameObject("MinimapTiles");
        for (int i = 0; i < m_nGridWidth; i++)
        {
            for (int j = 0; j < m_nGridHeight; j++)
            {
                m_grid[i, j] = new Node(new Vector2(), false, 0, 0);
                m_grid[i, j].Walkable = Physics2D.Raycast((this.transform.position + new Vector3(i, j)) + new Vector3(0.5f, 0.5f, -1), Vector3.forward, 0.1f, m_WalkableLayer);
                m_grid[i, j].IndexX = i;
                m_grid[i, j].IndexY = j;
                m_grid[i, j].WorldPosition = this.transform.position + new Vector3(i, j) + new Vector3(0.5f, 0.5f, 0);
                //Debug.DrawRay((this.transform.position + new Vector3(i, j)) + new Vector3(0.5f, 0.5f, -1), Vector3.forward, Color.red, 10.0f);

                if (m_grid[i, j].Walkable)
                {                   
                    Instantiate(m_MinimapSprite, (this.transform.position + new Vector3(i, j) + new Vector3(0.5f, 0.5f, 0)), new Quaternion(), m_gMinimapParent.transform);
                }
            }
        }
    }

    public void CreateGrid(int gridWidth, int GridHeight)
    {
        m_grid = new Node[gridWidth, GridHeight];
        m_gMinimapParent = new GameObject("MinimapTiles");
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < GridHeight; j++)
            {
                m_grid[i, j] = new Node(new Vector2(), false, 0, 0);
                m_grid[i, j].Walkable = Physics2D.Raycast((this.transform.position + new Vector3(i, j)) + new Vector3(0.5f, 0.5f, -1), Vector3.forward, 0.1f, m_WalkableLayer);
                m_grid[i, j].IndexX = i;
                m_grid[i, j].IndexY = j;
                m_grid[i, j].WorldPosition = this.transform.position + new Vector3(i, j) + new Vector3(0.5f, 0.5f, 0);
                //Debug.DrawRay((this.transform.position + new Vector3(i, j)) + new Vector3(0.5f, 0.5f, -1), Vector3.forward, Color.red, 10.0f);

                if (m_grid[i, j].Walkable)
                {
                    Instantiate(m_MinimapSprite, (this.transform.position + new Vector3(i, j) + new Vector3(0.5f, 0.5f, 0)), new Quaternion(), m_gMinimapParent.transform);
                }
            }
        }
    }


    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        NeighborList.Clear();
        int icheckX; // Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY; // Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.IndexX + 1;
        icheckY = a_NeighborNode.IndexY;
        if (icheckX >= 0 && icheckX < m_nGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < m_nGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(m_grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.IndexX - 1;
        icheckY = a_NeighborNode.IndexY;
        if (icheckX >= 0 && icheckX < m_nGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < m_nGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(m_grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.IndexX;
        icheckY = a_NeighborNode.IndexY + 1;
        if (icheckX >= 0 && icheckX < m_nGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < m_nGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(m_grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.IndexX;
        icheckY = a_NeighborNode.IndexY - 1;
        if (icheckX >= 0 && icheckX < m_nGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < m_nGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(m_grid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        return NeighborList;//Return the neighbors list.
    }

    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 WorldPosition)
    { 
        Vector2 tempPos = WorldPosition; // Temp pos of target
        tempPos.x = WorldPosition.x - 0.5f; // X world position
        tempPos.y = WorldPosition.y - 0.58f; // Y world position

        Vector2 RelativePos = tempPos - (Vector2)this.transform.position; // Relative position to grid position

        Vector2Int tile = new Vector2Int(Mathf.RoundToInt(RelativePos.x / 1), Mathf.RoundToInt(RelativePos.y / 1)); // Round position to closet int
        Node tempNode = m_grid[tile.x, tile.y]; // Gets node based on position
        if(tempNode.Walkable)
        {
            return tempNode; // Returns node
        }
        else
        {
            List<Node> tempList = GetNeighboringNodes(tempNode);
            float tempDist = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                if(tempList[i].Walkable)
                {
                    if (Vector2.Distance(tempList[i].WorldPosition, tempNode.WorldPosition) <= tempDist)
                    {
                        tempNode = tempList[i];
                    }
                }
            }
            return tempNode;
        }
    }
    void SetPosition()
    {
        float highX = Mathf.NegativeInfinity; // Sets highX to lowest possible value
        float highY = Mathf.NegativeInfinity; // Sets highY to lowest possible value
        float lowX = Mathf.Infinity; // Sets lowX to highest possible value
        float lowY = Mathf.Infinity; // Sets lowY to highest possible value
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room"); // Finds all rooms in scene
        for (int i = 0; i < rooms.Length; i++) // Checks against every room in scene
        {
            float tempX = rooms[i].transform.position.x; // Saves rooms x position
            float tempY = rooms[i].transform.position.y; // Saves rooms y position
            if(tempX > highX) // If rooms x is higher then HighX
            {
                highX = tempX; // HighX = tempX
            }
            else if(tempX < lowX) // If lower
            {
                lowX = tempX; // LowX is now rooms x
            }
            else
            {

            }

            if(tempY > highY) // if rooms y is higher
            {
                highY = tempY; // Sets HighY to rooms y
            }
            else if(tempY < lowY) // If lower
            {
                lowY = tempY; // Sets lowY to rooms y
            }
            else
            {

            }
        }
        lowX = lowX - 50.0f; // Takes 50 off lowx
        lowY = lowY - 50.0f; // Takes 50 off lowy
        highX = highX + 150.0f; // adds 150 to high x
        highY = highY + 150.0f; // Adds 150 to high y
        m_nStartPos.x = lowX - 50f; 
        m_nStartPos.y = lowY - 50f;
        this.transform.position = m_nStartPos; // Sets grid position to bottom left of rooms based on low x and low y
        m_nGridHeight = (int)Vector2.Distance(new Vector2(0,lowY), new Vector2(0, highY)); // Sets grid height to the distance from lowY to highY
        m_nGridWidth = (int)Vector2.Distance(new Vector2(lowX, 0), new Vector2(highX, 0)); // Sets grid width to the distance from lowX to highX
    }

}
