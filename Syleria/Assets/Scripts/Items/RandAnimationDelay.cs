using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandAnimationDelay : MonoBehaviour
{
    public float m_fMinDelay = 0.1f;

    public float m_fMaxDelay = 2;

    private Animator m_Animator;

    private float m_fDelay;

    private float m_fDelayTimer;

    private bool m_bAnimStarted;

	// Use this for initialization
	void Start()
    {
        m_fDelay = Random.Range(m_fMinDelay, m_fMaxDelay);
        m_Animator = GetComponent<Animator>();
        m_Animator.enabled = false;
        m_bAnimStarted = false;
    }
	
	// Update is called once per frame
	void Update()
    {
       if(m_fDelayTimer > m_fDelay & !m_bAnimStarted)
        {
            m_Animator.enabled = true;
            m_bAnimStarted = true;
        }
       else
        {
            m_fDelayTimer += Time.deltaTime;
            m_fDelayTimer = m_fDelayTimer % 60;
        }
	}
}
