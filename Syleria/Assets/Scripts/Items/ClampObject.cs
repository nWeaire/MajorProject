//--------------------------------------------------------------------------------------
// Purpose: Sets objects position to player position
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampObject : MonoBehaviour {

    private GameObject m_gPlayer; // Reference to player object

    private void Awake()
    {
        m_gPlayer = GameObject.Find("Player"); // Find player
    }

    // Update is called once per frame
    void Update () {
        if (m_gPlayer) // If player exists 
            this.transform.position = m_gPlayer.transform.position; // Sets this position to player position
	}
}
