//--------------------------------------------------------------------------------------
// Purpose: Handles Health bar for UI
//
// Description:  Checks players max and current health stat and updates health bar
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [SerializeField]private Slider m_sHealthSlider; // Reference to health slider
    private GameObject m_gPlayer; // Reference to player object
    
	// Use this for initialization
	void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Sets reference to player
        m_sHealthSlider.minValue = 0; // Sets minimum value of slider
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_sHealthSlider.maxValue = m_gPlayer.GetComponent<Player>().GetMaxHealth(); // Sets max value of health slider based on player max health stat
        m_sHealthSlider.value = m_gPlayer.GetComponent<Player>().GetCurrentHealth(); // Sets value of helath slider based on player current health stat
    }
}
