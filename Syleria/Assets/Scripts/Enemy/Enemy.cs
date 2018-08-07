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

    // Lets the enemy know where the player is
    [HideInInspector]
    public GameObject m_gPlayer;

    // Boolean for leftmost raycast
    private bool m_bLeftHit;

    // Boolean for rightmost raycast
    private bool m_bRightHit;


    public void AvoidObstacles()
    {

        RaycastHit2D[] aHit = new RaycastHit2D[1];

        int count = 0;
        count = Physics2D.Raycast(transform.localPosition + (-transform.right * 0.64f), transform.up, m_cfFilter, aHit, 2);
        Debug.DrawRay(transform.localPosition + (-transform.right * 0.64f), transform.up, Color.cyan);
        if (count > 0)
        {
            m_bLeftHit = true;
        }
        else
        {
            m_bLeftHit = false;
        }

        count = Physics2D.Raycast(transform.localPosition + (transform.right * 0.64f), transform.up, m_cfFilter, aHit, 2);
        Debug.DrawRay(transform.localPosition + (transform.right * 0.64f), transform.up, Color.cyan);
        if (count > 0)
        {
            m_bRightHit = true;
        }
        else
        {
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

        // If the left feeler has been hit
        if (m_bLeftHit)
        {
            // Rotate towards the right feeler
            transform.Rotate(Vector3.back * (180f * Time.deltaTime));

        }
        // If the right feeler has been hit
        if (m_bRightHit)
        {
            // Rotate towards the left feeler
            transform.Rotate(Vector3.forward * (180f * Time.deltaTime));
        }
    }

    // Seek Towards the Player
    public void Seek(Vector3 v3Target)
    {
        // Find the target that will allow us to get to the target
        Vector3 v3TargetDir = v3Target - transform.position;
        v3TargetDir = v3TargetDir.normalized;

        Debug.DrawRay(this.transform.position, v3TargetDir, Color.red);

        float dot = Vector3.Dot(transform.up.normalized, v3TargetDir);
        float rightDot = Vector3.Dot(transform.right.normalized, v3TargetDir);

        if (dot > 0.9f)
        {
            // Don't rotate if facing target
        }
        else if (rightDot > 0) // rotate right as D is on the right.
        {
            transform.Rotate(Vector3.back * (180f * Time.deltaTime));
        }
        else if (rightDot < 0) // rotate left as D is on the Left
        {
            transform.Rotate(Vector3.forward * (180f * Time.deltaTime));

        }
        
        Arrive();
    }

    public void Arrive()
    {
        Vector3 v3Direction = m_gPlayer.transform.position - transform.position;

        float distance = v3Direction.magnitude;


        float fDeceleratoin = distance / 5f;

        if (distance < m_fSlowingRadius)
        {
            m_fSpeed = Mathf.Clamp(m_fSpeed * fDeceleratoin, 0f, m_fMaxSpeed);
        }
        else
        {
            m_fSpeed = m_fMaxSpeed;
        }
    }

    public void TakeDamage(int nDamage)
    {
        m_nCurrentHealth -= nDamage;
    }
}
