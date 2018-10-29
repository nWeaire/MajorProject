//--------------------------------------------------------------------------------------
// Purpose: Handles aiming and firing of bullets
//
// Description:  Handles all aiming and firing of bullets and the rotation of the aiming retical
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    #region Bullet Variables
    [SerializeField] private GameObject m_gBullet; // reference to Bullet gameObject
    [SerializeField] private float m_fBulletSpeed = 20; // Bullet speed
    [SerializeField] private float m_fTriSpread = 20;
    #endregion

    #region Retical Variables
    private GameObject m_gPlayer; // Reference to player gameObject
    private float m_fAngle; // Angle of retical aim
    private Vector2 stickInput; // Right stick input
    #endregion

    #region Shooting Variables
    private float m_fFireRate; // Fire Rate for bullets
    private float m_fTimeBetweenShots = 0; // Timer for time between shots
    private Vector3 m_v3Aim; // Aiming for shooting
    #endregion

    #region Animation Variables
    public Animator m_AAnim;
    public float m_fShootAnimTime = 0.1f; // Time which sprite is visable
    private float m_fTimer = 0; // Timer
    private bool m_bShooting = false;
    #endregion

    // Use this for initialization
    void Start ()
    { 
        m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Sets the reference for the player object
        m_fFireRate = 60.0f / (m_gPlayer.GetComponent<Player>().GetFireRate() * 60.0f); // Calculates firerate to be shots per second
        m_fTimeBetweenShots = m_fFireRate; // Sets time betweeen shots so you can fire straight away
    }

    // Update is called once per frame
    void Update ()
    {
        if(GameObject.FindGameObjectWithTag("GM").GetComponent<GM>().isPaused == false)
        {
            m_fFireRate = 60.0f / (m_gPlayer.GetComponent<Player>().GetFireRate() * 60.0f); // Updates fire Rate
            Rotate(); // Rotates the retical
            Shoot(); // Shoots based on the retical aim

            if(m_bShooting)
            {
                
                m_fTimer += Time.deltaTime; // Updates timer 
                m_fTimer = m_fTimer % 60;
                if (m_fTimer >= m_fShootAnimTime) // If timer > sprite length
                {
                    m_bShooting = false;
                    m_AAnim.SetBool("isAttacking", false);
                    m_fTimer = 0.0f;
                }
            }

        }
    }

    //--------------------------------------------------------------------------------------
    // Gets right stick input
    // Rotates retical based on the input
    //--------------------------------------------------------------------------------------
    void Rotate()
    {
        stickInput = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY")); // Gets right stick input

        if (stickInput.x >= 0.2f || stickInput.x <= -0.2f || stickInput.y >= 0.2f || stickInput.y <= -0.2f) // Checks if there is right stick input
        {
            m_fAngle = Mathf.Atan2(stickInput.x, stickInput.y) * Mathf.Rad2Deg; // Gets angle based on direction of the right stick input
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -m_fAngle)); // Rotates retical
        }
    }

    //--------------------------------------------------------------------------------------
    // Sets aim based on the right stick input
    // Fires projectiles based on aim
    //-------------------------------------------------------------------------------------
    void Shoot()
    {
        m_fTimeBetweenShots += Time.deltaTime; // Cooldown for firing
        if (Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0.1f) // Gets right bumper input
        {

            if (m_fTimeBetweenShots >= m_fFireRate) // If firing is possible
            {
                m_AAnim.SetBool("isAttacking", true);
                m_bShooting = true;
                SetAnimation();
                if(m_gPlayer.GetComponent<Player>().m_bTri)
                {
                    GameObject newBullet = Instantiate(m_gBullet, this.transform.position, Quaternion.Euler(0, 0, -m_fAngle)) as GameObject; // Instantiate bullet
                    GameObject newBullet1 = Instantiate(m_gBullet, this.transform.position, Quaternion.Euler(0, 0, -m_fAngle + m_fTriSpread)) as GameObject; // Instantiate bullet
                    GameObject newBullet2 = Instantiate(m_gBullet, this.transform.position, Quaternion.Euler(0, 0, -m_fAngle - m_fTriSpread)) as GameObject; // Instantiate bullet
                    newBullet.GetComponent<Bullet>().m_fSpeed = m_fBulletSpeed; // Set bullet speed
                    newBullet1.GetComponent<Bullet>().m_fSpeed = m_fBulletSpeed; // Set bullet speed
                    newBullet2.GetComponent<Bullet>().m_fSpeed = m_fBulletSpeed; // Set bullet speed
                }
                else
                {
                    GameObject newBullet = Instantiate(m_gBullet, this.transform.position, Quaternion.Euler(0, 0, -m_fAngle)) as GameObject; // Instantiate bullet
                    newBullet.GetComponent<Bullet>().m_fSpeed = m_fBulletSpeed; // Set bullet speed
                }
                GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
                GetComponent<AudioSource>().Play();

                m_fTimeBetweenShots = 0; // Sets time between shots to 0
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // Sets a boolean in the player's animator so it will do a fire animation in the right direction.
    //--------------------------------------------------------------------------------------
    void SetAnimation()
    {
        if (m_fAngle >= 0.0f && m_fAngle < 90.0f)
        {
            //Animate North-East
            m_AAnim.SetInteger("AttackingDirection", 0);
        }
        else if (m_fAngle >= 90.0f && m_fAngle < 180.0f)
        {
            //Animate South-East
            m_AAnim.SetInteger("AttackingDirection", 1);
        }
        else if (m_fAngle < 0.0f && m_fAngle <= -90.0f)
        {
            //Animate South-West
            m_AAnim.SetInteger("AttackingDirection", 2);
        }
        else if (m_fAngle < 0.0f && m_fAngle > -90.0f)
        {
            //Animate North-West
            m_AAnim.SetInteger("AttackingDirection", 3);
        }

    }

}
