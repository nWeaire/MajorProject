//--------------------------------------------------------------------------------------
// Purpose: Handles Logic for ability sprite
//
// Description:  Handles all logic based around ability sprites
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : MonoBehaviour {

    public float m_fTauntSpriteLength = 0.1f; // Time which sprite is visable
    private float m_fTimer = 0; // Timer

	// Use this for initialization
	void Awake ()
    {
        m_fTimer = 0; // Timer set to 0
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_fTimer += Time.deltaTime; // Updates timer 
        if(m_fTimer >= m_fTauntSpriteLength) // If timer > sprite length
        {
            this.gameObject.SetActive(false); // Set this to false
        }
	}
}
