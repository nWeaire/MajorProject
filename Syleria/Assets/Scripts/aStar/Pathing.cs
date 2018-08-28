﻿//--------------------------------------------------------------------------------------
// Purpose: Creation and handling for pathing to target node
//
// Description:  Finds paths to target through a* grid
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour {

    public aStarGrid m_grid; // Reference to a* grid

    // Use this for initialization
    void Start ()
    {
        m_grid = GetComponent<aStarGrid>(); // Gets reference to grid
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    public List<Node> FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = m_grid.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
        Node TargetNode = m_grid.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position

        List<Node> OpenList = new List<Node>();//List of nodes for the open list
        HashSet<Node> ClosedList = new HashSet<Node>();//Hashset of nodes for the closed list

        OpenList.Add(StartNode);//Add the starting node to the open list to begin the program

        while (OpenList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//Remove that from the open list
            ClosedList.Add(CurrentNode);//And add it to the closed list

            if (CurrentNode == TargetNode)//If the current node is the same as the target node
            {
                return GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            foreach (Node NeighborNode in m_grid.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.Walkable || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.igCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.igCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.ihCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }
           
        }
        return null;
    }

    List<Node> GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order
        for (int i = 0; i < FinalPath.Count; i++)
        {
            Debug.Log(FinalPath[i].WorldPosition);
        }
        return m_grid.FinalPath = FinalPath;//Set the final path

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.IndexX - a_nodeB.IndexX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.IndexY - a_nodeB.IndexY);//y1-y2

        return ix + iy;//Return the sum
    }

}
