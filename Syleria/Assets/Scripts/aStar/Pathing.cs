//--------------------------------------------------------------------------------------
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
    public List<Node> FinalPath; // Final path for a*
    List<Node> OpenList; // Openlist of nodes
    HashSet<Node> ClosedList; // Closed list of nodes

    // Use this for initialization
    void Awake ()
    {
        OpenList = new List<Node>(1); // Initalizes open list of nodes
        FinalPath = new List<Node>(1); // Initalizes final path of nodes
        ClosedList = new HashSet<Node>(); // Initalizes closedlist of nodes
        m_grid = GetComponent<aStarGrid>(); // Gets reference to grid
	}

    //--------------------------------------------------------------
    //  finds the path of nodes to the end position from the start position
    //  Parameters:
    //      startPos: Starting position of path
    //      targetPos: Target position of path
    //  Returns:
    //      List<Node> : Returns list of nodes from the start position to the target position
    //--------------------------------------------------------------
    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node StartNode = m_grid.NodeFromWorldPoint(startPos);//Gets the node closest to the starting position
        Node TargetNode = m_grid.NodeFromWorldPoint(targetPos);//Gets the node closest to the target position

        OpenList.Clear();
        ClosedList.Clear();

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

            foreach (Node NeighborNode in m_grid.GetNeighbouringNodes(CurrentNode))//Loop through each neighbor of the current node
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

    //--------------------------------------------------------------
    //  finds the path of nodes to the end node from the start node
    //  Parameters:
    //      startNode: Starting node
    //      endNode: end node
    //  Returns:
    //      List<Node> : Returns list of nodes from the start node to the end node
    //--------------------------------------------------------------
    public List<Node> GetFinalPath(Node startingNode, Node endNode)
    {
        Node CurrentNode = endNode;//Node to store the current node being checked

        while (CurrentNode != startingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order
        for (int i = 0; i < FinalPath.Count; i++)
        {
            //Debug.Log(FinalPath[i].WorldPosition);
        }
        return m_grid.FinalPath = FinalPath;//Set the final path

    }

    //--------------------------------------------------------------
    //  Finds the manhatten distance between to nodes
    //  Parameters:
    //      Node NodeA: First Node to check manhatten distance
    //      Node NodeB: Second Node to check manhatten distance
    //  Returns:
    //      int: The distance from nodeA to NodeB
    //--------------------------------------------------------------
    public int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.IndexX - nodeB.IndexX);//x1-x2
        int iy = Mathf.Abs(nodeA.IndexY - nodeB.IndexY);//y1-y2

        return ix + iy;//Return the sum
    }

}
