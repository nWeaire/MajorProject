//--------------------------------------------------------------------------------------
// Purpose: Handles Camera movement
//
// Description:  Handles all Camera movement and interactions
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    private GameObject m_oPlayer; // Reference to player object
    private Vector2 m_v2Target; // Vector2 for target position of camera
    [SerializeField] private float m_nCamDistance; // Max distance camera can move from player
    [SerializeField] private float m_fSpeed; // Max speed camera can move towards target 0-1

	// Use this for initialization
	void Awake ()
    {
        m_oPlayer = GameObject.FindGameObjectWithTag("Player"); // Sets reference to player object
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_v2Target = new Vector2(Input.GetAxis("RightStickX"),Input.GetAxis("RightStickY")); // Gets right stick input
        Vector2 tempTarget = m_v2Target * m_nCamDistance; // Sets a temp target based on right stick input and distance
        
        transform.Translate(((tempTarget - (Vector2)this.transform.position) + (Vector2)m_oPlayer.transform.position) * m_fSpeed * Time.deltaTime); 
    }
}
