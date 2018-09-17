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

    public GameObject[] m_aEnemies;
    public int m_nWaveNumber = 0;
    public int m_nCurrentWave = 0;
    public bool m_bWavedead;
    public bool m_bDone;
    private void Awake()
    {
        m_nWaveNumber = m_aEnemies.Length;
    }

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (m_nCurrentWave < m_nWaveNumber && enemies.Length <= 0)
        {
            Instantiate(m_aEnemies[m_nCurrentWave], this.transform.position, new Quaternion());
            m_nCurrentWave += 1;
        }
        else if(m_nCurrentWave >= m_nWaveNumber)
        {
            m_bDone = true;
        }
        

    }

}
