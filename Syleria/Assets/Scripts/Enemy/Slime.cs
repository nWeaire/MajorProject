//--------------------------------------------------------------------------------------
// Purpose: Used on the Slime enemies.
//
// Description: A child of Enemy, this script will control
//              all functions in the Slime enemy.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    // If true, this slime will spawn smaller slimes on death
    [Tooltip("Untick this on the Small Slime Prefab, or on any slime that shouldn't spawn other slimes")]
    public bool m_bBigSlime;

    // Prefab for the Small Slimes that a Big Slime will spawn
    [Tooltip("Prefab of Small Slime goes here")]
    public GameObject m_goSmallSlime;

    // If this is true, the Slime will not move
    [Tooltip("If this is true, Slime won't move")]
    public bool m_bCannotMove;

    // When the timer reaches this number, it will allow a spawning slime to move
    [Tooltip("When the timer reaches this number, it will allow a spawning slime to move")]
    public float m_fSpawnTime;

    // When the timer reaches this number, it will allow a stunned slime to move
    [Tooltip("When the timer reaches this number, it will allow a stunned slime to move")]
    public float m_fStunTime;

    // bool that will be set true on small slimes, it will then be false after the slime can move
    [Tooltip("bool that will be set true on small slimes, it will then be false after the slime can move")]
    public bool m_bSpawning;

    // This timer is used for all timing in the slime, being reset when its use is complete
    private float m_fTimer = 0f;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Start()
    {
        // Set player to Player
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        // Set current health to maxHealth.
        m_nCurrentHealth = m_nHealth;
        // Set current speed to maxSpeed.
        m_fSpeed = m_fMaxSpeed;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // If Slime can move
        if (!m_bCannotMove)
        {
            // Move forward by speed * deltaTime.
            transform.position += transform.up * m_fSpeed * Time.deltaTime;
            // Rotate towards the players position.
            Seek(m_gPlayer.transform.position);

            // Check for Obstacles, this will steer the Slime away from them when required.
            AvoidObstacles();
            
        }
        // If Slime is spawning.
        else if(!m_bBigSlime && m_bSpawning)
        {
          // increment Timer.
          m_fTimer += 1 * Time.deltaTime;

          // When Timer is higher than spawnTime
          if (m_fTimer > m_fSpawnTime)
          {
                // Slime can now move
                m_bCannotMove = false;
                // Slime is no longer spawning.
                m_bSpawning = false;
                // Reset the timer.
                m_fTimer = 0f;
          }
        }
        // if Big Slime is not moving
        else
        {
            // If Big slime is not Spawning, it is stunned.
            if(!m_bSpawning)
            {
                // Increment timer.
                m_fTimer += 1 * Time.deltaTime;

                // When timer reaches stun time.
                if (m_fTimer > m_fStunTime)
                {
                    // Unstun the Slime.
                    m_bCannotMove = false;
                    // Reset the Timer.
                    m_fTimer = 0;
                }
            }
        }
      
        // If slime have no health, die.
        if (m_nCurrentHealth <= 0)
        {
            Die();
        }
    }

    //--------------------------------------------------------------------------------------
    // Die: This is called when health is 0 or less, runs anything the enemy should do 
    //      on death, then deletes it.
    //--------------------------------------------------------------------------------------
    void Die()
    {
        // If this slime is a big slime.
        if (m_bBigSlime)
        {
            // Spawn Slimes (temp) replace with array spawning x amount of smallSlime .
            m_goSmallSlime.GetComponent<Slime>().m_bSpawning = true;
            Instantiate(m_goSmallSlime,new Vector2(transform.position.x - 0.5f,transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x + 0.5f, transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            // Destroy this object.
            Destroy(gameObject);
        }
        // If this slime is a small slime.
        else
        {
            // Destroy this object.
            Destroy(gameObject);
        }
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
        // If the Slime has collided with the Player.
        if (collision.tag == "Player")
        {
            // Slime has been stunned.
            m_bCannotMove = true;
            // Damage the player.
            m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDamage);

            // Knockback the player.
            m_gPlayer.transform.parent.position = m_gPlayer.transform.parent.position + (m_gPlayer.transform.parent.position - transform.position);
        }
    }
}
