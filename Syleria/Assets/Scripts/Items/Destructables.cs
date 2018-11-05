//--------------------------------------------------------------------------------------
// Purpose: Destructable objects in scene logic
//
// Author: Nicholas Weaire
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
            Destroy(this.gameObject); // Destroys this game object
        }
    }

    private void OnDestroy()
    {
        // Animation to play here
        Debug.Log("Destroyed");
    }
}
