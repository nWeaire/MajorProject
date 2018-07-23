using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float m_fSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 stickInput = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
        transform.Translate(stickInput * (m_fSpeed * Time.deltaTime));
    }
}
