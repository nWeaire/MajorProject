using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    private GameObject m_oPlayer;

    [SerializeField] private float m_fVertOffSet = 10.0f;

	// Use this for initialization
	void Start ()
    {
        m_oPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position = m_oPlayer.transform.position + new Vector3(0, 0, -m_fVertOffSet);
	}
}
