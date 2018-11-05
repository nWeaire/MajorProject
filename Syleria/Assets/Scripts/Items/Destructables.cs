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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        { }
        else
        {
            this.GetComponentInChildren<ParticleSystem>().Play();
            this.GetComponent<AudioSource>().pitch = Random.Range(0.85f, 1.15f);
            this.GetComponent<AudioSource>().Play();
            this.gameObject.SetActive(false); // Destroys this game object
        }
    }

    private void OnDestroy()
    {
        // Animation to play here
        Debug.Log("Destroyed");
    }
}
