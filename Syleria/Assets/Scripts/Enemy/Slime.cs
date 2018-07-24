using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    // If true, this slime will spawn smaller slimes on death
    public bool m_bBigSlime;

    public GameObject m_goSmallSlime;

    private float m_fTimer = 0;

    public bool m_bCannotMove;

    public ContactFilter2D m_cfFilter;

    public GameObject m_gLeftFeeler;

    public GameObject m_gRightFeeler;

    private Vector2 m_v2Target;

    public Vector2 m_v2Velocity;

    private Vector2 Force;

    private bool m_bLeftHit;

    private bool m_bMiddleHit;

    private bool m_bRightHit;

    // Use this for initialization
    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        // sets the layermask to 8 ("Terrain")
    }
	
	// Update is called once per frame
	void Update()
    {
        Vector3 v3LastPos;
        if (!m_bCannotMove)
        {
            Seek();
            v3LastPos = transform.position;
        }
        else
        {
            m_fTimer += 1 * Time.deltaTime;
        }
        if(m_fTimer > 1)
        {
            m_bCannotMove = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Die();
        }
        AvoidObstacles();

        transform.up = m_v2Velocity;

        //Vector3 moveDirection = gameObject.transform.position - new Vector3;
        //if (moveDirection != Vector3.zero)
        //{
        //    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //}

    }

    private void FixedUpdate()
    {
    }

    // Seek Towards the Player
    void Seek()
    {
        // Move to the target
        // Calculate Veloctity
        m_v2Velocity = ((Vector2)m_gPlayer.transform.position - (Vector2)transform.position).normalized * m_fMaxVelocity;
        Force = m_v2Velocity;
        m_v2Velocity += Force * Time.deltaTime;
        Vector2 pos = transform.position;
        // move the AI to position + velocity
        pos += m_v2Velocity * Time.deltaTime;
        transform.position = pos;
    }

    public void AvoidObstacles()
    {

        RaycastHit2D[] aHit = new RaycastHit2D[1];

        int count = 0;

        count = Physics2D.Raycast((Vector2)transform.position, m_v2Velocity, m_cfFilter, aHit, 2.0f);
        

        if (count > 0)
        {
            m_bMiddleHit = true;
        }
        else
        {
            m_bMiddleHit = false;
        }

        Vector2 leftR = (Vector2)transform.position;
        count = Physics2D.Raycast((Vector2)m_gLeftFeeler.transform.position, m_v2Velocity, m_cfFilter, aHit, 2.0f);

        if (count > 0)
        {
            m_bLeftHit = true;
        }
        else
        {
            m_bLeftHit = false;
        }

        Vector2 rightR = (Vector2)transform.position;
        count = Physics2D.Raycast((Vector2)m_gRightFeeler.transform.position,m_v2Velocity, m_cfFilter, aHit, 2.0f);

        if (count > 0)
        {
            m_bRightHit = true;
        }
        else
        {
            m_bRightHit = false;
        }

        // Display the ray casts in the scene view
        foreach (RaycastHit2D element in aHit)
        {
            if (element.point != new Vector2(0, 0))
            {
                Debug.DrawLine(transform.position, element.point, Color.red);
            }
        }

        if(m_bLeftHit && m_bMiddleHit && !m_bRightHit)
        {
            Debug.Log("go Right");
        }
        if(m_bRightHit && m_bMiddleHit && !m_bLeftHit)
        {
            Debug.Log("go left");
        }
        if(m_bRightHit && m_bMiddleHit && m_bLeftHit)
        {
            Debug.Log("Move Towards longest point");
        }

    }

    void Die()
    {
        if (m_bBigSlime)
        {
            // Spawn Slimes (temp)
            Instantiate(m_goSmallSlime,new Vector2(transform.position.x - 0.5f,transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x + 0.5f, transform.position.y), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y - 0.5f), transform.rotation);
            Instantiate(m_goSmallSlime, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            // Normal death stuff here
        }
    }
}
