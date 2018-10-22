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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_aDoors = this.GetComponentInParent<Corridors>().Door;
        m_nFloorNum = this.GetComponentInParent<WorldGeneration>().m_nFloorNum;
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy"); // Finds all enemies in scene

        if (m_bRoomActive && !m_bWaveSpawned) // If room is active
        {
            if(!m_bInitalStart)
            {
                TurnOnDoors();
                GameObject.FindGameObjectWithTag("Companion").transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
                m_bInitalStart = true;
            }
            for (int i = 0; i < m_aSpawnPoints.Length; i++)
            {
                m_aSpawnPoints[i].GetComponent<SpawnPoints>().SpawnWave(m_nWaveNum);
            }
            m_bWaveSpawned = true;
        }

        if (m_bWaveSpawned)
        {
            for (int i = 0; i < m_aSpawnPoints.Length; i++)
            {
                if (m_aSpawnPoints[i].GetComponent<SpawnPoints>().CheckWave(m_nWaveNum))
                {
                    m_nPointsCompleted += 1;
                }

                if (m_nPointsCompleted >= m_aSpawnPoints.Length)
                {
                    m_nPointsCompleted = 0;
                    m_bWaveSpawned = false;
                    m_nWaveNum += 1;
                }
            }
            m_nPointsCompleted = 0;
        }

        if (m_bRoomActive)
        {
            for (int i = 0; i < m_aSpawnPoints.Length; i++)
            {
                if (m_aSpawnPoints[i].GetComponent<SpawnPoints>().CheckDone(m_nWaveNum))
                {
                    m_nWavesCompleted += 1;
                }
                if (m_nWavesCompleted >= m_aSpawnPoints.Length)
                {
                    TurnOffDoors();
                    this.gameObject.SetActive(false);
                }
            }
            m_nWavesCompleted = 0;
        }
    }

    private void TurnOnDoors()
    {
        if (m_aDoors[m_nFloorNum])
        {
            m_aDoors[m_nFloorNum].SetActive(true); // Sets doors to active
        }
    }

    private void TurnOffDoors()
    {
        for (int j = 0; j < m_aDoors.Length; j++) // Loops through list of doors
        {
            m_aDoors[j].SetActive(false); // Sets doors to active
        }
        if(m_gRoomCompleted != null)
        {
            m_gRoomCompleted.SetActive(true);
        }
        if(m_bIsBoss)
        {
            m_gLadder.SetActive(true);
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
