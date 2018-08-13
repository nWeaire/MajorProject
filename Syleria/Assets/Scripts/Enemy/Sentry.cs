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

    // a timer for use in timing the shots
    private float m_fTimeBetweenShots = 0;

    // a timer for use in timing between bursts
    private float m_fTimeBetweenBursts;

    // a counter for the shots in a burst
    private int m_nBurstCount;


    // Use this for initialization
    void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_fTimeBetweenShots = m_fFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        // increase timer
        m_fTimeBetweenShots += Time.deltaTime;
        m_fTimeBetweenShots = m_fTimeBetweenShots % 60;

        // if timer has reached the limit,
        if (m_fTimeBetweenShots >= m_fFireRate)
        {
            // Shoot at player.
            if (m_nBurstCount < m_nBurstAmount)
            {
                Fire();
            }
            else
            {
                m_fTimeBetweenBursts += Time.deltaTime;
                m_fTimeBetweenBursts = m_fTimeBetweenBursts % 60;
                if(m_fTimeBetweenBursts >= m_fBurstTimer)
                {
                    m_fTimeBetweenBursts = 0.0f;
                    m_nBurstCount = 0;
                }
            }
        }

        // Boolean setting for the sprite
        if (transform.position.x - m_gPlayer.transform.position.x >= 0)
        {
            // Face left if the sprite is facing right by default
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else
        {
            // Face right if the sprite is facing right by default
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        if(m_nCurrentHealth <= 0)
        {
            Die();
        }
    }

    void Fire()
    {
        // The direction between the player and the sentry
        Vector3 v3AimDir = transform.position - m_gPlayer.transform.position;
        v3AimDir.Normalize();
        // Instantiate a bullet
        GameObject newBullet = Instantiate(m_gProjectile, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        m_nBurstCount++;

        // Add force so the bullet will go towards the player's pos * speed
        Vector3 v3Target = m_gPlayer.transform.position - transform.position;
        v3Target.Normalize();
        float angle = Mathf.Atan2(v3Target.y, v3Target.x) * Mathf.Rad2Deg;
        newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
        newBullet.GetComponent<EnemyBullet>().m_nDam = m_nDamage;
        newBullet.GetComponent<EnemyBullet>().m_fSpeed = m_fBulletSpeed;
        //newBullet.GetComponent<Rigidbody2D>().AddForce(-v3AimDir * m_fBulletSpeed);

        // Reset timer
        m_fTimeBetweenShots = 0.0f;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
