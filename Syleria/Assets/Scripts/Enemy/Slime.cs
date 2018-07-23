using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : Enemy
{
    // If true, this slime will spawn smaller slimes on death
    public bool m_bBigSlime;

    public GameObject m_goSmallSlime;

    private float m_fTimer = 0;

    public bool m_bCannotMove;

    // Use this for initialization
    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update()
    {
        if (!m_bCannotMove)
        {
            Seek(new Vector2(m_gPlayer.transform.position.x, m_gPlayer.transform.position.y));
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

	}

    private void FixedUpdate()
    {

        Vector2 v2Dir = new Vector2(transform.position.x - m_gPlayer.transform.position.x, transform.position.y - m_gPlayer.transform.position.x);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, v2Dir, 5, 8);
        Debug.DrawLine(transform.position, v2Dir);


        if(Physics2D.Linecast(transform.position, m_gPlayer.transform.position, 8))
        {
            Debug.Log("AAA");
        }
        if(hit.collider != null)
        {
            Debug.Log("AAAAaA");
        }


    }

    // Seek Towards the Player
    void Seek(Vector2 tTarget)
    {
        // Move to the target
        transform.position = Vector2.MoveTowards(transform.position, tTarget, m_fSpeed * Time.deltaTime);
        //GetComponent<NavMeshAgent>().SetDestination(tTarget);
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
            Destroy(gameObject);
        }
        else
        {
            // Normal death stuff here
        }
    }
}
