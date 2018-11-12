//--------------------------------------------------------------------------------------
// Purpose: Destructable objects in scene logic
//
// Author: Nicholas Weaire
//
//Contributors: Hamish Smithers
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour
{

    public GameObject m_gHealthPrefab; // Health prefab
    public float m_fHealthDropChance; // Float for drop chance of health orbs

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy") // If colliding with an enemy
        { }
        else
        {
            if (Random.Range(0f, 100f) < m_fHealthDropChance && m_gHealthPrefab != null) // If a random number between 0-100 is less then health drop chance and health prefab exists
            {
                Instantiate(m_gHealthPrefab, this.transform.position, Quaternion.identity); // Instaniates health orb at this position
            }
            this.GetComponent<ParticleSystem>().Play(); // Play particle effect
            this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.15f); // Sets pitch for audio
            this.GetComponent<AudioSource>().Play(); // Plays audio
            this.gameObject.SetActive(false); // Destroys this game object
        }
    }
}
