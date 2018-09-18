//--------------------------------------------------------------------------------------
// Purpose: Enemy spawn point logic
//
// Description:  Holds list of enemies to spawn
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour {

    public GameObject[] m_aEnemies; // Enemies to spawn
    public int m_nWaveNumber = 0; // Number of waves
    public int m_nCurrentWave = 0; // Current wave
    public bool m_bDone; // Checks if done
    GameObject m_gEnemy; // Current enemy
    private void Awake()
    {
        m_nWaveNumber = m_aEnemies.Length; // Wave number set to number of enemies in array
    }

    private void Update()
    {
        if (m_nCurrentWave < m_nWaveNumber) // If current wave isn't wave number
        {
            if(m_gEnemy == null) // Checks if enemy already exists
            {
                m_gEnemy = Instantiate(m_aEnemies[m_nCurrentWave], this.transform.position, new Quaternion()); // Instantiates enemy and saves to current enemy
                m_nCurrentWave += 1; // Adds to current wave
            }
        }
        if(m_nCurrentWave >= m_nWaveNumber) // If current wave is above wave number
        { 
            m_bDone = true; // Sets to done
        }
        

    }

}
