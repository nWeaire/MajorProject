using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour {

    public enum State{IDLE, FOLLOW, ASTAR, ATTACK}

    #region AStar
    [SerializeField] private float m_fAStarSpeed = 0.01f;
    public List<Node> m_Path;
    public Pathing m_aStar;
    private float m_fDistToNode;
    #endregion

    public GameObject m_gPlayer; // Reference to player
    [SerializeField] private float m_fFollowSpeed = 3.0f; // Follow move speed
    public State m_eState = State.IDLE; // Starting state for companion
    public LayerMask m_wallLayer; // LayerMask for sight check

    // Use this for initialization
    void Start ()
    {
        m_aStar = m_aStar.GetComponent<Pathing>(); // Gets pathing component
        m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position); // Finds starting path to player
	}
	
	// Update is called once per frame
	void Update ()
    {
        StateMachine(); // Calls state machine
    }

    public void UpdateState()
    {
        if(!Physics2D.Linecast((Vector2)this.transform.position, m_gPlayer.transform.position, m_wallLayer))
        {
            m_eState = State.FOLLOW;
        }
        else
        {
            m_eState = State.ASTAR;
        }
        if(Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position + m_gPlayer.GetComponent<CircleCollider2D>().offset) <= 1.4f)
        {
            m_eState = State.IDLE;
        }
    }

    public void StateMachine()
    {
        switch (m_eState)
        {
            case State.IDLE:
                // Is next to player and doesn't need to follow or attack
                UpdateState();
                break;
            case State.FOLLOW:
                // Can directly see player so follows with basic obstacle avoidance 
                Vector2 dirToPlayer = (m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset) - this.transform.position;
                dirToPlayer.Normalize();
                this.transform.Translate((dirToPlayer) * m_fFollowSpeed * Time.deltaTime);
                UpdateState();
                break;
            case State.ASTAR:
                // When following but walls are in way of target
                m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset); // Finds path to player
                Vector2 dirToNextNode = m_Path[0].WorldPosition - (Vector2)this.transform.position; // Sets direction to next node in list
                dirToNextNode.Normalize(); // Normalize direction
                transform.Translate(dirToNextNode * m_fAStarSpeed * Time.deltaTime); // translate to next node

                UpdateState(); // Checks for state update
                break;
            case State.ATTACK:
                // Enemy is in range for attacking

                break;

            default:
                break;
        }
    }
}
