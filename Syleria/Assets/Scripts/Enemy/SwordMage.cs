using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMage : Enemy
{
    // Projectile for the Sentry
    [Tooltip("Put Enemy Bullet prefab here")]
    public GameObject m_gProjectile;

    // Speed that the bullet will go at
    [Tooltip("Speed that the bullet will be fired at")]
    public float m_fBulletSpeed;

    public float m_fMoveDelay;

    // Timer between shots
    [Tooltip("Seconds between shots")]
    public float m_fFireRate;

    // Time between bursts
    [Tooltip("Seconds between bursts")]
    public float m_fBurstTimer;

    // Amount of shots per burst
    [Tooltip("Amount of shots per burst")]
    public int m_nBurstAmount;

    [Tooltip("Time in Seconds that the enemy will sit still after spawn")]
    public float m_fSpawnTime;

    // a timer for use in timing the shots
    private float m_fTimeBetweenShots = 0;

    // a timer for use in timing between bursts
    private float m_fTimeBetweenBursts;

    private float m_fMoveTimer = 0;

    // a counter for the shots in a burst
    private int m_nBurstCount;

    private Vector3 m_v3Target;

    private bool m_bFinishedFiring;

    private bool m_bCanMove;

    private float m_fSpawnTimer = 0.0f;

    private bool m_bSpawnStun;

    //Timer for knockback lerp
    private float m_fKnockTimer = 0;

    // Speed for knockback lerp
    private float m_fKnockSpeed = 5f;

    // Position knockback will end in 
    private Vector2 m_v2EndKnockPos;

    // Position knockback starts in
    private Vector2 m_v2StartKnockPos;

    // Total knockback distance
    [SerializeField] private float m_fKnockDistance = 3;

    private bool m_bKnockBack;

    private Animator m_Animator;

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
        m_Animator = GetComponentInChildren<Animator>();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    new void Update()
    {
        // Boolean setting for the animation.
        if (transform.position.x - m_gPlayer.transform.position.x >= 0 || m_bTaunted && transform.position.x - m_gCompanion.transform.position.x >= 0)
        {
            // Face left 
            m_Animator.SetBool("isLeft", false);
        }
        else
        {
            // Face right
            m_Animator.SetBool("isLeft", true);
        }

        if (!m_bSpawnStun)
        {
            // Increase timer.
            m_fTimeBetweenShots += Time.deltaTime;
            m_fTimeBetweenShots = m_fTimeBetweenShots % 60;
            if (m_bFinishedFiring)
            {
                m_fMoveTimer += Time.deltaTime;
                m_fMoveTimer = m_fMoveTimer % 60;
                if (m_fMoveTimer >= m_fMoveDelay)
                {
                    m_bCanMove = true;
                    m_fMoveTimer = 0f;
                }
                if (m_bCanMove)
                {
                    base.Update();
                }
            }

            // If timer has reached the limit,
            if (m_fTimeBetweenShots >= m_fFireRate)
            {
                // If burst amount is less than the amount of shots wanted,
                if (m_nBurstCount < m_nBurstAmount)
                {
                    // Fire a shot.
                    if (!m_bTaunted)
                    {
                        if (!Physics2D.Linecast((Vector2)this.transform.position + new Vector2(0, GetComponent<CapsuleCollider2D>().offset.y), (Vector2)m_gPlayer.transform.position - m_gPlayer.GetComponent<CircleCollider2D>().offset, m_WallLayer) 
                            || !Physics2D.OverlapCircle((Vector2)this.transform.position, 4f,m_WallLayer))
                        {
                            Fire();
                        }
                        else
                        {
                            m_bCanMove = true;
                            m_bFinishedFiring = true;
                            m_fTimeBetweenShots = m_fFireRate;
                        }
                    }
                    else
                    {
                        m_gCompanion = GameObject.FindGameObjectWithTag("Companion");
                        if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gCompanion.transform.position, m_WallLayer))
                        {
                            
                            Fire();
                        }
                        else
                        {
                            m_bCanMove = true;
                            m_bFinishedFiring = true;
                            m_fTimeBetweenShots = m_fFireRate;
                        }
                    }
                }
                // If BurstCount has added up to the amount of shots wanted,
                else
                {
                    m_bFinishedFiring = true;
                    if (m_bCanMove)
                    {
                        // Increment Counter.
                        m_fTimeBetweenBursts += Time.deltaTime;
                        // Make counter in seconds.
                        m_fTimeBetweenBursts = m_fTimeBetweenBursts % 60;
                    }
                    // If counter has reacher the timer,
                    if (m_fTimeBetweenBursts >= m_fBurstTimer)
                    {
                        // Reset timer.
                        m_fTimeBetweenBursts = 0.0f;
                        // Reset Counter.
                        m_nBurstCount = 0;
                    }
                }
            }
            // If health is less than or equal to zero,
            if (m_nCurrentHealth <= 0)
            {
                OnDeath();
                Die();
            }
        }
        else
        {
            // Increment timer.
            m_fSpawnTimer += 1 * Time.deltaTime;
            m_fSpawnTimer = m_fSpawnTimer % 60;

            // When timer reaches stun time,
            if (m_fSpawnTimer > m_fSpawnTime)
            {

                // Unstun this enemy.
                m_bSpawnStun = false;
                // Reset the Timer.
                m_fSpawnTimer = 0;
            }
        }
        if (m_bKnockBack)
        {
            m_fKnockTimer += Time.deltaTime * m_fKnockSpeed;
            m_gPlayer.transform.parent.position = Vector2.Lerp(m_v2StartKnockPos, m_v2EndKnockPos + m_gPlayer.GetComponentInParent<CircleCollider2D>().offset, m_fKnockTimer);

            if (Vector2.Distance(m_gPlayer.transform.parent.position, m_v2EndKnockPos + m_gPlayer.GetComponentInParent<CircleCollider2D>().offset) <= 0.1f)
            {
                m_bKnockBack = false;
                m_fKnockTimer = 0.0f;
            }
        }

        if(!m_bFinishedFiring && m_fBurstTimer >= m_fTimeBetweenBursts * 0.5f)
        {
            StartCoroutine(AnimatorSetFire(1.0f));
        }
    }
    //--------------------------------------------------------------------------------------
    // Fire: Spawn a projectile and fire it towards the player.
    //--------------------------------------------------------------------------------------
    void Fire()
    {
        m_bFinishedFiring = false;
        m_bCanMove = false;
        Vector3 m_V3Spawn = Vector3.zero;
        if (m_bMovingLeft)
        {
            //m_V3Spawn = new Vector3(transform.position.x - (GetComponent<CapsuleCollider2D>().size.x * 0.5f), transform.position.y - (GetComponent<CapsuleCollider2D>().size.y * 0.1f), 1);
        }
        else
        {
            //m_V3Spawn = new Vector3(transform.position.x + (GetComponent<CapsuleCollider2D>().size.x * 0.5f), transform.position.y - (GetComponent<CapsuleCollider2D>().size.y * 0.1f), 1);
        }
        // Instantiate a bullet.
        GameObject newBullet = Instantiate(m_gProjectile, transform.position + new Vector3(0, GetComponent<CapsuleCollider2D>().offset.y), Quaternion.Euler(0, 0, 0)) as GameObject;
        m_nBurstCount++;

        if (m_bTaunted)
        {
            m_v3Target = m_gCompanion.transform.position - transform.position;
            m_v3Target.Normalize();
        }
        else
        {
            // Get the target position.
            m_v3Target = m_gPlayer.transform.position - transform.position;
            m_v3Target.Normalize();
        }
        // Calculate rotation needed to face Player.
        float angle = Mathf.Atan2(m_v3Target.y, m_v3Target.x) * Mathf.Rad2Deg;
        // Set bullets rotation to face Player.
        newBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        // Set bullets damage to this SwordMage damage value.
        newBullet.GetComponent<EnemyBullet>().m_nDam = m_nDamage;
        // Set bullets damage to this SwordMage bullet speed.
        newBullet.GetComponent<EnemyBullet>().m_fSpeed = m_fBulletSpeed;

        // Reset timer.
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

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            //Vector3 dir = transform.position - collision.transform.position;
            //dir.Normalize();
            //transform.position += dir * 0.01f;
            //transform.position = (Vector2)transform.position;

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
        if (!m_bSpawnStun)
        {
            Vector3 dir = transform.position - collision.transform.position;
            dir.Normalize();
            // If the Slime has collided with the Player,
            if (collision.tag == "Player")
            {
                // Damage the player.
                m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDamage);

                if (!m_bKnockBack)
                {
                    KnockPlayer(dir);
                }
            }
        }
    }
    void KnockPlayer(Vector3 dir)
    {
        // Knockback the player.
        m_bKnockBack = true;
        int count = 0; // Count of collisions detected
        RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of objects the ray collides with
        Vector2 rayOrigin = (Vector2)m_gPlayer.transform.parent.position - new Vector2(m_gPlayer.GetComponentInParent<CircleCollider2D>().offset.x, m_gPlayer.GetComponentInParent<CircleCollider2D>().offset.y); // Gets ray origin based on player position and collider offset
        count = Physics2D.Raycast(rayOrigin, (Vector2)(m_gPlayer.transform.parent.position - transform.position), m_cFilter, Hit, m_fKnockDistance); // Ray casts in direction of movement
        Debug.DrawRay(rayOrigin, m_gPlayer.transform.parent.position - transform.position, Color.magenta, m_fKnockDistance);
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

    //https://answers.unity.com/questions/1386501/set-animator-boolean-false-after-animation-conplet.html
    //This gives you trigger like behavior from a bool
    private IEnumerator AnimatorSetFire(float animationLength)
    {
        m_Animator.SetBool("isAttacking", true);

        //You can wait for seconds, frames, other coroutines, whatever u need 
       yield return new WaitForSeconds(animationLength);

        m_Animator.SetBool("isAttacking", false);
    }
}