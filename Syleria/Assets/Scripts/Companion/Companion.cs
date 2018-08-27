using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour {

    public enum State
    {
        IDLE,
        FOLLOW,
        ASTAR,
        ATTACK
    }

    public List<Node> m_Path;
    public Pathing m_aStar;
    public GameObject m_gPlayer;
    public GameObject m_gNode;
    [SerializeField] private float m_fFollowSpeed = 3.0f;
    [SerializeField] private float m_fAStarSpeed = 0.01f;
    public State m_eState = State.IDLE;
    public LayerMask m_wallLayer;
    private float m_fDistToNode;

    // Use this for initialization
    void Start ()
    {
        m_aStar = m_aStar.GetComponent<Pathing>();
        m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            m_eState = State.ASTAR;
        }
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
                m_Path = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position + (Vector3)m_gPlayer.GetComponent<CircleCollider2D>().offset);
                    Vector2 dirToNextNode = m_Path[0].WorldPosition - (Vector2)this.transform.position;
                    dirToNextNode.Normalize();
                    m_fDistToNode = Vector2.Distance(this.transform.position, m_Path[0].WorldPosition);
                    Debug.DrawLine(this.transform.position, m_Path[0].WorldPosition);
                    transform.Translate(dirToNextNode * m_fAStarSpeed * Time.deltaTime);
                    //transform.position = Vector2.Lerp(this.transform.position, m_Path[i].WorldPosition, 0.01f);
                //if(m_fDistToNode <= 0.5f)
                //{
                 //   m_Path.RemoveAt(0);
                //}
                UpdateState();

                break;
            case State.ATTACK:
                // Enemy is in range for attacking

                break;

            default:
                break;
        }
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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < m_Path.Count; i++)
        {
            Gizmos.DrawWireCube(m_Path[i].WorldPosition, new Vector2(1, 1));
        }
    }
}
