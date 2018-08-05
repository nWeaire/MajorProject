using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Enemy
{
    // Projectile for the Sentry
    public GameObject m_gProjectile;

	// Use this for initialization
	void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 v3TargetDir = m_gPlayer.transform.position - transform.position;
        v3TargetDir = v3TargetDir.normalized;
        float angle = Mathf.Atan2(v3TargetDir.x, v3TargetDir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, -angle);
    }
}
