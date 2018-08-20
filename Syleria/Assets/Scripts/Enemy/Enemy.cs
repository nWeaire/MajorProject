//--------------------------------------------------------------------------------------
// Purpose: Used as the parent for all Enemy scripts.
//
// Description: Contains functions and variables that most if not all
//              enemies will use at some point.
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Maximum health for the Enemy
    [Tooltip("Maximum health for this Enemy")]
    public int m_nHealth;

    // Damage that the enemy will do
    [Tooltip("Damage that this enemy will do to the player")]
    public int m_nDamage;

    // The CurrentHealth of the Enemy
    [Tooltip("The Current Health of this enemy")]
    public int m_nCurrentHealth;

    // Maximum speed that the enemy can go
    [Tooltip("This is the maximum speed of the enemy, and their natural speed unless they are slowing down")]
    public float m_fMaxSpeed;

    // Current speed of the enemy
    [Tooltip("How fast the enemy is going currently")]
    public float m_fSpeed;

    // Filter for what the Enemy must avoid
    [Tooltip("This is the filter for the enemy to avoid (e.g the terrain layer)")]
    public ContactFilter2D m_cfFilter;

    //If the enemy gets within this distance of the player it will slow
    [Tooltip("If the enemy gets within this distance of the player it will slow")]
    public float m_fSlowingRadius;

    public float m_fKnockBack;

    // Lets the enemy know where the player is
    [HideInInspector]
    public GameObject m_gPlayer;

    // Boolean for leftmost raycast
    private bool m_bLeftHit;

    // Boolean for rightmost raycast
    private bool m_bRightHit;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public void Start()
    {
        m_bLeftHit = false;
        m_bRightHit = false;
    }


    //--------------------------------------------------------------------------------------
    // AvoidObstacles: 
    //
    // Parameters:
    //      v3Target: The Target that the Enemy will attempt to rotate towards
    //--------------------------------------------------------------------------------------
    public void AvoidObstacles()
    {
        // Initialise an array of RayCast hits.
        RaycastHit2D[] aHit = new RaycastHit2D[2];

        // Initialise the counter for raycasts
        int count = 0;
        // Count will be equal to the amount of objects hit in the Raycast.
        count = Physics2D.Raycast(transform.localPosition, transform.up - transform.right, m_cfFilter, aHit, 2f);
        // Draw the ray in Scene view.
        Debug.DrawRay(transform.localPosition, transform.up - transform.right, Color.cyan);
        // If the Raycast has hit something,
        if (count > 0)
        {
            // Then leftHit is true.
            m_bLeftHit = true;
        }
        // If the Raycast hasn't hit something,
        else
        {
            // Then leftHit is false.
            m_bLeftHit = false;
        }
        // Count will be equal to the amount of objects hit in the Raycast.
        count = Physics2D.Raycast(transform.localPosition, transform.up + transform.right, m_cfFilter, aHit, 2f);
        // Draw the ray in Scene view.
        Debug.DrawRay(transform.localPosition, transform.up + transform.right, Color.cyan);

        // If the Raycast has hit something,
        if (count > 0)
        {
            // Then rightHit is true.
            m_bRightHit = true;
        }
        // If the Raycast hasn't hit something,
        else
        {
            // Then rightHit is false.
            m_bRightHit = false;
        }

        // Display the ray casts in the scene view
        foreach (RaycastHit2D element in aHit)
        {
            if (element.point != new Vector2(0, 0))
            {
                Debug.DrawLine(transform.position, element.point, Color.red);
            }
        }     

        // If the left ray has been hit
        if (m_bLeftHit)
        {
            // Rotate towards the right, with speed dependent on how close it is to the obstacle. 
            if (aHit[0].distance > 0.0f)
            {
                transform.Rotate(Vector3.back * (180f * (2f / aHit[0].distance) * Time.deltaTime));
            }
            else
            {
                transform.Rotate(Vector3.back * (180f * (2f) * Time.deltaTime));
            }

        }
        // If the right ray has been hit
        else if (m_bRightHit)
        {
            // Rotate towards the left, with speed dependent on how close it is to the obstacle. 
            if (aHit[0].distance > 0.0f)
            {
                transform.Rotate(Vector3.forward * (180f * (2f / aHit[0].distance) * Time.deltaTime));
            }
            else
            {
                transform.Rotate(Vector3.forward * (180f * (2f) * Time.deltaTime));
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // Seek: Rotate towards the target over time.
    //
    // Parameters:
    //      v3Target: The Target that the Enemy will attempt to rotate towards.
    //--------------------------------------------------------------------------------------
    public void Seek(Vector3 v3Target)
    {
        // Find the target that will allow us to get to the target.
        Vector3 v3TargetDir = v3Target - transform.position;
        // Normalize the Direction vector.
        v3TargetDir.Normalize();

        Debug.DrawRay(this.transform.position, v3TargetDir, Color.red);

        float dot = Vector3.Dot(transform.up.normalized, v3TargetDir);
        float rightDot = Vector3.Dot(transform.right.normalized, v3TargetDir);



        if (dot > 0.9f) // if Target is in the direction this is facing,
        {
            // Don't rotate.
        }
       if (rightDot > 0f) // rotate right as D is on the right.
        {
            transform.Rotate(Vector3.back * (180f * Time.deltaTime));
        }
       if (rightDot < 0f) // rotate left as D is on the Left.
        {
            transform.Rotate(Vector3.forward * (180f * Time.deltaTime));

        }
        // Call arrive script, as it will do nothing unless it is close enough.
        Arrive();
    }

    //--------------------------------------------------------------------------------------
    // Arrive: When the Enemy is close to the Player, they should slow slightly for steering.
    //--------------------------------------------------------------------------------------
    public void Arrive()
    {
        Vector3 v3DesiredVelocity = m_gPlayer.transform.position - transform.position;

        float fDistance = v3DesiredVelocity.magnitude;

        if (fDistance < m_fSlowingRadius)
        {
            m_fSpeed = m_fMaxSpeed * (fDistance / m_fSlowingRadius);
            Mathf.Clamp(m_fSpeed, m_fMaxSpeed * 0.5f, m_fMaxSpeed);
        }
        else
        {
            m_fSpeed = m_fMaxSpeed;
        }
    }
    //--------------------------------------------------------------------------------------
    // TakeDamage: Calling this on any enemy will damage it.
    //
    // Parameters:
    //      int nDamage: The Enemy will have their current health decreased by this value.
    //--------------------------------------------------------------------------------------
    public void TakeDamage(int nDamage)
    {
        // Current health = currentHealth - damage.
        m_nCurrentHealth -= nDamage;
    }
}
