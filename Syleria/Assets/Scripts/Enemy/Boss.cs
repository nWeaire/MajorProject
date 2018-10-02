//--------------------------------------------------------------------------------------
// Purpose: Used on the Boss enemy.
//
// Description: A child of Enemy, this script will control
//              all functions in the Boss enemy.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    // Projectile for the Shotgun Blast Bullets.
    [Tooltip("Put Enemy Bullet prefab here")]
    public GameObject m_gShotgunProjectile;

    // Speed that the shotgun bullets will go at.
    [Tooltip("Speed that the bullet will be fired at")]
    public float m_fShotgunBulletSpeed;

    // Timer between shots
    [Tooltip("Seconds between shots")]
    public float m_fFireRate;

    // Time between bursts
    [Tooltip("Seconds between bursts")]
    public float m_fBurstTimer;

    // Amount of shots per burst
    [Tooltip("Amount of shots per burst")]
    public int m_nBurstAmount = 1;

    // Amount of bullets that will be instantiated in the spread
    [Tooltip("Amount of bullets that will be instantiated in the spread")]
    public int m_nBulletAmount = 5;

    // Amount of spread between the bullets in a burst
    [Tooltip("Amount of spread between the bullets in a burst")]
    public float m_fBulletSpread = 10;

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
    void Start()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    new void Update()
    {
		
	}
}
