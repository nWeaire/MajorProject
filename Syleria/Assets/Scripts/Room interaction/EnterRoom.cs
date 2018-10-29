//--------------------------------------------------------------------------------------
// Purpose: Room logic
//
// Description:  locks room and spawns enemies when player triggers it
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoom : MonoBehaviour
{
    public GameObject[] m_aSpawnPoints; // List of spawn points to interact with
    private GameObject[] m_aDoors;
    public GameObject m_gLadder;
    [HideInInspector] public bool m_bRoomActive = false; // If the room is active
    [HideInInspector] public bool m_bSpawned = false; // If the room was spawned
    public bool m_bIsBoss = false;
    public bool m_bIsFinalRoom = false;
    private bool m_bInitalStart = false;
    private int m_nFloorNum;
    private int m_nWaveNum = 0;
    private int m_nPointsCompleted = 0;
    private int m_nWavesCompleted = 0;
    private bool m_bWaveSpawned;
    public GameObject m_gRoomCompleted;
    private GameObject[] m_enemy; // List of enemies to check
    public bool m_bIsGauntletRoom = false;

    // Update is called once per frame
    void Update()
    {
        m_aDoors = this.GetComponentInParent<Corridors>().Door; //Gets all doors from room
        if(!m_bIsGauntletRoom) // If not a gauntlet room
        {
            m_nFloorNum = this.GetComponentInParent<WorldGeneration>().m_nFloorNum; // Gets current floor number
        }
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy"); // Finds all enemies in scene

        if (m_bRoomActive && !m_bWaveSpawned) // If room is active
        {
            if(!m_bInitalStart) // If the room hasn't been activated before
            {
                TurnOnDoors(); // Turns on doors for the room
                // Sets current companions position to the players position
                GameObject.FindGameObjectWithTag("Companion").transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                m_bInitalStart = true; // Sets inital start to true
            }
            for (int i = 0; i < m_aSpawnPoints.Length; i++) // For all spawn points 
            {
                m_aSpawnPoints[i].GetComponent<SpawnPoints>().SpawnWave(m_nWaveNum); // Spawns current wave
            }
            m_bWaveSpawned = true; // Sets wave spawned to true
        }

        if (m_bWaveSpawned) // If wave is spawned
        {
            for (int i = 0; i < m_aSpawnPoints.Length; i++) // for each spawn point
            {
                if (m_aSpawnPoints[i].GetComponent<SpawnPoints>().CheckWave(m_nWaveNum)) // Check if wave is complete
                {
                    m_nPointsCompleted += 1; // If wave is complete add to points completed
                }

                if (m_nPointsCompleted >= m_aSpawnPoints.Length) // If points completed is equal to or more then the number of spawn points
                {
                    m_nPointsCompleted = 0;
                    m_bWaveSpawned = false; // Sets wave spawned to false
                    m_nWaveNum += 1; // Adds one to the wave number
                }
            }
            m_nPointsCompleted = 0; // If not completed set points completed to false
        }

        if (m_bRoomActive) // If room is active
        {
            for (int i = 0; i < m_aSpawnPoints.Length; i++) // For all spawn points
            {
                if (m_aSpawnPoints[i].GetComponent<SpawnPoints>().CheckDone(m_nWaveNum)) // Checks if all waves are completed for the spawn point
                {
                    m_nWavesCompleted += 1; // Adds one to the spawn point
                }
                if (m_nWavesCompleted >= m_aSpawnPoints.Length) // If waves completed is equal to more then number of spawn points
                {
                    TurnOffDoors(); // Turns off doors to room
                    this.gameObject.SetActive(false); // Turns off this script
                }
            }
            m_nWavesCompleted = 0; // If not completed sets waves completed to false to reset count
        }
    }


    //--------------------------------------------------------------
    // Handles logic for turning doors on for the current floor number
    //--------------------------------------------------------------
    private void TurnOnDoors()
    {
        if (m_bIsGauntletRoom) // If gauntlet room turns on all doors
        {
            for (int i = 0; i < m_aDoors.Length; i++) // For all doors
            {
                m_aDoors[i].SetActive(true); // Sets doors to true
            }
        }
        else // If not gauntlet room turns on the connecting doors
        {
            if (m_aDoors[m_nFloorNum]) // If not null
            {
                m_aDoors[m_nFloorNum].SetActive(true); // Sets doors to active
            }
        }
    }

    //--------------------------------------------------------------
    // Handles logic for turning doors off for the current floor number
    //--------------------------------------------------------------
    private void TurnOffDoors()
    {
        for (int j = 0; j < m_aDoors.Length; j++) // Loops through list of doors
        {
            m_aDoors[j].SetActive(false); // Sets doors to active
        }
        if(m_gRoomCompleted != null) // If sprite is not null
        {
            m_gRoomCompleted.SetActive(true); // Room complete sprite to true
        }
        if(m_bIsBoss) // if boss room spawn ladder to next floor
        {
            m_gLadder.SetActive(true);
        }
    }

    //--------------------------------------------------------------
    // When colliding with player
    // Sets room to active and begins update sequence
    //--------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // Checks if colliding with player
        {
            m_bRoomActive = true; // Sets room to active
        }
    }
}
