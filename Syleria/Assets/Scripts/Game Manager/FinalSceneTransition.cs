//--------------------------------------------------------------------------------------
// Purpose: Loading final level
//
// Description:  Saves player data and loads new scene
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FinalSceneTransition : MonoBehaviour {

    private GameObject m_gRunTimeObjects; // Reference to all run time objects
    private GameObject m_gFinalRoom; // Reference to the final room
    private GameObject m_gAStar; // Reference to the A* controller


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------------
        // If colliding with player
        // Finds all references to run time objects, final room and a*
        // Sets position of run time objects to the final rooms position
        // Sets a* position to the bottom left of the final room
        // Recalculates the a* grid
        //--------------------------------------------------------------
        if (collision.gameObject.tag == "Player")
        {
            m_gRunTimeObjects = GameObject.FindGameObjectWithTag("SceneObjects"); // Finds all runtime objects
            m_gFinalRoom = GameObject.FindGameObjectWithTag("FinalRoom"); // Finds the final room
            m_gRunTimeObjects.transform.position = m_gFinalRoom.transform.position; // Sets runtime objects position to the final rooms
            m_gAStar = GameObject.FindGameObjectWithTag("A*"); // find a* object
            m_gAStar.transform.position = m_gFinalRoom.transform.position + new Vector3(-100, -100, 0); // Sets a star to bottom left of the final rooms position
            m_gAStar.GetComponent<aStarGrid>().CreateGrid(200, 200); // Creates new a* grid
        }
    }
}
