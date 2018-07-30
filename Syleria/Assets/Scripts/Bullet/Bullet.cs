using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField]
    private float m_fLifeSpan = 0.5f;
    private float m_fTimer = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        m_fTimer += Time.deltaTime;
        if (m_fLifeSpan <= m_fTimer)
            DestroyObject(this.gameObject);
	}


}
