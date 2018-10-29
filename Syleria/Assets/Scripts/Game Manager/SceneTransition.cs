//--------------------------------------------------------------------------------------
// Purpose: Transistioning to a new floor in the dungeon
//
// Description:  Turns off and deletes required game objects before rebuilding the entire level
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{

    private GameObject m_gSceneObjects; // Reference to all objects in scene
    private WorldGeneration m_gWorldGeneration; // Reference to world generation script
    private GameObject m_aMinimapTiles; // Reference to all minimap tiles


    //--------------------------------------------------------------
    // If colliding with player
    // Finds all references to run time objects, world generation and a*
    // Sets position of player and companion to the local position of zero
    // destroys a*
    // Adds one to the floor number
    // Destroys all rooms
    // Recreates world with world generation script
    //--------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_aMinimapTiles = GameObject.Find("MinimapTiles"); // Find and turns off all current minimap tiles
            m_aMinimapTiles.SetActive(false); 
            m_gSceneObjects = GameObject.FindGameObjectWithTag("SceneObjects"); // Finds all scene objects
            m_gWorldGeneration = GameObject.FindObjectOfType<WorldGeneration>(); // Finds world generation script
            GameObject.Find("Player").transform.localPosition = Vector2.zero; // Finds and sets local position of player to zero
            GameObject.FindGameObjectWithTag("Companion").transform.localPosition = Vector2.zero; // finds and sets companions local position to zero
            m_gSceneObjects.SetActive(false); // Sets scene objects to false            
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room"); // Finds all rooms in the scene
            Destroy(GameObject.FindGameObjectWithTag("A*")); // Destroys a*
            for (int i = 0; i < rooms.Length; i++) // For all rooms in scene
            {
                Destroy(rooms[i]); // Destroys all rooms
            }
            m_gWorldGeneration.m_nFloorNum += 1; // adds 1 to floor number
            m_gWorldGeneration.CreateWorld(); // Creates world

            Destroy(this.gameObject); // Destroys this game object
        }
    }
}
