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
            // Heal the Player by x amount.
            collision.GetComponent<Player>().AddCurrentHealth(m_nHealthRestored);
            // Destroy this object.
            Destroy(this.gameObject);
        }
    }
}
