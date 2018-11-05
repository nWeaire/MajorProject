//--------------------------------------------------------------------------------------
// Purpose: Handles collision and movement of the bullets
//
// Description:  Handles all collision, movement and interaction with bullets
//
// Author: Nicholas Weaire
// Contributor: Hamish Smithers
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool m_bBounced = false;
    public float m_fSpeed; // Speed of bullet
    private GameObject m_gPlayer; // Reference to player object
    private Vector2 m_v2StartPos; // Start position of bullet
    private bool m_bDoneDamage = false;
    public GameObject m_gDestroyedProjectile; //Sprite to replace bullet
    public GameObject m_gHitSlash; //Sprite to indicate damage

    public AudioClip m_sHitWall;
    public AudioClip m_sHitEnemy;
    // Use this for initialization
    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Sets reference to player object
        m_v2StartPos = this.transform.position; // Sets start position to player position
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gPlayer.GetComponent<Player>().m_bExpand) // If player has the expanding upgrade
        {
            // Scales the bullet based on the distance from the players starting at the base size to a maximum of 4x the based size
            transform.localScale = new Vector2(1 + Vector2.Distance(this.transform.position, m_gPlayer.transform.position) / 4, 1 + Vector2.Distance(this.transform.position, m_gPlayer.transform.position) / 4);
        }

        // Move forward by speed * deltaTime
        transform.position += transform.up * m_fSpeed * Time.deltaTime;
        // Check Range for deletion
        CheckRange();
        if (m_bDoneDamage && m_gPlayer.GetComponent<Player>().m_bPierce == false)
        {
            
            Destroy(this.gameObject);
        }
    }

    //--------------------------------------------------------------------------------------
    // If bullet collides with any trigger colliders
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) // If colliding with terrain
        {
            GameObject GO = Instantiate(m_gDestroyedProjectile, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90f)) as GameObject;
            GO.transform.localScale = transform.localScale;
            AudioSource audioSource = GO.GetComponent<AudioSource>();
            audioSource.clip = m_sHitWall;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
            Destroy(GO, 2f);
            
            Destroy(this.gameObject); // Deletes bullet
        }
        if (collision.tag == "Enemy" && !m_bDoneDamage || collision.tag == "Enemy" && m_gPlayer.GetComponent<Player>().m_bPierce) // If collision with enemy
        {
            GameObject GO = Instantiate(m_gDestroyedProjectile, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90f)) as GameObject;
            GO.transform.localScale = transform.localScale;
            AudioSource audioSource = GO.GetComponent<AudioSource>();
            audioSource.clip = m_sHitEnemy;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
            Destroy(GO, 2f);
            
            collision.GetComponent<Enemy>().TakeDamage(m_gPlayer.GetComponent<Player>().GetDamage()); // Deals damage to enemy based on players damage stat
            m_bDoneDamage = true;
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks distance between bullet position and start position
    // Deletes bullet if over range
    //--------------------------------------------------------------------------------------
    void CheckRange()
    {
        if (Vector2.Distance(this.transform.position, m_v2StartPos) >= m_gPlayer.GetComponent<Player>().GetRange()) // If distance from bullet to start position is greater then player range stat
        {
            GameObject GO = Instantiate(m_gDestroyedProjectile, transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 90f)) as GameObject;
            Destroy(GO, 0.5f);
            Destroy(this.gameObject); // Destroys bullet object
        }
    }
}
