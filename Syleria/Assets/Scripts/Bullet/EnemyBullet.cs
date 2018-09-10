//--------------------------------------------------------------------------------------
// Purpose: Used on the Slime enemies.
//
// Description: A child of MonoBehaviour, this script will control all 
//              functions in the Enemy Bullets.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Range until the bullet despawns.
    [Tooltip("Range until bullet despawns")]
    public float m_fRange;

    public float m_fSinMag = 20f;

    public float m_fSinFreq = 0.1f;

    public bool m_bSerpentine = false;

    public float m_fMaxOffSet;

    public float m_fMinOffSet;

    // The damage that this bullet will do.
    // NOTE: this will be set in the enemy that spawns it.
    [HideInInspector]
    public int m_nDam;

    // The speed that this bullet will go.
    // NOTE: this will be set in the enemy that spawns it.
    [HideInInspector]
    public float m_fSpeed;

    // A reference to the player for damaging it.
    private GameObject m_gPlayer;

    // The position this bullet was instantiated at.
    private Vector2 m_v2StartPos;

    private float m_fOffset;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Start()
    {
        // Set Reference to the GameObject tagged "Player".
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        // Set the startPos as the position it starts at.
        m_v2StartPos = this.transform.position;
        m_fOffset = Random.Range(m_fMinOffSet, m_fMaxOffSet);
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    private void Update()
    {
        // Move forward by speed * deltaTime
        transform.position += transform.up * m_fSpeed * Time.deltaTime;
        // Delete bullet if it reaches its max range
        if (m_bSerpentine && Vector2.Distance(this.transform.position, m_v2StartPos) >= 0.5f)
        {
            transform.position += transform.right * Mathf.Sin(Time.time * m_fOffset)* m_fSinMag * (m_fSinFreq * 0.01f);
        }
        CheckRange();
    }

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter2D: A function called when the trigger on this object collides with 
    //                   another object.
    //
    // Parameters:
    //      Collider2D collision: The collider that this has collided with.
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the collision is on the 8th layer(Terrain).
        if (collision.gameObject.layer == 8)
        {
            // Destroy this object.
            Destroy(this.gameObject);
        }
        // If the collision is with an object tagged Shield.
        if(collision.tag == "Shield")
        {
            // Destroy this object.
            Destroy(this.gameObject);
        }
        // If the collision is with the player.
        if(collision.tag == "Player")
        {
            // Damage the player.
            m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDam);
            // Destroy this object.
            Destroy(this.gameObject);
        }
    }

    //--------------------------------------------------------------------------------------
    // CheckRange: Sees how far the bullet has gone, and deletes it
    //             when it reaches or goes beyond its range.
    //                   
    //--------------------------------------------------------------------------------------
    void CheckRange()
    {
        // If the distance between bullets start position and it's current position
        // is greater than or equal to the range it may go
        if (Vector2.Distance(this.transform.position, m_v2StartPos) >= m_fRange)
        {
            // Destroy this object.
            Destroy(this.gameObject);
        }
    }
}