//--------------------------------------------------------------------------------------
// Purpose: Handles collision and movement of the bullets
//
// Description:  Handles all collision, movement and interaction with bullets
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float m_fSpeed; // Speed of bullet
    private GameObject m_gPlayer; // Reference to player object
    private Vector2 m_v2StartPos; // Start position of bullet
    private bool m_bDoneDamage = false;
	// Use this for initialization
	void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Sets reference to player object
        m_v2StartPos = this.transform.position; // Sets start position to player position
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move forward by speed * deltaTime
        transform.position += transform.up * m_fSpeed * Time.deltaTime;
        // Check Range for deletion
        CheckRange();
        if(m_bDoneDamage)
        {
            Destroy(this.gameObject);
        }
	}

    //--------------------------------------------------------------------------------------
    // If bullet collides with any trigger colliders
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) // If colliding with terrain
        {
            Destroy(this.gameObject); // Deletes bullet
        }
        if (collision.tag == "Enemy" && !m_bDoneDamage) // If collision with enemy
        {
            collision.GetComponent<Enemy>().TakeDamage(m_gPlayer.GetComponent<Player>().GetDamage()); // Deals damage to enemy based on players damage stat
            m_bDoneDamage = true;

        }
    }

    //--------------------------------------------------------------------------------------
    // Checks distance between bullet position and start position
    // Deletes bullet if over range
    //--------------------------------------------------------------------------------------
    void CheckRange()
    {
        if(Vector2.Distance(this.transform.position, m_v2StartPos) >= m_gPlayer.GetComponent<Player>().GetRange()) // If distance from bullet to start position is greater then player range stat
        {
            Destroy(this.gameObject); // Destroys bullet object
        }
    }
}
