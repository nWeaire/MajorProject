using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private GameObject m_oPlayer;
    private Vector2 m_v2StartPos;

	// Use this for initialization
	void Start ()
    {
        m_oPlayer = GameObject.FindGameObjectWithTag("Player");
        m_v2StartPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
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
    }

    void CheckRange()
    {
        if(Vector2.Distance(this.transform.position, m_v2StartPos) >= m_oPlayer.GetComponent<Player>().GetRange())
        {
            Destroy(this.gameObject);
        }
    }
}
