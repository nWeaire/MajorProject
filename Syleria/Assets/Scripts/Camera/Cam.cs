using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    private GameObject m_oPlayer;

    [SerializeField] private float m_fSpeed;

    //[SerializeField] private float m_fVertOffSet = 10.0f;

	// Use this for initialization
	void Start ()
    {
        m_oPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(m_oPlayer.transform.position.x, m_oPlayer.transform.position.y, this.transform.position.z), m_fSpeed);
    }
}
