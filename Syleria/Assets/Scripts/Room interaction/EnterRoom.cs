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
    public GameObject[] m_Door;
    public GameObject[] m_aSpawnPoints;
    public bool m_bRoomActive = false;
    public bool m_bSpawned = false;
    private GameObject[] m_enemy;
    private int m_nRoomsDone = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_bRoomActive && !m_bSpawned)
        {
            for (int i = 0; i < m_Door.Length; i++)
            {
                m_Door[i].SetActive(true);
            }
            for (int i = 0; i < m_aSpawnPoints.Length; i++)
            {
                m_aSpawnPoints[i].SetActive(true);
            }
        }
        for (int i = 0; i < m_aSpawnPoints.Length; i++)
        {
            if (m_aSpawnPoints[i].GetComponent<SpawnPoints>().m_bDone == true)
            {
                for (int j = 0; j < m_Door.Length; j++)
                {
                    m_Door[j].SetActive(false);
                }
                Destroy(this.gameObject);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bRoomActive = true;
        }
    }
}
