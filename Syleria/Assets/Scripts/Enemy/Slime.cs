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

    public float m_fSpeed;

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
        // Switch the objects right and up
        // Temporary fix for testing purposes
        // Will remove when it doesn't orbit its target when not switched.
        //Vector3 v3right = transform.right;
        //transform.right = transform.up;
        //transform.up = v3right;

        Vector3 v3LastPos;
        if (!m_bCannotMove)
        {
            
            transform.position += transform.up * m_fSpeed * Time.deltaTime;

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

        //float fDot = m_gPlayer.transform.position.x * transform.position.x + m_gPlayer.transform.position.y * transform.position.y;
    }

    // Seek Towards the Player
    void Seek(Vector3 v3Target)
    {
        // Find the target that will allow us to get to the target
        Vector3 v3TargetDir = v3Target - transform.position;
        v3TargetDir = v3TargetDir.normalized;
        //Debug.Log(Vector3.Dot(transform.forward.normalized, v3TargetDir.normalized));
        //Debug.Log("forward " + transform.forward);
        //float angle = Mathf.Atan2(v3Target.y, v3Target.x) * Mathf.Rad2Deg;
        //Kinda "works", however rotating instantly is not the wanted behaviour
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Debug.DrawRay(this.transform.position, v3TargetDir, Color.red);
        float dot = Vector3.Dot(transform.up.normalized, v3TargetDir);
        float rightDot = Vector3.Dot(transform.right.normalized, v3TargetDir);
        if (dot > 0.9f)
        {
            // Don't rotate if facing target
            Debug.Log("AAAA");
        }
        else if (rightDot > 0) // rotate right as D is on the right.
        {
            transform.Rotate(Vector3.back * (180f * Time.deltaTime));
            Debug.Log("Right");
        }
        else if(rightDot < 0) // rotate left as D is on the Left
        {
            transform.Rotate(Vector3.forward * (180f * Time.deltaTime));
            Debug.Log("Left");
        }

        // Rotates on spot very fast, never seeks
        //Vector3.RotateTowards(transform.position, new Vector3(0, 0, angle),5f,5f);

        // Goes diagonally downwards whilst rotating back and forth
        //Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), 2f * Time.deltaTime);

        // Goes diagonally downwards whilst rotating back and forth
        //transform.Rotate(new Vector3(0, 0, angle));

        // Goes diagonally downards whilst rotating around all angles
        //Quaternion newRotation = Quaternion.AngleAxis(angle, Vector3.up);
        //transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, .05f);

    }

    public void AvoidObstacles()
    {

        RaycastHit2D[] aHit = new RaycastHit2D[1];

        int count = 0;

        count = Physics2D.Raycast((Vector2)transform.position, transform.up, m_cfFilter, aHit, 5.0f);

        Debug.DrawRay(transform.position, transform.up, Color.red);
        if (count > 0)
        {
            m_bMiddleHit = true;
        }
        else
        {
            m_bMiddleHit = false;
        }

        count = Physics2D.Raycast((Vector2)m_gLeftFeeler.transform.position, transform.up, m_cfFilter, aHit, 5.0f);

        if (count > 0)
        {
            m_bLeftHit = true;
        }
        else
        {
            m_bLeftHit = false;
        }

        count = Physics2D.Raycast((Vector2)m_gRightFeeler.transform.position, transform.up, m_cfFilter, aHit, 5.0f);

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

        // If the left feeler has been hit
        if(m_bLeftHit)
        {
            // Rotate towards the right feeler
            Seek(m_gRightFeeler.transform.position);

        }
        // If the right feeler has been hit
        if(m_bRightHit)
        {
            // Rotate towards the left feeler
            Seek(m_gLeftFeeler.transform.position);
        }
        // If all feelers are not hit
        if(!m_bRightHit && !m_bMiddleHit && !m_bLeftHit)
        {
            // Rotate towards player
            Seek(m_gPlayer.transform.position);
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
