//--------------------------------------------------------------------------------------
// Purpose: Used for the Simple Health Orbs that heal the player on collision.
//
// Description: A child of MonoBehaviour, this script simply checks if it collides with 
//              the player then healths the player by x amount.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    // Amount of Health the Player receives when it collides with this.
    [Tooltip("Amount of Health the Player receives when it collides with this")]
    public int m_nHealthRestored;

    // How close the player needs to get to have the orb approach them.
    [Tooltip("How close the player needs to get to have the orb approach them")]
    public float m_fSeekDistance;

    // The minimum speed of the Health Orb.
    [Tooltip("The minimum speed of the Health Orb.")]
    public float m_fMinSpeed;

    // The maximum speed of the Health Orb.
    [Tooltip("The maximum speed of the Health Orb.")]
    public float m_fMaxSpeed;

    // This number is added to the current speed until it reaches maxSpeed.
    [Tooltip("How much the speed will increase by each frame until it reaches max")]
    public float m_fSpeedIndex;

    //Heal effect to play on pickup
    [Tooltip("Heal effect to play on pickup")]
    public GameObject m_gHealEffect;

    // If the player has been within the heal orbs range.
    private bool m_bSeenPlayer = false;

    // The current speed of the Health Orb.
    private float m_fCurrentSpeed;

    private GameObject m_gPlayer;

    private int m_fPlayerMaxHealth;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public void Awake()
    {
        m_fCurrentSpeed = m_fMinSpeed;
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_fPlayerMaxHealth = m_gPlayer.GetComponent<Player>().GetMaxHealth();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        if (m_gPlayer.GetComponent<Player>().GetCurrentHealth() != m_fPlayerMaxHealth)
        {
            if (Vector2.Distance(this.transform.position, (Vector2)m_gPlayer.transform.position) <= m_fSeekDistance)
            {
                m_bSeenPlayer = true;
            }

            if (m_bSeenPlayer)
            {
                Vector2 DirToTarget = (Vector2)(m_gPlayer.transform.position) - (Vector2)this.transform.position;
                DirToTarget.Normalize();
                this.transform.Translate((DirToTarget) * m_fCurrentSpeed * Time.deltaTime);
                if (m_fCurrentSpeed != m_fMaxSpeed)
                {
                    m_fCurrentSpeed += m_fSpeedIndex;
                }
                else if (m_fCurrentSpeed >= m_fMaxSpeed)
                {
                    m_fCurrentSpeed = m_fMaxSpeed;
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter2D: A function called when the trigger on this object collides with 
    //                   another object.
    //
    // Parameters:
    //      Collider2D collision: The collider that this has collided with.
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the Player walks into this.
        if(collision.tag == "Player")
        {
            if (m_gPlayer.GetComponent<Player>().GetCurrentHealth() != m_fPlayerMaxHealth)
            {
                // Heal the Player by x amount.
                collision.GetComponent<Player>().AddCurrentHealth(m_nHealthRestored);
                // Instantiate Heal effect
                GameObject GO = Instantiate(m_gHealEffect, transform.position, Quaternion.identity) as GameObject;
                //Randomize heal SFX pitch
                GO.GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                //Destroy GO in 1 second
                Destroy(GO, 1f);

                // Destroy this object.
                Destroy(this.gameObject);
            }
        }
    }
}
