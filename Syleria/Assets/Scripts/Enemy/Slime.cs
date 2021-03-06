﻿//--------------------------------------------------------------------------------------
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

    // Amount of Small Slimes to Spawn.
    [Tooltip("Amount of Small Slimes to spawn after this one dies. 0 = 1 and 3 = 4, cannot exceed 4")]
    public int m_nSmallSlimes;

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

    // Total knockback distance
    [SerializeField] private float m_fKnockDistance = 3;

    // This timer is used for all timing in the slime, being reset when its use is complete
    private float m_fTimer = 0f;

    // Timer for the moving of the slime.
    private float m_fMoveTimer = 0f;

    // Timer for the pause of the Slime movement.
    private float m_fPauseTimer = 0f;

    // Timer for knockback lerp.
    private float m_fKnockTimer = 0;

    // Speed for knockback lerp.
    private float m_fKnockSpeed = 5f;

    // Position knockback will end in.
    private Vector2 m_v2EndKnockPos;

    // Position knockback starts in.
    private Vector2 m_v2StartKnockPos; 

    // If the Player is being knocked back.
    private bool m_bKnockBack;

    // If the slime can knockback the player.
    private bool m_bCanKnockback;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    new void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        base.Awake();
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        // Set current health to maxHealth.
        m_nCurrentHealth = m_nHealth;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    new void Update()
    {
        // If the slime has been taunted,
        if(m_bTaunted)
        {
            // Find the Turtle.
            m_gCompanion = GameObject.FindGameObjectWithTag("Companion");
        }
        // Boolean setting for the sprite
        if (transform.position.x - m_gPlayer.transform.position.x >= 0 && !m_bTaunted || m_bTaunted && transform.position.x - m_gCompanion.transform.position.x >= 0)
        {
            // Face left 
            m_Animator.SetBool("MovingLeft", true);
        }
        else
        {
            // Face right
            m_Animator.SetBool("MovingLeft", false);
        }
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
            
        }
        // If Slime is spawning.
        else if(!m_bBigSlime && m_bSpawning)
        {
          // increment Timer.
          m_fTimer += 1 * Time.deltaTime;
            //GetComponent<CircleCollider2D>().enabled = false;
            // When Timer is higher than spawnTime
            if (m_fTimer > m_fSpawnTime)
          {
                // Slime can now move
                m_bCannotMove = false;
                //GetComponent<CircleCollider2D>().enabled = true;
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
            OnDeath();
            Die();
        }

        if(m_bKnockBack)
        {
            m_fKnockTimer += Time.deltaTime * m_fKnockSpeed;
            m_gPlayer.transform.parent.position = Vector2.Lerp(m_v2StartKnockPos, m_v2EndKnockPos, m_fKnockTimer);
            
            if(Vector2.Distance(m_gPlayer.transform.parent.position,m_v2EndKnockPos) <= 0.1f)
            {
                m_bKnockBack = false;
                m_fKnockTimer = 0.0f;
            }
        }
        if(m_bCannotMove && m_bCanKnockback)
        {
            m_bCanKnockback = false;
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
            for(int i = 0; i < m_nSmallSlimes; ++i)
            {
                if (i == 0)
                {
                    Instantiate(m_goSmallSlime, new Vector2(transform.position.x - m_goSmallSlime.GetComponent<CircleCollider2D>().radius, transform.position.y), transform.rotation);
                }
                else if (i == 1)
                {
                    Instantiate(m_goSmallSlime, new Vector2(transform.position.x + m_goSmallSlime.GetComponent<CircleCollider2D>().radius, transform.position.y), transform.rotation);
                }
                else if (i == 2)
                {
                    Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y - m_goSmallSlime.GetComponent<CircleCollider2D>().radius), transform.rotation);
                }
                else if (i == 3)
                {
                    Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y + m_goSmallSlime.GetComponent<CircleCollider2D>().radius), transform.rotation);
                }
            }
           
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
    // KnockPlayer: A function that knocks back the player.
    //
    // Parameters:
    //      Vector3 dir: The direction between Player and Enemy.
    //--------------------------------------------------------------------------------------
    void KnockPlayer(Vector3 dir)
    {
        // Knockback the player.
        m_bKnockBack = true;
        int count = 0; // Count of collisions detected
        RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of objects the ray collides with
        Vector2 rayOrigin = (Vector2)m_gPlayer.transform.parent.position; // Gets ray origin based on player position and collider offset
        count = Physics2D.Raycast(rayOrigin, (Vector2)(m_gPlayer.transform.parent.position - transform.position), m_cFilter, Hit, m_fKnockDistance + 1f); // Ray casts in direction of movement
        if (count > 0) // Checks if anything collided with the ray
        {
            m_v2EndKnockPos.x = Hit[0].point.x + dir.x;
            m_v2EndKnockPos.y = Hit[0].point.y + dir.y;
        }
        else // If nothing hit
        {
            m_v2EndKnockPos = (Vector2)m_gPlayer.transform.parent.position + (Vector2)(m_gPlayer.transform.parent.position - transform.position); // End position of dash set based on dash distance
        }

        m_v2StartKnockPos = m_gPlayer.transform.parent.position;
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
        if (!m_bCanKnockback)
        {
            Vector3 dir = transform.position - collision.transform.position;
            dir.Normalize();
            // If the Slime has collided with the Player,
            if (collision.tag == "Player" && !m_bCannotMove)
            {
                // Slime has been stunned.
                m_bCannotMove = true;
                // Damage the player.
                m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDamage);

                if (!m_bKnockBack)
                {
                    KnockPlayer(dir);
                }
            }
        }
    }
    //--------------------------------------------------------------------------------------
    // OnTriggerStay2D: A function called when the trigger on this object stays within another
    //                  trigger.
    // Parameters:
    //      Collider2D collision: The collider that this has collided with.
    //--------------------------------------------------------------------------------------
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!m_bKnockBack && !m_bCannotMove)
            {
                m_bCannotMove = true;
                m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDamage);
                Vector3 dir = transform.position - collision.transform.position;
                dir.Normalize();
                KnockPlayer(dir);
            }
        }

        if (collision.tag == "Enemy")
        {
            Vector3 dir = transform.position - collision.transform.position;
            dir.Normalize();
            transform.position += dir * 0.01f;
            transform.position = (Vector2)transform.position;
            
        }
    }
}
