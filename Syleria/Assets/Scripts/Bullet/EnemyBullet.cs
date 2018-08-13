using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    // Range until the bullet despawns
    [Tooltip("Range until bullet despawns")]
    public float m_fRange;

    [HideInInspector]
    public int m_nDam;

    public float m_fSpeed;

    private GameObject m_gPlayer;

    private Vector2 m_v2StartPos;

    // Use this for initialization
    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_v2StartPos = this.transform.position;
    }

    private void Update()
    {
        // Comment out this line to make bullets go straight
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, Mathf.Sin(Time.time) * 1.5f), ForceMode2D.Force);
        // Move forward by speed * deltaTime
        transform.position += transform.up * m_fSpeed * Time.deltaTime;
        CheckRange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Destroy(this.gameObject);
        }
        if(collision.tag == "Shield")
        {
            Debug.Log("Hit Shield");
            Destroy(this.gameObject);
        }

        if(collision.tag == "Player")
        {
            m_gPlayer.GetComponent<Player>().AddCurrentHealth(-m_nDam);


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