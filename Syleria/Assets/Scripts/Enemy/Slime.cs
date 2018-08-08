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
        // If Slime is spawning
        else if(!m_bBigSlime && m_bSpawning)
        {
          // increment Timer
          m_fTimer += 1 * Time.deltaTime;

          // When Timer is higher than 1
          if (m_fTimer > m_fSpawnTime)
          {
            m_bCannotMove = false;
                m_bSpawning = false;
                m_fTimer = 0f;
          }
        }
        else
        {
            if(!m_bSpawning)
            {
                m_bCannotMove = true;
                m_fTimer += 1 * Time.deltaTime;

                if (m_fTimer > m_fStunTime)
                {
                    m_bCannotMove = false;
                    m_fTimer = 0;
                }
            }
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
            m_goSmallSlime.GetComponent<Slime>().m_bSpawning = true;
            Instantiate(m_goSmallSlime,new Vector2(transform.position.x - 0.5f,transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x + 0.5f, transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            // Normal death stuff here
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_bCannotMove = true;
            m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDamage);
            // Knockback here
        }
    }
}
