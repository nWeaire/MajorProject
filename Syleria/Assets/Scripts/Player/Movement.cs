using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [SerializeField]
    private float m_fSpeed = 5;

    [SerializeField]
    private Vector2 m_v2Direction;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();

        Dash();
    }
    void Move()
    {
        Vector2 stickInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(stickInput * (Time.deltaTime * m_fSpeed));
    }
    void Dash()
    {

    }
}
