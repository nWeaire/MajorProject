//--------------------------------------------------------------------------------------
// Purpose: Used on the Sentry enemies.
//
// Description: A child of Enemy, this script will control
//              all functions in the Sentry enemy.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    // Projectile for the Sentry
    [Tooltip("Put Enemy Bullet prefab here")]
    public GameObject m_gProjectile;

    // Speed that the bullet will go at
    [Tooltip("Speed that the bullet will be fired at")]
    public float m_fBulletSpeed;

    // Timer between shots
    [Tooltip("Seconds between shots")]
    public float m_fFireRate;

    // Time between bursts
    [Tooltip("Seconds between bursts")]
    public float m_fBurstTimer;

    // Amount of shots per burst
    [Tooltip("Amount of shots per burst")]
    public int m_nBurstAmount;

    public GameObject m_gEye;

    [Tooltip("Time in Seconds that the enemy will sit still after spawn")]
    public float m_fSpawnTime;

    // a timer for use in timing the shots
    private float m_fTimeBetweenShots = 0;

    // a timer for use in timing between bursts
    private float m_fTimeBetweenBursts;

    // a counter for the shots in a burst
    private int m_nBurstCount;

    private Vector3 m_v3Target;

    private float m_fSpawnTimer = 0.0f;

    private bool m_bSpawnStun;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    new void Awake()
    {
        base.Awake();
        // Get Player.
        m_bSpawnStun = true;
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        // Set the counter to max timer.
        m_fTimeBetweenShots = m_fFireRate;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    new void Update()
    {
        if (!m_bSpawnStun)
        {
            base.Update();
            // Boolean setting for the sprite
            if (transform.position.x - m_gPlayer.transform.position.x >= 0)
            {
                // Face left 
                m_gEye.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                // Face right
                m_gEye.GetComponent<SpriteRenderer>().flipX = true;
            }
            // increase timer
            m_fTimeBetweenShots += Time.deltaTime;
            m_fTimeBetweenShots = m_fTimeBetweenShots % 60;

            // if timer has reached the limit,
            if (m_fTimeBetweenShots >= m_fFireRate)
            {
                // if burst amount is less than the amount of shots wanted,
                if (m_nBurstCount < m_nBurstAmount)
                {
                    // Fire a shot.
                    Fire();
                }
                // if BurstCount has added up to the amount of shots wanted,
                else
                {
                    // Increment Counter.
                    m_fTimeBetweenBursts += Time.deltaTime;
                    // Make counter in seconds.
                    m_fTimeBetweenBursts = m_fTimeBetweenBursts % 60;
                    // If counter has reacher the timer.
                    if (m_fTimeBetweenBursts >= m_fBurstTimer)
                    {
                        // Reset timer.
                        m_fTimeBetweenBursts = 0.0f;
                        // Reset Counter.
                        m_nBurstCount = 0;
                    }
                }
            }
            // If health is less than or equal to zero
            if (m_nCurrentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            // Increment timer.
            m_fSpawnTimer += 1 * Time.deltaTime;
            m_fSpawnTimer = m_fSpawnTimer % 60;

            // When timer reaches stun time.
            if (m_fSpawnTimer > m_fSpawnTime)
            {

                // Unstun the Slime.
                m_bSpawnStun = false;
                // Reset the Timer.
                m_fSpawnTimer = 0;
            }
        }
    }
    //--------------------------------------------------------------------------------------
    // Fire: Spawn a projectile and fire it towards the player.
    //--------------------------------------------------------------------------------------
    void Fire()
    {
        // Instantiate a bullet
        GameObject newBullet = Instantiate(m_gProjectile, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        m_nBurstCount++;

        if (m_bTaunted)
        {
            m_gCompanion = GameObject.FindGameObjectWithTag("Turtle");
            m_v3Target = m_gCompanion.transform.position - transform.position;
            m_v3Target.Normalize();
        }
        else
        {
            // Get the target position
            m_v3Target = m_gPlayer.transform.position - transform.position;
            m_v3Target.Normalize();
        }
        // Calculate rotation needed to face Player
        float angle = Mathf.Atan2(m_v3Target.y, m_v3Target.x) * Mathf.Rad2Deg;
        // Set bullets rotation to face Player.
        newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
        // Set bullets damage to this Sentrys damage value
        newBullet.GetComponent<EnemyBullet>().m_nDam = m_nDamage;
        // Set bullets damage to this Sentrys bullet speed
        newBullet.GetComponent<EnemyBullet>().m_fSpeed = m_fBulletSpeed;

        // Reset timer
        m_fTimeBetweenShots = 0.0f;
    }

    //--------------------------------------------------------------------------------------
    // Die: This is called when health is 0 or less, runs anything the enemy should do 
    //      on death, then deletes it.
    //--------------------------------------------------------------------------------------
    void Die()
    {
        // Destroy this object.
        Destroy(gameObject);
    }
}
