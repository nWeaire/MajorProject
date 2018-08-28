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

    public SpawnPoints[] m_aSpawnPoints;
    public bool m_bRoomActive = false;
    public bool m_bSpawned = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_bRoomActive && !m_bSpawned)
        {
            for (int i = 0; i < m_aSpawnPoints.Length; i++)
            {
                m_aSpawnPoints[i].SpawnEnemy(0);
            }
            m_bSpawned = true;
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
