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

    // Timer for the Movement
    private float m_fMoveTimer = 0;

    // Use this for initialization
    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_nCurrentHealth = m_nHealth;
        m_fSpeed = m_fMaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // If Slime can move
        if (!m_bCannotMove)
        {
            // Move forward by speed * deltaTime
            transform.position += transform.up * m_fSpeed * Time.deltaTime;

            // Rotate towards the players position
            Seek(m_gPlayer.transform.position);

            // Check for Obstacles, this will steer the Slime away from them when required
            AvoidObstacles();
        }
        // If Slime cannot move
        else
        {
            // increment Timer
            m_fMoveTimer += 1 * Time.deltaTime;
        }
        // When Timer is higher than 1
        if(m_fMoveTimer > 1)
        {
            m_bCannotMove = false;
        }
        // If you have no health, die.
        if (m_nCurrentHealth <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        if (m_bBigSlime)
        {
            // Spawn Slimes (temp) replace with array spawning x amount of smallSlime 
            Instantiate(m_goSmallSlime,new Vector2(transform.position.x - 0.5f,transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x + 0.5f, transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            // Normal death stuff here
        }
    }
}
