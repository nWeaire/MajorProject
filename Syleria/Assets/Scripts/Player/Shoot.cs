using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    [SerializeField]
    private GameObject m_gBullet;

    [SerializeField]
    private float m_fBulletSpeed = 10;

    [SerializeField]
    private Vector3 m_v3Aim;

    [SerializeField]
    private float m_fFireRate = 0.2f;
    private float m_fTimeBetweenShots = 0;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 stickInput = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
        if (stickInput.x != 0 && Input.GetButton("Fire2")  || stickInput.y != 0 && Input.GetButton("Fire2"))
        {
            m_v3Aim.x = stickInput.x;
            m_v3Aim.y = stickInput.y;
            m_v3Aim.z = 0;
            m_v3Aim.Normalize();

            m_fTimeBetweenShots += Time.deltaTime;

            Debug.DrawRay(this.transform.position, m_v3Aim);
            if (m_fTimeBetweenShots >= m_fFireRate)
            {
                GameObject newBullet = Instantiate(m_gBullet, this.transform.position + m_v3Aim, Quaternion.Euler(m_v3Aim)) as GameObject;
                newBullet.GetComponent<Rigidbody2D>().AddForce(m_v3Aim * m_fBulletSpeed);
                m_fTimeBetweenShots = 0;
            }
        }

    }
}
