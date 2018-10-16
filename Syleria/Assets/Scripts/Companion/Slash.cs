//--------------------------------------------------------------------------------------
// Purpose: On the Slash object.
//
// Description:  Handles all logic based around the Slash.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    // Pointer to Player.
    public GameObject m_gPlayer;
    // Pointer to the direction the slash should start at.
    public GameObject m_gAim;

    // Time until the Slash is deleted
    public float m_fSlashTime = 0.1f;

    // Speed that the Slash will move at
    public float m_fSlashSpeed = 400f;

    public GameObject m_gSlash;

    private float m_fSlashTimer = 0;

    private Animator m_Animator;

    private bool m_bSpawnedSlash = false;

    private GameObject gSlash;

    // Use this for initialization
    void Awake()
    {
        m_fSlashTimer = 0; // Timer set to 0
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_bSpawnedSlash)
        {
            //gSlash = Instantiate(m_gSlash, m_gPlayer.transform);
            //gSlash.transform.position = m_gAim.transform.position;
            //m_bSpawnedSlash = true;
        }
        //gSlash.transform.RotateAround(m_gPlayer.transform.position, new Vector3(0, 0, -1), m_fSlashSpeed * Time.deltaTime);
        //Vector3 desiredPosition = (gSlash.transform.position - m_gPlayer.transform.position).normalized * -3 + m_gPlayer.transform.position;
        //gSlash.transform.position = Vector3.MoveTowards(gSlash.transform.position, desiredPosition, Time.deltaTime * m_fSlashSpeed);
        m_fSlashTimer += Time.deltaTime; // Updates timer 
        m_fSlashTimer = m_fSlashTimer % 60;
        if (m_fSlashTimer >= m_fSlashTime) // If timer > sprite length
        {
            m_bSpawnedSlash = false;
            //GameObject.Destroy(gSlash);
            this.gameObject.SetActive(false); // Set this to false
            m_fSlashTimer = 0.0f;
        }
    }
}

