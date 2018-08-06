using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {

    private float m_ShieldCD = 3.0f;
    private float m_ShieldDuration = 2.0f;
    private float m_ShieldTimer = 3.0f;



	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_ShieldTimer += Time.deltaTime;
        if(Input.GetAxisRaw("Ability") > 0)
        {
            Debug.Log("SHIELD BOIIIIIII");
        }



	}
}
