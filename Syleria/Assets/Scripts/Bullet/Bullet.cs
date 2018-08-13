using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float m_fSpeed;

    private GameObject m_gPlayer;
    private Vector2 m_v2StartPos;

	// Use this for initialization
	void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_v2StartPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Move forward by speed * deltaTime
        transform.position += transform.up * m_fSpeed * Time.deltaTime;
        // Check Range for deletion
        CheckRange();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(m_gPlayer.GetComponent<Player>().GetDamage());
            Destroy(this.gameObject);
        }

    }

    void CheckRange()
    {
        if(Vector2.Distance(this.transform.position, m_v2StartPos) >= m_gPlayer.GetComponent<Player>().GetRange())
        {
            Destroy(this.gameObject);
        }
    }
}
