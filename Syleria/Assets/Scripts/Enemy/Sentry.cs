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
    [Tooltip("Time between shots")]
    public float m_fFireRate;

    // Time between bursts
    [Tooltip("Time between bursts")]
    public float m_fBurstTimer;

    // Amount of shots per burst
    [Tooltip("Amount of shots per burst")]
    public int m_nBurstAmount;

    // a timer for use in timing the shots
    private float m_fTimeBetweenShots = 0;

    // a counter for the shots in a burst
    private int m_nBurstCount;


    // Use this for initialization
    void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_fFireRate = 60.0f / (m_fFireRate * 60.0f);
        m_fTimeBetweenShots = m_fFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        // increase timer
        m_fTimeBetweenShots += Time.deltaTime;

        // if timer has reached the limit,
        if (m_fTimeBetweenShots >= m_fFireRate)
        {
            // Shoot at player.
            Fire();
        }

        // Boolean setting for the sprite
        if (transform.position.x - m_gPlayer.transform.position.x >= 1)
        {
            // Face left if the sprite is facing right by default
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else
        {
            // Face right if the sprite is facing left by default
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
        //newBullet.transform.LookAt(transform.position - v3AimDir, Vector3.up);

        //newBullet.transform.eulerAngles = new Vector3(newBullet.transform.eulerAngles.x, 0, 0);
        // Add force so the bullet will go towards the player's pos * speed

        Vector3 v3Target = m_gPlayer.transform.position - transform.position;
        float angle = Mathf.Atan2(v3Target.y, v3Target.x) * Mathf.Rad2Deg;

        //Kinda works, however rotating instantly is not the wanted behaviour
        newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        newBullet.GetComponent<EnemyBullet>().m_nDam = m_nDamage;
        newBullet.GetComponent<Rigidbody2D>().AddForce(-v3AimDir * m_fBulletSpeed);

        // Reset timer
        m_fTimeBetweenShots = 0;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
