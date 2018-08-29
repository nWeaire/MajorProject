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
    public GameObject m_Door;
    public SpawnPoints[] m_aSpawnPoints;
    public bool m_bRoomActive = false;
    public bool m_bSpawned = false;
    private GameObject[] m_enemy;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(m_bRoomActive && !m_bSpawned)
        {
            m_Door.SetActive(true);
            for (int i = 0; i < m_aSpawnPoints.Length; i++)
            {
                m_aSpawnPoints[i].SpawnEnemy(0);
            }
            m_bSpawned = true;
            m_enemy = GameObject.FindGameObjectsWithTag("Enemy");
        }
        m_enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (m_enemy.Length <= 0 && m_bRoomActive)
        {
            m_Door.SetActive(false);
            Destroy(this.gameObject);
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
