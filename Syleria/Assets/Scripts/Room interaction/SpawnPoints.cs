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
    public bool m_bWavedead = true;
    public bool m_bDone;
    GameObject m_gEnemy;
    private void Awake()
    {
        m_nWaveNumber = m_aEnemies.Length;
    }

    private void Update()
    {
        if (m_nCurrentWave < m_nWaveNumber)
        {
            if(m_gEnemy == null)
            {
                m_gEnemy = Instantiate(m_aEnemies[m_nCurrentWave], this.transform.position, new Quaternion());
                m_nCurrentWave += 1;
            }
        }
        if(m_nCurrentWave >= m_nWaveNumber)
        {
            m_bDone = true;
        }
        

    }

}
