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

    public void SpawnEnemy(int waveNumber)
    {
        if(m_aEnemies.Length > 0)
        {
            Instantiate(m_aEnemies[waveNumber], this.transform.position, new Quaternion());
        }
    }
    
}
