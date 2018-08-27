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

public class aStarGrid : MonoBehaviour {
    
    public List<Node> FinalPath; // Final path to target
    private Node[,] m_grid; // 2d array of nodes to represent grid
    public int m_nGridHeight = 1; // Grid height
    public int m_nGridWidth = 1; // Grid width
    public LayerMask m_WalkableLayer; // Layers of all walkable tiles

	// Use this for initialization
	void Awake ()
    {
        CreateGrid(); // creates grid on wake up
	}

    public void CreateGrid()
    {
        m_grid = new Node[m_nGridWidth, m_nGridHeight];
        for (int i = 0; i < m_nGridWidth; i++)
        {
            for (int j = 0; j < m_nGridHeight; j++)
            {
                m_grid[i, j] = new Node(new Vector2(), false, 0, 0);
                m_grid[i, j].Walkable = Physics2D.Raycast((this.transform.position + new Vector3(i, j)) + new Vector3(0.5f, 0.5f, -1), Vector3.forward, 0.1f, m_WalkableLayer);
                m_grid[i, j].IndexX = i;
                m_grid[i, j].IndexY = j;
                m_grid[i, j].WorldPosition = this.transform.position + new Vector3(i, j) + new Vector3(0.5f, 0.5f , 0);
                Debug.DrawRay((this.transform.position + new Vector3(i, j)) + new Vector3(0.5f, 0.5f, -1), Vector3.forward, Color.red, 10.0f);

                if (m_grid[i, j].Walkable)
                {
                    
                    //Instantiate(m_gNode, (this.transform.position + new Vector3(i, j) + new Vector3(0.5f, 0.5f, 0)), new Quaternion());
                }
            }
        }
    }

    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

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
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + m_nGridWidth / 2) / m_nGridWidth);
        float iyPos = ((a_vWorldPos.y + m_nGridHeight / 2) / m_nGridHeight);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((m_nGridWidth - 1) * ixPos);
        int iy = Mathf.RoundToInt((m_nGridHeight - 1) * iyPos);

        return m_grid[ix, iy];
    }

    // Update is called once per frame
    void Update () {
		
	}
}
