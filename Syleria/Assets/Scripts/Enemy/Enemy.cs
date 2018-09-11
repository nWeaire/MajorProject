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
    public float m_fIdleDistance;

    public enum State { IDLE, CHASE, ASTAR, ATTACK, TAUNTED }

    public enum EnemyType { SLIME, SHOTGUN, SENTRY}

    #region AStar
    public List<Node> m_Path;
    public Pathing m_aStar;
    [SerializeField] private float m_fAStarSpeed = 0.01f;
    private float m_fDistToNode;
    #endregion

    public GameObject m_gPlayer; // Reference to player

    public EnemyType m_eEnemyType;
  
    public State m_eState = State.IDLE; // Starting state for Enemy

    public LayerMask m_WallLayer;

    [HideInInspector]
    public bool m_bHit;

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

    [HideInInspector]
    public ContactFilter2D m_cFilter;

    [HideInInspector]
    public GameObject m_gCompanion;

    [HideInInspector]
    public bool m_bCanShoot = true;

    [HideInInspector]
    public bool m_bMovingLeft;

    [HideInInspector]
    public bool m_bTaunted = false;

    private bool m_bSeenPlayer;

    public float m_fFlashTimer = 0.0f;

    private Vector2 m_gTarget;



    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public void Start()
    {
        m_bTaunted = false;
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_aStar = GameObject.FindGameObjectWithTag("A*").GetComponent<Pathing>();
        m_gCompanion = GameObject.FindGameObjectWithTag("Companion");
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
        if(Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position - m_gPlayer.GetComponent<CircleCollider2D>().offset) <= m_fIdleDistance)
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

            if(m_fFlashTimer > m_fFlashTime)
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
            if (!Physics2D.Linecast((Vector2)this.transform.position, m_gPlayer.transform.position, m_WallLayer) 
                && Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset) <= 3f)
            {
                m_eState = State.CHASE;
            }
            else
                m_eState = State.ASTAR;

            if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset) <= 4f)
            {
                if (m_eEnemyType == EnemyType.SHOTGUN)
                {
                    m_eState = State.IDLE;
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
                AStar(m_gTarget);
                break;
            case State.ATTACK:
                // Enemy is in range for attacking
                break;
            case State.TAUNTED:
                // Enemy is being taunted
                // When following but walls are in way of target
                if(m_eEnemyType != EnemyType.SENTRY)
                AStar(m_gCompanion.transform.position);
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
    public void AStar(Vector2 TargetPosition)
    {
        m_Path = m_aStar.FindPath(this.transform.position, TargetPosition); // Finds path to target
        if (m_Path.Count > 1)
        {
            Vector2 dirToNextNode = m_Path[1].WorldPosition - (Vector2)this.transform.position; // Sets direction to next node in list
            dirToNextNode.Normalize(); // Normalize direction
            transform.Translate(dirToNextNode * m_fAStarSpeed * Time.deltaTime); // translate to next node
        }
        else
        {

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
