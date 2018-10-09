//--------------------------------------------------------------------------------------
// Purpose: Used as the parent for all Enemy scripts.
//
// Description: Contains functions and variables that most if not all
//              enemies will use at some point.
//
// Author: Callan Davies
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

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public void Awake()
    {

        m_nCurrentHealth = m_nHealth;
        m_bTaunted = false;
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_aStar = GameObject.FindGameObjectWithTag("A*").GetComponent<Pathing>();
        m_aStar = m_aStar.GetComponent<Pathing>(); // Gets pathing component
        m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position); // Finds starting path to player
        m_cFilter.layerMask = m_WallLayer;
        m_cFilter.useLayerMask = true;
        m_bSeenPlayer = false;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public void Update()
    {
        if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position - new Vector2(0, m_gPlayer.GetComponent<CapsuleCollider2D>().size.y)) <= m_fIdleDistance)
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

    }

    public IEnumerator UpdateState()
    {
        if (!m_bTaunted)
        {

            if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gPlayer.transform.position - new Vector2(0, 0.59f), m_WallLayer))
            {               
                m_eState = State.CHASE;
            }
            else
            {
                m_eState = State.ASTAR;
            }   

            if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position - m_gPlayer.GetComponent<CircleCollider2D>().offset) <= m_fAimDistance)
            {
                if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gPlayer.transform.position - new Vector2(0,0.2f), m_WallLayer) && !Physics2D.OverlapCircle((Vector2)this.transform.position, 0.2f, m_WallLayer)
                    && !Physics2D.OverlapCircle((Vector2)this.transform.position, 4f, m_WallLayer))
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
                Debug.DrawLine(this.transform.position, m_Path[0].WorldPosition, Color.cyan, 5);
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

        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        m_bHit = true;
    }

    public void SetTarget(Vector2 gTarget)
    {
        m_gTarget = gTarget;
    }
}
