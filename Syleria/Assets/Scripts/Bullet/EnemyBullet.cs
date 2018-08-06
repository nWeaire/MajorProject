using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float m_fRange;

    private GameObject m_gPlayer;

    private Vector2 m_v2StartPos;

    // Use this for initialization
    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_v2StartPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
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
        if(collision.tag == "Player")
        {
            m_gPlayer.GetComponent<Player>().AddCurrentHealth(-10);


            Debug.Log("Enemy Bullet Hit");
            Destroy(this.gameObject);
        }
    }

    void CheckRange()
    {
        if (Vector2.Distance(this.transform.position, m_v2StartPos) >= m_fRange)
        {
            Destroy(this.gameObject);
        }
    }
}