using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    enum MoveDirection{UP, DOWN, LEFT, RIGHT };


    [SerializeField]
    private float m_fSpeed = 5;

    private Vector2 m_v2StickInput;
    private Vector2 m_v2DashInput;

    [SerializeField]
    private float m_fDashDistance = 3;

    [SerializeField]
    private ContactFilter2D m_cfFilter;

    [SerializeField]
    private float m_fDashSpeed = 8;

    private bool m_bIsDashing = false;

    private float m_fDashTimer = 0;

    public float m_fDashCDTimer = 0;
    private bool m_bDash = true;

    [SerializeField]
    private float m_fDashCD = 0.5f;

    private Vector2 m_v2EndDashPos;
    private Vector2 m_v2StartDashPos;

    private MoveDirection dir = MoveDirection.LEFT;

    RaycastHit2D[] m_rHit = new RaycastHit2D[1];
    private int m_nCount = 0;
    void Start ()
    {
    }

	// Update is called once per frame
	void Update ()
    {
        Move();
        Direction();
        Dash();
    }

    void Direction()
    {
        if(m_v2StickInput.x > 0.1f && m_v2StickInput.y > -0.5f && m_v2StickInput.y < 0.5f)
        {
            dir = MoveDirection.RIGHT;
        }
        else if(m_v2StickInput.x < -0.1f && m_v2StickInput.y > -0.5f && m_v2StickInput.y < 0.5f)
        {
            dir = MoveDirection.LEFT;
        }
        else if(m_v2StickInput.y > 0.1f && m_v2StickInput.x > -0.5f && m_v2StickInput.x < 0.5f)
        {
            dir = MoveDirection.UP;
        }
        else if (m_v2StickInput.y < -0.1f && m_v2StickInput.x > -0.5f && m_v2StickInput.x < 0.5f)
        {
            dir = MoveDirection.DOWN;
        }
        else
        {
            Debug.Log(dir);
        }
    }
    void Move()
    {
        m_v2StickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Gets input of the left analog stick
        if (m_v2StickInput.x >= 0.1f || m_v2StickInput.x <= -0.1f || m_v2StickInput.y >= 0.1f || m_v2StickInput.y <= -0.1f) // Check if left analog stick is being used
        {
                //transform.Translate(m_v2StickInput * (Time.deltaTime * m_fSpeed));
            GetComponent<Rigidbody2D>().MovePosition((Vector2)this.transform.position + (m_v2StickInput.normalized * (Time.deltaTime * m_fSpeed)));
        }
    }

    void Dash()
    {
        m_v2DashInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); // Gets input of the left analog stick
        m_v2DashInput.Normalize();
        if (Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x > 0.1f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x < -0.1f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y > 0.1f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y < -0.1f)
        {
            m_bIsDashing = true;
            int count = 0;
            RaycastHit2D[] Hit = new RaycastHit2D[1];
            count = Physics2D.Raycast(this.transform.position, m_v2DashInput, m_cfFilter, Hit, m_fDashDistance);
            if (count > 0)
            {
                m_v2EndDashPos.x = Hit[0].point.x - (m_v2DashInput.x * 0.5f);
                m_v2EndDashPos.y = Hit[0].point.y - (m_v2DashInput.y * 0.5f);
            }
            else
            {
              m_v2EndDashPos = (Vector2)this.transform.position + (m_v2DashInput * m_fDashDistance);
            }
            m_v2StartDashPos = this.transform.position;
        }
        else if
           (Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x < 0.1f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x > -0.1f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y < 0.1f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y > -0.1f)
        {
            switch (dir)
            {
                case MoveDirection.UP:
                    m_bIsDashing = true;
                    m_nCount = 0;
                    m_nCount = Physics2D.Raycast(this.transform.position, Vector2.up , m_cfFilter, m_rHit, m_fDashDistance);
                    if (m_nCount > 0)
                    {
                        m_v2EndDashPos.x = m_rHit[0].point.x - (Vector2.up.x * 0.5f);
                        m_v2EndDashPos.y = m_rHit[0].point.y - (Vector2.up.y * 0.5f);
                    }
                    else
                    {
                        m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.up * m_fDashDistance);
                    }
                    m_v2StartDashPos = this.transform.position;
                    break;
                case MoveDirection.DOWN:
                    m_bIsDashing = true;
                    m_nCount = 0;
                    m_nCount = Physics2D.Raycast(this.transform.position, Vector2.down, m_cfFilter, m_rHit, m_fDashDistance);
                    if (m_nCount > 0)
                    {
                        m_v2EndDashPos.x = m_rHit[0].point.x - (Vector2.down.x * 0.5f);
                        m_v2EndDashPos.y = m_rHit[0].point.y - (Vector2.down.y * 0.5f);
                    }
                    else
                    {
                        m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.down * m_fDashDistance);
                    }
                    m_v2StartDashPos = this.transform.position;
                    break;
                case MoveDirection.LEFT:
                    m_bIsDashing = true;
                    m_nCount = 0;
                    m_nCount = Physics2D.Raycast(this.transform.position, Vector2.left, m_cfFilter, m_rHit, m_fDashDistance);
                    if (m_nCount > 0)
                    {
                        m_v2EndDashPos.x = m_rHit[0].point.x - (Vector2.left.x * 0.5f);
                        m_v2EndDashPos.y = m_rHit[0].point.y - (Vector2.left.y * 0.5f);
                    }
                    else
                    {
                        m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.left * m_fDashDistance);
                    }
                    m_v2StartDashPos = this.transform.position;
                    break;
                case MoveDirection.RIGHT:
                    m_bIsDashing = true;
                    m_nCount = 0;
                    m_nCount = Physics2D.Raycast(this.transform.position, Vector2.right, m_cfFilter, m_rHit, m_fDashDistance);
                    if (m_nCount > 0)
                    {
                        m_v2EndDashPos.x = m_rHit[0].point.x - (Vector2.right.x * 0.5f);
                        m_v2EndDashPos.y = m_rHit[0].point.y - (Vector2.right.y * 0.5f);
                    }
                    else
                    {
                        m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.right * m_fDashDistance);
                    }
                    m_v2StartDashPos = this.transform.position;
                    break;
                default:
                    break;
            }
        }
        if (m_bIsDashing)
        {
            m_fDashTimer += Time.deltaTime * m_fDashSpeed;
            transform.position = Vector2.Lerp(m_v2StartDashPos, m_v2EndDashPos, m_fDashTimer);

            if (Vector2.Distance(this.transform.position, m_v2EndDashPos) <= 0.05)
            {
                m_bIsDashing = false;
                m_fDashTimer = 0;
                m_bDash = false;
                m_fDashCDTimer = 0;
            }
        }
        if (!m_bDash)
        {
            m_fDashCDTimer += Time.deltaTime;
            if (m_fDashCDTimer >= m_fDashCD)
            {
                m_bDash = true;
            }
        }
    }
}
