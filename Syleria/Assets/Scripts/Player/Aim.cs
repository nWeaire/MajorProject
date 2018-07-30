using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField]
    private GameObject m_gBullet;

    [SerializeField]
    private float m_fBulletSpeed = 10;

    [SerializeField]
    private Vector3 m_v3Aim;

    [SerializeField]
    private float m_fFireRate = 0.2f;

    private float m_fTimeBetweenShots = 0;

    public Vector2 stickInput;

	// Use this for initialization
	void Start ()
    {
        m_fTimeBetweenShots = m_fFireRate;

    }

    // Update is called once per frame
    void Update ()
    {
        stickInput = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));

        if (stickInput.x >= 0.2f || stickInput.x <= -0.2f || stickInput.y >= 0.2f || stickInput.y <= -0.2f)
        {
            float angle = Mathf.Atan2(stickInput.x, stickInput.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));
        }
        Shoot();
    }

    void Shoot()
    {
        m_fTimeBetweenShots += Time.deltaTime;
        Vector2 stickInput = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
        if (Input.GetButton("Fire2"))
        {
            m_v3Aim.x = stickInput.x;
            m_v3Aim.y = stickInput.y;
            m_v3Aim.z = 0;
            m_v3Aim.Normalize();

            Debug.DrawRay(this.transform.position, this.transform.up);
            if (m_fTimeBetweenShots >= m_fFireRate)
            {
                GameObject newBullet = Instantiate(m_gBullet, this.transform.position + this.transform.up, Quaternion.Euler(m_v3Aim)) as GameObject;
                newBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * m_fBulletSpeed);
                m_fTimeBetweenShots = 0;
            }
        }
        else
        {

        }

    }
}
