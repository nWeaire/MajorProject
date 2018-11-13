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

public class SpawnPoints : MonoBehaviour
{

    public GameObject[] m_aEnemies; // Enemies to spawn
    private int m_nWaveNumber = 0; // Number of waves
    [HideInInspector] public int m_nCurrentWave = 0; // Current wave
    private GameObject m_gEnemy; // Current enemy
    private void Awake()
    {
        m_nWaveNumber = m_aEnemies.Length; // Wave number set to number of enemies in array
    }

    //--------------------------------------------------------------------------------------
    // If all waves are not completed 
    // Instaniates enemies from current wave to scene
    // Parameters
    //      int WaveNumber: Current wave number
    //--------------------------------------------------------------------------------------
    public void SpawnWave(int waveNumber)
    {
        if (waveNumber < m_nWaveNumber) // If current wave isn't wave number
        {
            m_gEnemy = Instantiate(m_aEnemies[waveNumber], this.transform.position, new Quaternion()); // Instantiates enemy and saves to current enemy
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks if Current enemy still exists
    // If enemy no longer exists then wave is complete, returns true
    // Parameters
    //      int WaveNumber: Current wave number
    // Returns
    //      Bool: True or false if waves are complete
    //--------------------------------------------------------------------------------------
    public bool CheckWave(int waveNumber)
    {
        if (m_gEnemy == null && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) // If enemy exists
        {
            return true; // Returns true
        }
        else
        {
            return false; // returns false
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks if all waves are completed
    // Parameters
    //      int WaveNumber: Current wave number
    // Returns
    //      Bool: True or false if waves are complete
    //--------------------------------------------------------------------------------------
    public bool CheckDone(int waveNumber)
    {
        if (waveNumber >= m_nWaveNumber) // If current wave is above wave number
        {
            return true;
        }
        else return false;
    }
}
