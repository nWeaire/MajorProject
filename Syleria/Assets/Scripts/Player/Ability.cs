using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {

    private float m_ShieldCD = 3.0f;
    public float m_ShieldDuration = 2.0f;
    private float m_ShieldCDTimer = 3.0f;
    private float m_ShieldDurationTimer = 0.0f;

    private bool m_bIsShielded = false;

	// Use this for initialization
	void Start ()
    {
        m_ShieldCDTimer = m_ShieldCD;

    }
	
	// Update is called once per frame
	void Update ()
    {

        if(Input.GetAxisRaw("Ability") > 0 && !m_bIsShielded && m_ShieldCDTimer >= m_ShieldCD)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<CircleCollider2D>().enabled = true;
            m_bIsShielded = true;

        }

        if(m_bIsShielded)
        {
            m_ShieldDurationTimer += Time.deltaTime;
        }
        else if(!m_bIsShielded)
        {
            m_ShieldCDTimer += Time.deltaTime;
        }

        if(m_ShieldDurationTimer >= m_ShieldDuration)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
            m_ShieldDurationTimer = 0;
            m_bIsShielded = false;
            m_ShieldCDTimer = 0;
        }


	}
}
