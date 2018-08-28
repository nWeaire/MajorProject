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
    public bool m_bWavedead;
    private void Start()
    {
        m_nWaveNumber = m_aEnemies.Length;
    }

    public void SpawnEnemy(int waveNumber)
    {
        if(m_aEnemies.Length > 0)
        {
            Instantiate(m_aEnemies[waveNumber], this.transform.position, new Quaternion());
        }
    }



    
}
