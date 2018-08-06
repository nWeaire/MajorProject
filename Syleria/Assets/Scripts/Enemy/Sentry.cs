using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    // Projectile for the Sentry
    public GameObject m_gProjectile;

    // Speed that the bullet will go at
    public float m_fBulletSpeed;

    // Timer between shots
    public  float m_fFireRate;

    // a timer for use in timing the shots
    private float m_fTimeBetweenShots = 0;


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
            Debug.Log("Left");
            // Face left if the sprite is facing right by default
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else
        {
            Debug.Log("Right");
            // Face right if the sprite is facing left by default
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    void Fire()
    {
        // The direction between the player and the sentry
        Vector3 v3AimDir = transform.position - m_gPlayer.transform.position;
        v3AimDir.Normalize();
        // Instantiate a bullet
        GameObject newBullet = Instantiate(m_gProjectile, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        // Add force so the bullet will go towards the player's pos * speed
        newBullet.GetComponent<Rigidbody2D>().AddForce(-v3AimDir * m_fBulletSpeed);
        // Reset timer
        m_fTimeBetweenShots = 0;
    }
}
