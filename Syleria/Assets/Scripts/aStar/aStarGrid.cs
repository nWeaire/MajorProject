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
        float highX = Mathf.NegativeInfinity;
        float highY = Mathf.NegativeInfinity;
        float lowX = Mathf.Infinity;
        float lowY = Mathf.Infinity;
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < rooms.Length; i++)
        {
            float tempX = rooms[i].transform.position.x;
            float tempY = rooms[i].transform.position.y;
            if(tempX > highX)
            {
                highX = tempX;
            }
            else if(tempX < lowX)
            {
                lowX = tempX;
            }
            else
            {

            }

            if(tempY > highY)
            {
                highY = tempY;
            }
            else if(tempY < lowY)
            {
                lowY = tempY;
            }
            else
            {

            }
        }
        lowX = lowX - 50.0f;
        lowY = lowY - 50.0f;
        highX = highX + 150.0f;
        highY = highY + 150.0f;
        m_nStartPos.x = lowX - 50f;
        m_nStartPos.y = lowY - 50f;
        this.transform.position = m_nStartPos;
        m_nGridHeight = (int)Vector2.Distance(new Vector2(0,lowY), new Vector2(0, highY));
        m_nGridWidth = (int)Vector2.Distance(new Vector2(lowX, 0), new Vector2(highX, 0));
    }

}
