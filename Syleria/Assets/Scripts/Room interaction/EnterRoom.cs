//--------------------------------------------------------------------------------------
// Purpose: Room logic
//
// Description:  locks room and spawns enemies when player triggers it
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoom : MonoBehaviour
{
    public GameObject[] m_aSpawnPoints; // List of spawn points to interact with
    private GameObject[] m_aRooms;
    private GameObject[] m_aDoors;
    public bool m_bRoomActive = false; // If the room is active
    public bool m_bSpawned = false; // If the room was spawned
    private GameObject[] m_enemy; // List of enemies to check
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_aDoors = this.GetComponentInParent<Corridors>().Door;
        int floorNum = this.GetComponentInParent<WorldGeneration>().m_nFloorNum;
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy"); // Finds all enemies in scene
        if (m_bRoomActive) // If room is active
        {

            if (m_aDoors[floorNum])
            {
                m_aDoors[floorNum].SetActive(true); // Sets doors to active
            }           
            for (int i = 0; i < m_aSpawnPoints.Length; i++) // Loops through spawn points
            {
                m_aSpawnPoints[i].SetActive(true); // Sets spawn points to true
            }
        }
        for (int i = 0; i < m_aSpawnPoints.Length; i++) // Loops through Spawn points
        {
            if (m_aSpawnPoints[i].GetComponent<SpawnPoints>().m_bDone == true && m_enemy.Length <= 0) // Checks if rooms are done and there are no enemies
            {
                
                for (int j = 0; j < m_aDoors.Length; j++) // Loops through list of doors
                {
                    m_aDoors[j].SetActive(false); // Sets doors to active
                }
                Destroy(this.gameObject); // Deletes this game object
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Checks if colliding with player
        {
            m_bRoomActive = true; // Sets room to active
        }
    }
}
