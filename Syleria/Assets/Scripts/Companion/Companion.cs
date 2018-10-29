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

    public enum State { IDLE, FOLLOW, FOLLOW_OUTOFRANGE, PATH, ATTACK, TAUNT } // Enum controlling states of companion

    #region AStar
    [SerializeField] private float m_fAStarSpeed = 0.01f; // Speed the companions aStar
    public List<Node> m_Path; // Path they aStar on
    #endregion

    public Ability m_gAbility; // Reference to ability script
    public GameObject m_gPlayer; // Reference to player
    [SerializeField] private float m_fFollowSpeed = 3.0f; // Follow move speed
    public State m_eState; // Starting state for companion
    public LayerMask m_wallLayer; // LayerMask for sight check
    private bool isAttacking = false; // Bool for Companion attacking
    private GameObject m_gTarget; // Current target of companion
    private float m_fAttackTimer = 0; // Timer for attack rate
    [SerializeField] private float m_fAttackCD = 1.0f; // Attack cooldown
    [SerializeField] private int m_nAttackPercentage = 40; // Percentage of player damage that companion deals
    [SerializeField] private int m_nDamage; // Damage the companion will deal
    private bool m_bAttackOnCD = false; // bool for attack availbility
    private Animator m_Animator; // Reference to animator
    public float m_fMaxFollowRange = 4.0f;
    private float m_fCurrentFollowRange = 0f;
    public float m_fAttackRange = 5f;

    void Awake()
    {
        m_Animator = GetComponent<Animator>(); // Gets animator component
        m_eState = State.IDLE; // Sets state to idle by default      
        this.transform.position = m_gPlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine(); // Calls state machine
        if (!isAttacking) // Companion not attacking
        {
            StartCoroutine("UpdateState"); // Check for new state
        }
        UpdateAnimation(); // updates animations
    }

    public IEnumerator UpdateState()
    {
        if (!Physics2D.Linecast((Vector2)this.transform.position - new Vector2(0, 0.5f), (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset, m_wallLayer)) // Checks if companion can direction see player
        {

            if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset) >= m_fMaxFollowRange)
            {
                m_eState = State.FOLLOW; // Sets state to follow
            }
            else
            {
                m_eState = State.FOLLOW_OUTOFRANGE;
            }
        }
        else
        {
            Vector2 position = m_gPlayer.GetComponent<CircleCollider2D>().offset;
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
                Follow(FollowTargetPos);
                break;
            case State.FOLLOW_OUTOFRANGE:
                // Can directly see player so follows with basic obstacle avoidance 
                FollowTargetPos = m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset;
                m_fCurrentFollowRange = Vector2.Distance(this.transform.position, FollowTargetPos);
                float speed = (m_fCurrentFollowRange / m_fMaxFollowRange) * m_fFollowSpeed;
                if (speed < 2f)
                {
                    speed = 0f;
                }
                Follow(FollowTargetPos, speed);
                break;
            case State.PATH:
                // When following but walls are in way of target
                Vector2 aStarTargetPos = m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset;
                AStar(aStarTargetPos);
                break;
            case State.ATTACK:
                // When attacking

                if (m_gTarget != null) // If target isn't null
                {
                    if (Vector2.Distance(this.transform.position, m_gTarget.transform.position) >= 1.5f) // Checks if companion is further than 1.5f of enemy
                    {
                        if (!Physics2D.Linecast((Vector2)this.transform.position - new Vector2(0, -0.2f), (Vector2)m_gTarget.transform.position, m_wallLayer)) // Checks if companion can direction see player
                        {
                            Follow((Vector2)m_gTarget.transform.position); // If further then 1.5 units moves to enemy
                        }
                        else
                        {
                            bool temp = AStar((Vector2)m_gTarget.transform.position);
                            if (temp == false)
                            {
                                Follow((Vector2)m_gTarget.transform.position);
                            }
                        }
                    }
                    else if (!m_bAttackOnCD) // If attack isn't on cooldown
                    {
                        StartCoroutine("UpdateDamage"); // Updates damage
                        m_gTarget.GetComponent<Enemy>().TakeDamage(m_nDamage); // Deals damage to enemy
                        m_bAttackOnCD = true; // Puts attack on cooldown
                    }
                    else
                    {
                        m_fAttackTimer += Time.deltaTime; // Updates attack timer
                        if (m_fAttackTimer >= m_fAttackCD) // If attack timer greater then cooldown time
                        {
                            m_bAttackOnCD = false; // Attack off cooldown
                            m_fAttackTimer = 0f; // Attack timer back to 0
                        }
                    }
                }
                else
                {
                    isAttacking = false; // If no target, stop attacking
                }

                break;
            case State.TAUNT:
                // If in taunt state don't do anything
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
    public void Follow(Vector2 TargetPosition, float Speed)
    {
        Vector2 DirToTarget = (TargetPosition) - (Vector2)this.transform.position;
        DirToTarget.Normalize();
        this.transform.Translate((DirToTarget) * Speed * Time.deltaTime);
    }
    public bool AStar(Vector2 TargetPosition)
    {
        if (TargetPosition != null)
        {
            m_Path = GameObject.FindGameObjectWithTag("A*").GetComponent<Pathing>().FindPath(this.transform.position, TargetPosition); // Finds path to target
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
        else if (m_eState != State.ATTACK)
        {
            // Face right
            m_Animator.SetBool("isLeft", false);
        }
        if (m_eState == State.ATTACK)
        {
            if (m_gTarget != null)
            {
                if (transform.position.x - m_gTarget.transform.position.x >= 0)
                {
                    // Face left 
                    m_Animator.SetBool("isLeft", true);
                }
                else
                {
                    // Face right
                    m_Animator.SetBool("isLeft", false);
                }
            }
        }

        if (m_eState == State.IDLE)
        {
            m_Animator.SetBool("isMoving", false);
        }
        else
        {
            m_Animator.SetBool("isMoving", true);
        }

        if (m_eState == State.ATTACK)
        {
            m_Animator.SetBool("isAttacking", true);
        }
        else
        {
            m_Animator.SetBool("isAttacking", false);
        }

    }
}