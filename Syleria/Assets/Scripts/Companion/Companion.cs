//--------------------------------------------------------------------------------------
// Purpose: Handles Movement and state changes of companions
//
// Description:  Handles movement and state changes of companion, including attack, idle, follow and aStar States
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{

    public enum State { IDLE, FOLLOW, PATH, ATTACK, TAUNT } // Enum controlling states of companion

    #region AStar
    [SerializeField] private float m_fAStarSpeed = 0.01f;
    public List<Node> m_Path;
    public Pathing m_aStar;
    #endregion

    public GameObject m_gPlayer; // Reference to player
    [SerializeField] private float m_fFollowSpeed = 3.0f; // Follow move speed
    public State m_eState; // Starting state for companion
    public LayerMask m_wallLayer; // LayerMask for sight check
    private bool isAttacking = false;
    private GameObject m_gTarget;
    private float m_fAttackTimer = 0;
    [SerializeField] private float m_fAttackCD = 1.0f;
    [SerializeField] private int m_nAttackPercentage = 40;
    [SerializeField] private int m_nDamage;
    private bool m_bAttackOnCD = false;

    private Animator m_Animator;
    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_eState = State.IDLE;
        m_aStar = m_aStar.GetComponent<Pathing>(); // Gets pathing component
        m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position); // Finds starting path to player
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine(); // Calls state machine
        if (!isAttacking)
        {
            StartCoroutine("UpdateState");
        }
        UpdateAnimation();
    }

    public IEnumerator UpdateState()
    {
        if (!Physics2D.Linecast((Vector2)this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset, m_wallLayer)) // Checks if companion can direction see player
        {
            m_eState = State.FOLLOW; // Sets state to follow
        }
        else
        {
            m_eState = State.PATH; // Sets state to AStar
        }
        if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset) <= 1.7f) // Checks if companion is within 1.4 unit of player 
        {
            m_eState = State.IDLE; // Sets state to idle
        }
        

        yield return new WaitForSeconds(.1f); // Waits a tenth of a second to repeat this function
    }

    public void StateMachine()
    {
        switch (m_eState)
        {
            case State.IDLE:
                // Is next to player and doesn't need to follow or attack            
                break;
            case State.FOLLOW:
                // Can directly see player so follows with basic obstacle avoidance 
                Vector2 FollowTargetPos = m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset;
                AStar(FollowTargetPos);
                break;
            case State.PATH:
                // When following but walls are in way of target
                Vector2 aStarTargetPos = m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset;
                AStar(aStarTargetPos);
                break;
            case State.ATTACK:
                if(m_gTarget != null)
                {
                    if(Vector2.Distance(this.transform.position, m_gTarget.transform.position) >= 1.5f)
                    {
                        Follow((Vector2)m_gTarget.transform.position);
                    }
                    else if(!m_bAttackOnCD)
                    {
                        StartCoroutine("UpdateDamage");
                        m_gTarget.GetComponent<Enemy>().TakeDamage(m_nDamage);
                        m_bAttackOnCD = true;
                    }
                    else
                    {
                        m_fAttackTimer += Time.deltaTime;
                        if(m_fAttackTimer >= m_fAttackCD)
                        {
                            m_bAttackOnCD = false;
                            m_fAttackTimer = 0f;
                        }
                    }
                }
                else
                {
                    isAttacking = false;
                }

                break;
            case State.TAUNT:

                break;
            default:
                break;
        }
    }
    public GameObject[] getEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            return enemies;
        }
        else
        {
            return null;
        }
    }

    public void Follow(Vector2 TargetPosition)
    {
        Vector2 DirToTarget = (TargetPosition) - (Vector2)this.transform.position;
        DirToTarget.Normalize();
        this.transform.Translate((DirToTarget) * m_fFollowSpeed * Time.deltaTime);
    }
    public void AStar(Vector2 TargetPosition)
    {
        m_Path = m_aStar.FindPath(this.transform.position, TargetPosition); // Finds path to target
        if (m_Path.Count > 1)
        {
            Vector2 dirToNextNode = m_Path[0].WorldPosition - (Vector2)this.transform.position; // Sets direction to next node in list
            dirToNextNode.Normalize(); // Normalize direction
            transform.Translate(dirToNextNode * m_fAStarSpeed * Time.deltaTime); // translate to next node
        }
        else
        {

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_eState != State.ATTACK)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (!Physics2D.Linecast(this.transform.position, collision.transform.position, m_wallLayer))
                {
                    isAttacking = true;
                    m_eState = State.ATTACK;
                    m_gTarget = collision.gameObject;
                }
            }
        }
    }

    public IEnumerator UpdateDamage()
    {
        m_nDamage = (m_gPlayer.GetComponentInChildren<Player>().GetDamage() * m_nAttackPercentage) / 100;
        yield return new WaitForSeconds(10.0f);
    }

    public void UpdateAnimation()
    {
        // Boolean setting for the sprite
        if (transform.position.x - m_gPlayer.transform.position.x >= 0 && m_eState != State.ATTACK)
        {
            // Face left 
            m_Animator.SetBool("isLeft", true);
        }
        else if (m_eState == State.ATTACK)
        {
            if (transform.position.x - m_gTarget.transform.position.x >= 0)
            {
                // Face left 
                m_Animator.SetBool("isLeft", true);
            }
        }
        else
        {
            // Face right
            m_Animator.SetBool("isLeft", false);
        }

      

        if(m_eState == State.IDLE)
        {
            m_Animator.SetBool("isMoving", false);
        }
        else
        {
            m_Animator.SetBool("isMoving", true);
        }

        if(m_eState == State.ATTACK)
        {
            m_Animator.SetBool("isAttacking", true);
        }
        else
        {
            m_Animator.SetBool("isAttacking", false);
        }

    }
}