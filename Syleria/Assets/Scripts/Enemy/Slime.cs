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

    // When the timer reaches this number, it will allow stop moving
    [Tooltip("How long each move of the slime will last for, in seconds")]
    public float m_fMoveTime;

    // When the timer reaches this number, it will allow a slime to move 
    [Tooltip("How long the slime will wait before moving again, in seconds")]
    public float m_fPauseTime;

    // bool that will be set true on small slimes, it will then be false after the slime can move
    [Tooltip("bool that will be set true on small slimes, it will then be false after the slime can move")]
    public bool m_bSpawning;

    // This timer is used for all timing in the slime, being reset when its use is complete
    private float m_fTimer = 0f;

    private float m_fMoveTimer = 0f;

    private float m_fPauseTimer = 0f;

    private float m_fKnockTimer = 0; // Timer for knockback lerp 

    private float m_fKnockSpeed = 5f; // Speed for knockback lerp

    private Vector2 m_v2EndKnockPos; // Position knockback will end in 

    private Vector2 m_v2StartKnockPos; // Position knockback starts in

    [SerializeField] private float m_fKnockDistance = 3; // Total knockback distance

    private bool m_bKnockBack;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    new void Start()
    {
        base.Start();
        // Set current health to maxHealth.
        m_nCurrentHealth = m_nHealth;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    new void Update()
    {
        // If Slime can move
        if (!m_bCannotMove)
        {
            //increment timer.
            m_fMoveTimer += Time.deltaTime;
            m_fMoveTimer = m_fMoveTimer % 60;
            // Move forward by speed * deltaTime.
            if (m_fMoveTimer <= m_fMoveTime)
            {
                base.Update();
            }
            if(m_fMoveTimer >= m_fMoveTime)
            {
                m_fPauseTimer += Time.deltaTime;
                m_fPauseTimer = m_fPauseTimer % 60;

                if(m_fPauseTimer >= m_fPauseTime)
                {
                    m_fMoveTimer = 0.0f;
                    m_fPauseTimer = 0.0f;
                }

            }

            // Rotate towards the players position.
            //Seek(m_gPlayer.transform.position);

            // Check for Obstacles, this will steer the Slime away from them when required.
            //AvoidObstacles();
            
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
                m_fTimer = m_fTimer % 60;

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

        if(m_bKnockBack)
        {
            m_fKnockTimer += Time.deltaTime * m_fKnockSpeed;
            m_gPlayer.transform.parent.position = Vector2.Lerp(m_v2StartKnockPos, m_v2EndKnockPos, m_fKnockTimer);

            if(Vector2.Distance(m_gPlayer.transform.parent.position,m_v2EndKnockPos) <= 0.5f)
            {
                m_bKnockBack = false;
                m_fKnockTimer = 0.0f;
            }
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
            m_goSmallSlime.GetComponent<Slime>().m_gPlayer = m_gPlayer;
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x - 0.5f,transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x + 0.5f, transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            // Destroy this object.
            Debug.Log("DEAD");
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
        Vector3 dir = transform.position - collision.transform.position;
        dir.Normalize();
        // If the Slime has collided with the Player.
        if (collision.tag == "Player")
        {
            // Slime has been stunned.
            m_bCannotMove = true;
            // Damage the player.
            m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDamage);

            // Knockback the player.
            m_bKnockBack = true;
            int count = 0; // Count of collisions detected
            RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of objects the ray collides with
            Vector2 rayOrigin = (Vector2)m_gPlayer.transform.parent.position - new Vector2(m_gPlayer.GetComponentInParent<CircleCollider2D>().offset.x, m_gPlayer.GetComponentInParent<CircleCollider2D>().offset.y); // Gets ray origin based on player position and collider offset
            count = Physics2D.Raycast(rayOrigin, (Vector2)(m_gPlayer.transform.parent.position - transform.position), m_cFilter, Hit, m_fKnockDistance); // Ray casts in direction of movement
            Debug.DrawRay(rayOrigin, m_gPlayer.transform.parent.position - transform.position, Color.magenta, 10f);
            if (count > 0) // Checks if anything collided with the ray
            {
                m_v2EndKnockPos.x = Hit[0].point.x + dir.x;
                m_v2EndKnockPos.y = Hit[0].point.y + dir.y;
            }
            else // If nothing hit
            {
                m_v2EndKnockPos = (Vector2)m_gPlayer.transform.position + (Vector2)(m_gPlayer.transform.parent.position - transform.position); // End position of dash set based on dash distance
            }
            
            m_v2StartKnockPos = m_gPlayer.transform.parent.position;
        }

        if (collision.tag == "Enemy")
        {
            transform.position += dir * 0.1f;
        }
    }
}
