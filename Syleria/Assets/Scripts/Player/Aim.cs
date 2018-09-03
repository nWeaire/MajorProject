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
        m_fFireRate = 60.0f / (m_gPlayer.GetComponent<Player>().GetFireRate() * 60.0f); // Updates fire Rate
        Rotate(); // Rotates the retical
        Shoot(); // Shoots based on the retical aim
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
        if (Input.GetButton("Fire2")) // Gets right bumper input
        {

            if (m_fTimeBetweenShots >= m_fFireRate) // If firing is possible
            {
                GameObject newBullet = Instantiate(m_gBullet, this.transform.position, Quaternion.Euler(0, 0, -m_fAngle)) as GameObject; // Instantiate bullet
                newBullet.GetComponent<Bullet>().m_fSpeed = m_fBulletSpeed; // Set bullet speed
                m_fTimeBetweenShots = 0; // Sets time between shots to 0
            }
        }
        else // if unable to shoot, nothing happens
        {

        }

    }

}
