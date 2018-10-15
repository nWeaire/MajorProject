//--------------------------------------------------------------------------------------
// Purpose: Used on the Boss enemy.
//
// Description: A child of Enemy, this script will control
//              all functions in the Boss enemy.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    #region Shotgun Variables
    [Header("Shotgun Variables")]
    // Projectile for the Shotgun Blast Bullets.
    [Tooltip("Put Enemy Bullet prefab here")]
    public GameObject m_gShotgunProjectile;

    // Speed that the shotgun bullets will go at.
    [Tooltip("Speed that the bullet will be fired at")]
    public float m_fShotgunBulletSpeed;

    // Timer between shots
    [Tooltip("Seconds between Shotgun shots")]
    public float m_fShotgunFireRate;

    // Time between bursts
    [Tooltip("Seconds between bursts")]
    public float m_fBurstTimer;

    // Amount of shots per burst
    [Tooltip("Amount of shots per burst")]
    public int m_nBurstAmount = 2;

    // Amount of bullets that will be instantiated in the spread
    [Tooltip("Amount of bullets that will be instantiated in the spread")]
    public int m_nFirstBulletCount = 5;

    // Amount of bullets that will be instantiated in the spread
    [Tooltip("Amount of bullets that will be instantiated in the spread")]
    public int m_nSecondBulletCount = 4;

    // Amount of spread between the bullets in a burst
    [Tooltip("Amount of spread between the bullets in a burst")]
    public float m_fFirstBulletSpread = 10;

    // Amount of spread between the bullets in a burst
    [Tooltip("Amount of spread between the bullets in a burst")]
    public float m_fSecondBulletSpread = 10;

    // a timer for use in timing the shots
    private float m_fTimeBetweenShotguns = 0;

    // a timer for use in timing between bursts
    private float m_fTimeBetweenBursts;

    // a counter for the shots in a burst
    private int m_nBurstCount;
    #endregion

    #region Spiral Burst Variables
    [Header("Spiral Burst Variables")]
    // Projectile for the Spiral Blast Bullets.
    [Tooltip("Put Enemy Bullet prefab here")]
    public GameObject m_gSpiralProjectile;

    // Speed that the Spiral bullets will go at.
    [Tooltip("Speed that the bullet will be fired at")]
    public float m_fSpiralBulletSpeed;

    // Timer between shots
    [Tooltip("Seconds between Spiral shots")]
    public float m_fSpiralFireRate;

    // Amount of bullets that will be instantiated in the spread
    [Tooltip("Amount of bullets that will be instantiated in the spread")]
    public int m_nSpiralBulletCount = 5;

    // a timer for use in timing the shots
    private float m_fTimeBetweenSpirals = 0;
    #endregion

    #region Laser Variables
    [Header("Laser Variables")]
    // Projectile for the Laser.
    [Tooltip("Put Laser prefab here")]
    public GameObject m_gLaserProjectile;

    // How long the laser telegraph will last before it will deal damage
    public float m_fLaserChargeTime;

    // How long the laser will last, dealing damage and such.
    public float m_fLaserBlastTime;

    // a timer for use in timing the shots
    [Tooltip("Seconds between Laser shots")]
    public float m_fLaserFireRate;

    // Timer between shots
    private float m_fTimeBetweenLasers = 0;

    // a timer for use in the telegraph
    private float m_fLaserChargeTimer = 0;

    // a timer for use in the Laser Blast
    private float m_fLaserBlastTimer = 0;

    #endregion

    [Header("Misc. Variables")]
    [Tooltip("Time in Seconds that the enemy will sit still after spawn")]
    public float m_fSpawnTime;

    private Vector3 m_v3Target;

    private float m_fSpawnTimer = 0.0f;

    private bool m_bSpawnStun;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Start()
    {
        base.Awake();
        m_bSpawnStun = true;
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    new void Update()
    {
        if(!m_bSpawnStun)
        {
            if(m_nCurrentHealth <= 0)
            {
                Die();
            }
            //TODO: Setup logic for choosing what shots to fire and so on
            if(m_fTimeBetweenShotguns <= m_fShotgunFireRate)
            {
            #region Shotgun Calculations and Timers
            // if burst amount is less than the amount of shots wanted,
            if (m_nBurstCount < m_nBurstAmount)
            {
                // Fire a shot.
                if (!m_bTaunted)
                {
                    if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gPlayer.transform.position, m_WallLayer))
                    {
                        FireShotgun();
                    }
                }
                else
                {
                    if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gCompanion.transform.position, m_WallLayer))
                    {
                        FireShotgun();
                    }
                }
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
                    // Reset Shotgun counter.
                    m_fTimeBetweenShotguns = 0.0f;
                }
            }
            #endregion
            }

            if(m_fTimeBetweenSpirals >= m_fSpiralFireRate)
            {
                #region Spiral Calculations and Timers
                Debug.Log("Spiral Shot");
            #endregion
            }

            if(m_fTimeBetweenLasers >= m_fLaserFireRate)
            {
                #region Laser Calculations and Timers
                Debug.Log("Laser Charge up");
            #endregion
            }

            #region Timers
            // Increase Shotgun timer.
            m_fTimeBetweenShotguns += Time.deltaTime;
            m_fTimeBetweenShotguns = m_fTimeBetweenShotguns % 60;
            // Increase Spiral timer.
            m_fTimeBetweenSpirals += Time.deltaTime;
            m_fTimeBetweenSpirals = m_fTimeBetweenSpirals % 60;
            // Increase Laser timer.
            m_fTimeBetweenLasers += Time.deltaTime;
            m_fTimeBetweenLasers = m_fLaserFireRate % 60;
            #endregion
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
    // FireShotgun: Fires multiple bursts, alternating between the two given sizes, towards the player.
    //--------------------------------------------------------------------------------------
    void FireShotgun()
    {
        if (m_nBurstCount % 2 == 0)
        {
            // Instantiate a bullet
            for (int i = 0; i < m_nFirstBulletCount; ++i)
            {
                GameObject newBullet = Instantiate(m_gShotgunProjectile, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                // Get the target position
                if (m_bTaunted)
                {
                    m_v3Target = m_gCompanion.transform.position - transform.position;
                    m_v3Target.Normalize();
                }
                else
                {
                    m_v3Target = m_gPlayer.transform.position - transform.position;
                    m_v3Target.Normalize();
                }
                // Calculate rotation needed to face Player
                float angle = Mathf.Atan2(m_v3Target.y, m_v3Target.x) * Mathf.Rad2Deg;
                // Set bullets rotation to face Player.
                newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90 - (((float)m_nFirstBulletCount - 1) * 0.5f) * m_fFirstBulletSpread + (i * m_fFirstBulletSpread)));
                // Set bullets damage to this damage value
                newBullet.GetComponent<EnemyBullet>().m_nDam = m_nDamage;
                // Set bullets damage to this  bullet speed
                newBullet.GetComponent<EnemyBullet>().m_fSpeed = m_fShotgunBulletSpeed;
            }
        }
        else
        {
            // Instantiate a bullet
            for (int i = 0; i < m_nSecondBulletCount; ++i)
            {
                GameObject newBullet = Instantiate(m_gShotgunProjectile, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                // Get the target position
                if (m_bTaunted)
                {
                    m_v3Target = m_gCompanion.transform.position - transform.position;
                    m_v3Target.Normalize();
                }
                else
                {
                    m_v3Target = m_gPlayer.transform.position - transform.position;
                    m_v3Target.Normalize();
                }
                // Calculate rotation needed to face Player
                float angle = Mathf.Atan2(m_v3Target.y, m_v3Target.x) * Mathf.Rad2Deg;
                // Set bullets rotation to face Player.
                newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90 - (((float)m_nSecondBulletCount - 1) * 0.5f) * m_fSecondBulletSpread + (i * m_fSecondBulletSpread)));
                // Set bullets damage to this damage value
                newBullet.GetComponent<EnemyBullet>().m_nDam = m_nDamage;
                // Set bullets damage to this  bullet speed
                newBullet.GetComponent<EnemyBullet>().m_fSpeed = m_fShotgunBulletSpeed;
            }
        }
        m_nBurstCount++;
        // Reset timer
        m_fTimeBetweenBursts = 0.0f;
    }

    //--------------------------------------------------------------------------------------
    // FireSpiral: Fires out a circle of bullets around itself.
    //--------------------------------------------------------------------------------------
    void FireSpiral()
    {

    }

    //--------------------------------------------------------------------------------------
    // FireLaser: Begins the telegraphing of the Laser until it eventually fires.
    //--------------------------------------------------------------------------------------
    void FireLaser()
    {

    }

    //--------------------------------------------------------------------------------------
    // Die: This is called when health is 0 or less, runs anything the enemy should do 
    //      on death, then deletes it.
    //--------------------------------------------------------------------------------------
    void Die()
    {
        // Destroy this object.
        Destroy(gameObject);

        // Player wins game! da da dunnn
    }
}
*/