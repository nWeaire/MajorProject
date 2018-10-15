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
    private GameObject[] m_atDoors; // List of doors to turn on
    private GameObject[] m_abDoors;
    private GameObject[] m_alDoors;
    private GameObject[] m_arDoors;
    public GameObject[] m_aSpawnPoints; // List of spawn points to interact with
    private GameObject[] m_aRooms;
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
        int floorNum = this.GetComponentInParent<WorldGeneration>().m_nFloorNum;
        m_atDoors = this.GetComponentInParent<Corridors>().topDoor;
        m_abDoors = this.GetComponentInParent<Corridors>().bottomDoor;
        m_alDoors = this.GetComponentInParent<Corridors>().leftDoor;
        m_arDoors = this.GetComponentInParent<Corridors>().rightDoor;
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy"); // Finds all enemies in scene
        if (m_bRoomActive) // If room is active
        {

            if (m_atDoors[floorNum].GetComponent<Door>().m_bIsUsed)
            {
                m_atDoors[floorNum].SetActive(true); // Sets doors to active
            }


            if (m_abDoors[floorNum].GetComponent<Door>().m_bIsUsed)
            {
                m_abDoors[floorNum].SetActive(true); // Sets doors to active
            }

            if (m_alDoors[floorNum].GetComponent<Door>().m_bIsUsed)
            {
                m_alDoors[floorNum].SetActive(true); // Sets doors to active
            }
            if (m_arDoors[floorNum].GetComponent<Door>().m_bIsUsed)
            {
                m_arDoors[floorNum].SetActive(true); // Sets doors to active
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
                for (int j = 0; j < m_atDoors.Length; j++) // Loops through list of doors
                {
                    m_atDoors[j].SetActive(false); // Sets doors to active
                }
                for (int j = 0; j < m_abDoors.Length; j++) // Loops through list of doors
                {
                    m_abDoors[j].SetActive(false); // Sets doors to active
                }
                for (int j = 0; j < m_alDoors.Length; j++) // Loops through list of doors
                {
                    m_alDoors[j].SetActive(false); // Sets doors to active
                }
                for (int j = 0; j < m_arDoors.Length; j++) // Loops through list of doors
                {
                    m_arDoors[j].SetActive(false); // Sets doors to active
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
