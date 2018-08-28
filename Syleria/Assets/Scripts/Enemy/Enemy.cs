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

    public enum State { IDLE, CHASE, ASTAR, ATTACK }

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

    // Maximum health for the Enemy
    [Tooltip("Maximum health for this Enemy")]
    public int m_nHealth;

    // The CurrentHealth of the Enemy
    [Tooltip("The Current Health of this enemy")]
    public int m_nCurrentHealth;

    // Damage that the enemy will do
    [Tooltip("Damage that this enemy will do to the player")]
    public int m_nDamage;

    [HideInInspector]
    public ContactFilter2D m_cFilter;

    [SerializeField] private float m_fSeekSpeed = 3.0f; // Seek move speed


    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public void Start()
    {
        m_aStar = GameObject.FindGameObjectWithTag("A*").GetComponent<Pathing>();
        m_aStar = m_aStar.GetComponent<Pathing>(); // Gets pathing component
        m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position); // Finds starting path to player
        m_cFilter.layerMask = m_WallLayer;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public void Update()
    {
        StateMachine(m_gPlayer.transform.position); // Calls state machine
        StartCoroutine("UpdateState");
    }

    public IEnumerator UpdateState()
    {
        if (!Physics2D.Linecast((Vector2)this.transform.position, m_gPlayer.transform.position, m_WallLayer))
        {
            m_eState = State.CHASE;
        }
        else
        {
            m_eState = State.ASTAR;
        }
        if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset) <= 6f)
        {
            if (m_eEnemyType == EnemyType.SHOTGUN)
            {
                m_eState = State.IDLE;
            }
        }
        yield return new WaitForSeconds(.1f);
    }

    public void StateMachine(Vector3 v3Target)
    {
        switch (m_eState)
        {
            case State.IDLE:
                // Is next to player and doesn't need to follow or attack            
                break;
            case State.CHASE:
                // Can directly see player so follows with basic obstacle avoidance 
                Vector2 dirToPlayer = (v3Target + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset) - this.transform.position;
                dirToPlayer.Normalize();
                this.transform.Translate((dirToPlayer) * m_fSeekSpeed * Time.deltaTime);
                break;
            case State.ASTAR:
                // When following but walls are in way of target
                m_Path = m_aStar.FindPath(this.transform.position, v3Target + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset); // Finds path to player
                Vector2 dirToNextNode = m_Path[0].WorldPosition - (Vector2)this.transform.position; // Sets direction to next node in list
                dirToNextNode.Normalize(); // Normalize direction
                transform.Translate(dirToNextNode * m_fAStarSpeed * Time.deltaTime); // translate to next node
                break;
            case State.ATTACK:
                // Enemy is in range for attacking
                break;
            default:
                break;
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
    }
}
