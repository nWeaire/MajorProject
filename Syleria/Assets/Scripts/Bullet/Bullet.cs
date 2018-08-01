using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    GameObject m_oPlayer;

	// Use this for initialization
	void Start ()
    {
        m_oPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckRange();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

    }

    void CheckRange()
    {
        if(Vector2.Distance(this.transform.position, m_oPlayer.transform.position) >= m_oPlayer.GetComponent<Player>().GetRange())
        {
            Destroy(this.gameObject);
        }
    }
}
