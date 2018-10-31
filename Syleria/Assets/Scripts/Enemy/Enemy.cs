//--------------------------------------------------------------------------------------
// Purpose: Used as the parent for all Enemy scripts.
//
// Description: Contains functions and variables that most if not all
//              enemies will use at some point.
//
// Author: Callan Davies
//
// Contributors: Hamish Smithers
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Variables")]
    [Tooltip("How close the player must be before aggroing the enemy")]
    public float m_fIdleDistance;

    [Tooltip("How far the enemy will stay away from the player for shooting")]
    public float m_fAimDistance;

    public enum State { IDLE, CHASE, ASTAR, ATTACK, TAUNTED }

    public enum EnemyType { SLIME, SHOTGUN, SENTRY, SWORD, BOSS }

    #region AStar
    public List<Node> m_Path;
    public Pathing m_aStar;
    [SerializeField] private float m_fAStarSpeed = 0.01f;
    #endregion

    [Tooltip("Pointer to the Player")]
    public GameObject m_gPlayer; // Reference to player

    // What enemy this one is.
    [Tooltip("What Enemy is this")]
    public EnemyType m_eEnemyType;

    [Tooltip("What state this Enemy is currently in")]
    public State m_eState = State.IDLE; // Starting state for Enemy

    // LayerMask for the objects the enemies can't go through
    public LayerMask m_WallLayer;

    // Maximum health for the Enemy
    [Tooltip("Maximum health for this Enemy")]
    public int m_nHealth;

    // The CurrentHealth of the Enemy
    [Tooltip("The Current Health of this enemy")]
    public int m_nCurrentHealth;

    // Damage that the enemy will do
    [Tooltip("Damage that this enemy will do to the player")]
    public int m_nDamage;

    [Tooltip("Amount of seconds the slime will spend as red")]
    public float m_fFlashTime;

    public GameObject m_gHealthOrb;

    public float m_fOrbDropChance;

    public GameObject m_gSpawnParticle;

    public GameObject m_gDeathParticle;

    public GameObject m_gHitSlash;

    public AudioSource m_audioSource;

    public GameObject m_gSFXPrefab;

    public AudioClip m_sDeathSFX;

    public AudioClip m_sShootSFX;

    public float m_fAudioPitchOffset=0f;

    public Vector2 m_fOffset;

    //Boolean for raycasts
    [HideInInspector]
    public bool m_bHit;

    // A contact filter, so the raycasts can use the layerMask correctly.
    [HideInInspector]
    public ContactFilter2D m_cFilter;

    // Pointer to the companion that taunts this enemy.
    [HideInInspector]
    public GameObject m_gCompanion;

    [HideInInspector]
    public bool m_bCanShoot = true;

    // If the enemy is currently moving left, mostly obsolete.
    [HideInInspector]
    public bool m_bMovingLeft;

    // Boolean for if this enemy is being taunted currently.
    [HideInInspector]
    public bool m_bTaunted = false;

    // Boolean for if this enemy has ever seen the player.
    private bool m_bSeenPlayer;

    // Timer for the flashing of the enemy when they take damage.
    private float m_fFlashTimer = 0f;

    // Pointer to this enemy's target.
    private Vector2 m_gTarget;

    private Vector2 m_v2PlayerOffset;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public void Awake()
    {

        m_nCurrentHealth = m_nHealth;
        m_bTaunted = false;
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_v2PlayerOffset = new Vector2(0, m_gPlayer.GetComponent<CapsuleCollider2D>().size.y);
        m_aStar = GameObject.FindGameObjectWithTag("A*").GetComponent<Pathing>();
        m_aStar = m_aStar.GetComponent<Pathing>(); // Gets pathing component
        m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position); // Finds starting path to player
        m_cFilter.layerMask = m_WallLayer;
        m_cFilter.useLayerMask = true;
        m_bSeenPlayer = false;
        GameObject gSpawnParticle = Instantiate(m_gSpawnParticle, this.transform.position, Quaternion.identity) as GameObject;
        Destroy(gSpawnParticle, 1.5f);
        m_audioSource = GetComponent<AudioSource>();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public void Update()
    {
        if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position - m_v2PlayerOffset) <= m_fIdleDistance)
        {
            m_bSeenPlayer = true;
        }
        StateMachine(); // Calls state machine

        StartCoroutine("UpdateState");
        SetTarget((Vector2)m_gPlayer.transform.position - m_gPlayer.GetComponent<CircleCollider2D>().offset);

        if (m_bHit)
        {
            m_fFlashTimer += Time.deltaTime;
            m_fFlashTimer %= 60;

            if (m_fFlashTimer > m_fFlashTime)
            {
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
                m_bHit = false;
                m_fFlashTimer = 0;
            }
        }
        if (m_nCurrentHealth <= 0)
        {
            //OnDeath();
        }
    }

    public IEnumerator UpdateState()
    {
        if (!m_bTaunted)
        {

            if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gPlayer.transform.position - m_v2PlayerOffset/*new Vector2(0, 0.59f)*/, m_WallLayer))
            {               
                m_eState = State.CHASE;
            }
            else
            {
                m_eState = State.ASTAR;
            }   

            if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position - m_gPlayer.GetComponent<CircleCollider2D>().offset) <= m_fAimDistance)
            {
                if (!Physics2D.Linecast((Vector2)this.transform.position + new Vector2(0, GetComponent<CapsuleCollider2D>().offset.y), (Vector2)m_gPlayer.transform.position - new Vector2(0,0.2f), m_WallLayer) && !Physics2D.OverlapCircle((Vector2)this.transform.position, 0.2f, m_WallLayer)
                    || !Physics2D.OverlapCircle((Vector2)this.transform.position, 4f, m_WallLayer))
                {
                    if (m_eEnemyType == EnemyType.SHOTGUN || m_eEnemyType == EnemyType.SWORD)
                    {
                        m_eState = State.IDLE;
                    }
                }
            }
            if (!m_bSeenPlayer)
            {
                m_eState = State.IDLE;
            }
        }
        else
        {
            m_eState = State.TAUNTED;
        }
        yield return new WaitForSeconds(.1f);
    }

    public void StateMachine()
    {
        switch (m_eState)
        {
            case State.IDLE:
                // Is next to player and doesn't need to follow or attack            
                break;
            case State.CHASE:
                // Can directly see player so follows with basic obstacle avoidance 
                Follow(m_gTarget);
                break;
            case State.ASTAR:
                // When following but walls are in way of target
                Vector2 aStarTargetPos = m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset;
                AStar(m_gTarget);
                break;
            case State.ATTACK:
                // Enemy is in range for attacking
                break;
            case State.TAUNTED:
                // Enemy is being taunted
                // When following but walls are in way of target
                if (m_eEnemyType != EnemyType.SENTRY)
                {
                    m_gCompanion = GameObject.FindGameObjectWithTag("Companion");
                    if (m_gCompanion)
                    {
                        AStar(m_gCompanion.transform.position);
                    }
                    else
                    {
                        m_bTaunted = false;
                    }
                }

                break;
            default:
                break;
        }
    }

    public void OnDeath()
    {
        if (m_gDeathParticle)
        {
            GameObject gSFXPrefab = Instantiate(m_gSFXPrefab) as GameObject;
            gSFXPrefab.GetComponent<AudioSource>().clip = m_sDeathSFX;
            gSFXPrefab.GetComponent<AudioSource>().pitch = Random.Range(m_fAudioPitchOffset+ 0.9f, m_fAudioPitchOffset + 1.1f);
            gSFXPrefab.GetComponent<AudioSource>().volume = 0.5f;
            gSFXPrefab.GetComponent<AudioSource>().Play();

            Destroy(gSFXPrefab, 2f);

            GameObject gDeathParticle = Instantiate(m_gDeathParticle, this.transform.position, Quaternion.identity) as GameObject;
            Destroy(gDeathParticle, 5f);
        }
        DropHealthOrb();
    }

    public void OnShoot()
    {
        GameObject gSFXPrefab = Instantiate(m_gSFXPrefab) as GameObject;
        gSFXPrefab.GetComponent<AudioSource>().clip = m_sShootSFX;
        gSFXPrefab.GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
        gSFXPrefab.GetComponent<AudioSource>().volume = 0.5f;
        gSFXPrefab.GetComponent<AudioSource>().Play();
    }

    public void Follow(Vector2 TargetPosition)
    {
        Vector2 DirToTarget = (TargetPosition) - (Vector2)this.transform.position;
        DirToTarget.Normalize();
        this.transform.Translate((DirToTarget) * m_fAStarSpeed * Time.deltaTime);
    }
    public bool AStar(Vector2 TargetPosition)
    {
        if (TargetPosition != null)
        {
            m_Path = m_aStar.FindPath(this.transform.position, TargetPosition); // Finds path to target
        }
        else
        {
            m_Path = null;
        }
        if (m_Path != null)
        {
            if (m_Path.Count > 1)
            {
                Vector2 dirToNextNode = m_Path[0].WorldPosition - (Vector2)this.transform.position; // Sets direction to next node in list
                dirToNextNode.Normalize(); // Normalize direction
                transform.Translate(dirToNextNode * m_fAStarSpeed * Time.deltaTime); // translate to next node
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    //--------------------------------------------------------------------------------------
    // TakeDamage: Calling this on any enemy will damage it.
    //
    // Parameters:
    //      int nDamage: The Enemy will have their current health decreased by this value.
    //--------------------------------------------------------------------------------------
    public void TakeDamage(int nDamage)
    {
        // Current health = currentHealth - damage.
        m_nCurrentHealth -= nDamage;

        //GetComponentInChildren<SpriteRenderer>().color = Color.red;
        GameObject gHitSlash = Instantiate(m_gHitSlash, GetComponentInChildren<SpriteRenderer>().bounds.center, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GameObject;
        float randomScale = Random.Range(0.95f, 1.3f);
        gHitSlash.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        gHitSlash.GetComponent<HitSlash>().targetRender = GetComponentInChildren<SpriteRenderer>();
        Destroy(gHitSlash, 0.5f);
        m_bHit = true;

        m_audioSource.pitch = (Random.Range(m_fAudioPitchOffset + 0.9f, m_fAudioPitchOffset + 1.1f));
        m_audioSource.Play();
    }

    public void SetTarget(Vector2 gTarget)
    {
        m_gTarget = gTarget;
    }


    //--------------------------------------------------------------------------------------
    // DropHealthOrb: Calls on all enemies death to possibly drop a health orb.
    //                
    //--------------------------------------------------------------------------------------
    public void DropHealthOrb()
    {
        float fRand = Random.Range(0, 100);
        if(fRand <= m_fOrbDropChance)
        {
            Instantiate(m_gHealthOrb);
        }
    }
}
