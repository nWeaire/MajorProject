using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {

    private GameObject m_oPlayer;
    private Vector2 m_v2Target;
    [SerializeField] private float m_nCamDistance;
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
        m_v2Target = new Vector2(Input.GetAxis("RightStickX"),Input.GetAxis("RightStickY"));
        Vector2 tempTarget = m_v2Target * m_nCamDistance;
        transform.position = Vector3.Lerp(transform.position, new Vector3(m_oPlayer.transform.position.x + tempTarget.x, m_oPlayer.transform.position.y + tempTarget.y, this.transform.position.z), m_fSpeed);
    }
}
