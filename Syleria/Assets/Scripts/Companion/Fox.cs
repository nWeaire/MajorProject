using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour {

    private GameObject m_gPlayer;
    private Vector2 m_v2PlayerPos;
    private float m_fRadius;
    [SerializeField] private float m_fSpeed = 3.0f;
    private Vector2 m_v2DirectionToPlayer;
    private Vector2 m_v2LeftCollisionPoint;
    private Vector2 m_v2RightCollisionPoint;
    [SerializeField] private ContactFilter2D m_cfFilter; // Contact filter for collision detection

    // Use this for initialization
    void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_v2PlayerPos = m_gPlayer.transform.position;
        m_fRadius = this.GetComponent<CircleCollider2D>().radius;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_v2DirectionToPlayer = m_gPlayer.transform.position - this.transform.position;
        Rotate();
        Move();
    }

    public void Move()
    {
        m_v2DirectionToPlayer.Normalize(); // Normalize left analog input
        RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of collisions detected
        int lCount = 0; // Left ray count
        int rCount = 0; // Right ray count 
        lCount += Physics2D.Raycast(m_v2LeftCollisionPoint, m_v2DirectionToPlayer, m_cfFilter, Hit, m_fRadius + 0.1f); // Left ray cast
        rCount += Physics2D.Raycast(m_v2RightCollisionPoint, m_v2DirectionToPlayer, m_cfFilter, Hit, m_fRadius + 0.1f); // Right ray cast

        if (lCount > 0 && rCount <= 0) // if only the left ray hits
        {
            if (Hit[0].normal.x > 0) // Checks normal for sliding direction
            {
                transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
            else if (Hit[0].normal.x < 0) // Checks normal for sliding direction
            {
                transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
            else if (Hit[0].normal.y > 0) // Checks normal for sliding direction
            {
                transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
            else // Checks normal for sliding direction
            {
                transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
        }
        else if (rCount > 0 && lCount <= 0) // if only the right ray hits
        {
            if (Hit[0].normal.x > 0) // Checks normal for sliding direction
            {
                transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
            else if (Hit[0].normal.x < 0) // Checks normal for sliding direction
            {
                transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
            else if (Hit[0].normal.y > 0) // Checks normal for sliding direction
            {
                transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
            else // Checks normal for sliding direction
            {
                transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed)); // Slides against wall
            }
        }
        else if (rCount > 0 && lCount > 0) // If right and left ray hit but not middle
        {

        }
        else
        {
            transform.Translate(m_v2DirectionToPlayer * (Time.deltaTime * m_fSpeed)); // If not rays hit move in direction of input
        }

    }

    //--------------------------------------------------------------------------------------
    // Checks the direction of the left analog input
    // Updates the left and right point for collision detection rays
    //--------------------------------------------------------------------------------------
    void Rotate()
    {
        m_v2DirectionToPlayer.Normalize(); // Normalize left analog input
        m_v2LeftCollisionPoint = this.transform.position; // Left point set to fox position
        m_v2LeftCollisionPoint += new Vector2(-m_v2DirectionToPlayer.y, m_v2DirectionToPlayer.x) / Mathf.Sqrt((m_v2DirectionToPlayer.x * m_v2DirectionToPlayer.x) + (m_v2DirectionToPlayer.y * m_v2DirectionToPlayer.y)) * m_fRadius; // Sets left point for collision
        m_v2RightCollisionPoint = this.transform.position; // Right point set to fox position
        m_v2RightCollisionPoint -= new Vector2(-m_v2DirectionToPlayer.y, m_v2DirectionToPlayer.x) / Mathf.Sqrt((m_v2DirectionToPlayer.x * m_v2DirectionToPlayer.x) + (m_v2DirectionToPlayer.y * m_v2DirectionToPlayer.y)) * m_fRadius; // Sets right point for collision
        Debug.DrawRay(m_v2LeftCollisionPoint, m_v2DirectionToPlayer);
        Debug.DrawRay(m_v2RightCollisionPoint, m_v2DirectionToPlayer);
        Debug.DrawRay(this.transform.position, m_v2DirectionToPlayer);
    }

}
