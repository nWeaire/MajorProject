using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {

    private float m_fTempAngle;

    public Vector2 stickInput;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        stickInput = new Vector2(Input.GetAxisRaw("RightStickX"), Input.GetAxisRaw("RightStickY"));

        if (stickInput.x != 0 || stickInput.y != 0)
        {
            float angle = Mathf.Atan2(stickInput.x, stickInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        }
    }
}
