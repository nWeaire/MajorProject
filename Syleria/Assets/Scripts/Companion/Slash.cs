//--------------------------------------------------------------------------------------
// Purpose: On the Slash object.
//
// Description:  Handles all logic based around the Slash.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    // Time until the Slash is deleted
    public float m_fSlashTime = 0.1f;

    // Timer for the Slash.
    private float m_fSlashTimer = 0;


    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Update timer 
        m_fSlashTimer += Time.deltaTime; 
        m_fSlashTimer = m_fSlashTimer % 60;

        // If timer > sprite length
        if (m_fSlashTimer >= m_fSlashTime) 
        {
            this.gameObject.SetActive(false); // Set this to false
            m_fSlashTimer = 0.0f;
        }
    }
}

