using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {

    private float m_ShieldCD = 3.0f; // Cooldown for shield
    public float m_ShieldDuration = 2.0f; // Duration of shield
    private float m_ShieldCDTimer = 3.0f; // Shield cooldown timer
    private float m_ShieldDurationTimer = 0.0f; // Shield duration timer

    private bool m_bIsShielded = false; // Checks if shielded

	// Use this for initialization
	void Start ()
    {
        m_ShieldCDTimer = m_ShieldCD; // Sets cooldown timer to cooldown to allow for immediate use of shield
    }
	
	// Update is called once per frame
	void Update ()
    {

        if(Input.GetAxisRaw("Ability") > 0 && !m_bIsShielded && m_ShieldCDTimer >= m_ShieldCD) // Checks if left trigger is pressed and shield is available
        {
            this.GetComponent<SpriteRenderer>().enabled = true; // Turns on shield sprite
            this.GetComponent<CircleCollider2D>().enabled = true; // Turns on shield collider
            m_bIsShielded = true; // is shielded to true
        }

        if(m_bIsShielded) // If shielded
        {
            m_ShieldDurationTimer += Time.deltaTime; // Start duration timer
        }
        else if(!m_bIsShielded) // if not shielded
        {
            m_ShieldCDTimer += Time.deltaTime; // Start cooldown timer
        }

        if(m_ShieldDurationTimer >= m_ShieldDuration) // If shield duration timer is greater then duration
        {
            this.GetComponent<SpriteRenderer>().enabled = false; // Turn shield sprite off
            this.GetComponent<CircleCollider2D>().enabled = false; // Turn shield collider off
            m_ShieldDurationTimer = 0; // Set duration timer to 0
            m_bIsShielded = false; // Set shield to false
            m_ShieldCDTimer = 0; // Set cooldown timer to 0
        }

	}
}
