using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : MonoBehaviour {

    public float m_fTauntSpriteLength = 0.1f;
    private float m_fTimer = 0;

	// Use this for initialization
	void Awake ()
    {
        m_fTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_fTimer += Time.deltaTime;
        if(m_fTimer >= m_fTauntSpriteLength)
        {
            this.gameObject.SetActive(false);
        }
	}
}
