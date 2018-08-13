using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    enum MoveDirection { UP, DOWN, LEFT, RIGHT };

    [SerializeField] private float m_fDashDistance = 3;
    [SerializeField] private ContactFilter2D m_cfFilter;
    [SerializeField] private float m_fDashSpeed = 8;
    [SerializeField] private float m_fDashCD = 0.5f;
    [SerializeField] private Sprite m_sprFront;
    [SerializeField] private Sprite m_sprBack;
    [SerializeField] private Sprite m_sprSide;
    [SerializeField] private GameObject m_gSprite;

    public Vector2 m_v2rightPoint;
    public Vector2 m_v2leftPoint;

    private GameObject m_gPlayer;


    private bool m_bIsDashing = false;
    private float m_fDashTimer = 0;

    public float m_fDashCDTimer = 0;
    private bool m_bDash = true;

    private Vector2 m_v2StickInput;
    private Vector2 m_v2DashInput;
    private Vector2 m_v2EndDashPos;
    private Vector2 m_v2StartDashPos;
    private MoveDirection tempDir;
    private float m_fRadius;

    public float m_fSpeed;
    private MoveDirection dir = MoveDirection.LEFT;

    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_fRadius = GetComponent<CircleCollider2D>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        m_fSpeed = m_gPlayer.GetComponent<Player>().GetMoveSpeed();
        m_v2StickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        tempDir = dir;
        CheckCollision();
        Direction();
        Dash();
        if (dir != tempDir)
        {
            UpdateModel();
        }

    }

    void Direction()
    {
        if (m_v2StickInput.x > 0.2f && m_v2StickInput.y > -0.5f && m_v2StickInput.y < 0.5f)
        {
            dir = MoveDirection.RIGHT;
        }
        else if (m_v2StickInput.x < -0.2f && m_v2StickInput.y > -0.5f && m_v2StickInput.y < 0.5f)
        {
            dir = MoveDirection.LEFT;
        }
        else if (m_v2StickInput.y > 0.2f && m_v2StickInput.x > -0.5f && m_v2StickInput.x < 0.5f)
        {
            dir = MoveDirection.UP;
        }
        else if (m_v2StickInput.y < -0.2f && m_v2StickInput.x > -0.5f && m_v2StickInput.x < 0.5f)
        {
            dir = MoveDirection.DOWN;
        }

    }

    void Dash()
    {
        m_v2DashInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Gets input of the left analog stick
        m_v2DashInput.Normalize();
        if (Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x > 0.2f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x < -0.2f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y > 0.2f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y < -0.2f)
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
            (Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x > -0.2f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.x < 0.2f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y > -0.2f ||
            Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash && m_v2DashInput.y < 0.2f)
        {
            if (dir == MoveDirection.UP)
            {
                m_bIsDashing = true;
                int count = 0;
                RaycastHit2D[] Hit = new RaycastHit2D[1];
                count = Physics2D.Raycast(this.transform.position, Vector2.up, m_cfFilter, Hit, m_fDashDistance);
                if (count > 0)
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.up.x * 0.5f);
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.up.y * 0.5f);
                }
                else
                {
                    m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.up * m_fDashDistance);
                }
                m_v2StartDashPos = this.transform.position;
            }
            else if (dir == MoveDirection.DOWN)
            {
                m_bIsDashing = true;
                int count = 0;
                RaycastHit2D[] Hit = new RaycastHit2D[1];
                count = Physics2D.Raycast(this.transform.position, Vector2.down, m_cfFilter, Hit, m_fDashDistance);
                if (count > 0)
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.down.x * 0.5f);
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.down.y * 0.5f);
                }
                else
                {
                    m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.down * m_fDashDistance);
                }
                m_v2StartDashPos = this.transform.position;
            }
            else if (dir == MoveDirection.RIGHT)
            {
                m_bIsDashing = true;
                int count = 0;
                RaycastHit2D[] Hit = new RaycastHit2D[1];
                count = Physics2D.Raycast(this.transform.position, Vector2.right, m_cfFilter, Hit, m_fDashDistance);              
                if (count > 0)
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.right.x * 0.5f);
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.right.y * 0.5f);
                }
                else
                {
                    m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.right * m_fDashDistance);
                }
                m_v2StartDashPos = this.transform.position;
            }
            else if (dir == MoveDirection.LEFT)
            {
                m_bIsDashing = true;
                int count = 0;
                RaycastHit2D[] Hit = new RaycastHit2D[1];
                count = Physics2D.Raycast(this.transform.position, Vector2.left, m_cfFilter, Hit, m_fDashDistance);
                if (count > 0)
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.left.x * 0.5f);
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.left.y * 0.5f);
                }
                else
                {
                    m_v2EndDashPos = (Vector2)this.transform.position + (Vector2.left * m_fDashDistance);
                }
                m_v2StartDashPos = this.transform.position;
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

    public void UpdateModel()
    {
        switch (dir)
        {
            case MoveDirection.UP:
                m_gSprite.GetComponent<SpriteRenderer>().sprite = m_sprBack;
                break;
            case MoveDirection.DOWN:
                m_gSprite.GetComponent<SpriteRenderer>().sprite = m_sprFront;
                break;
            case MoveDirection.LEFT:
                m_gSprite.GetComponent<SpriteRenderer>().sprite = m_sprSide;
                m_gSprite.GetComponent<SpriteRenderer>().flipX = true;
                break;
            case MoveDirection.RIGHT:
                m_gSprite.GetComponent<SpriteRenderer>().sprite = m_sprSide;
                m_gSprite.GetComponent<SpriteRenderer>().flipX = false;
                break;
            default:
                break;
        }
    }

    void Rotate()
    {
            m_v2StickInput.Normalize();
            m_v2leftPoint = this.transform.position;
            m_v2leftPoint += GetComponent<CircleCollider2D>().offset;
            m_v2leftPoint += new Vector2(-m_v2StickInput.y, m_v2StickInput.x) / Mathf.Sqrt((m_v2StickInput.x * m_v2StickInput.x) + (m_v2StickInput.y * m_v2StickInput.y)) * m_fRadius;
            m_v2rightPoint = this.transform.position;
            m_v2rightPoint += GetComponent<CircleCollider2D>().offset;
            m_v2rightPoint -= new Vector2(-m_v2StickInput.y, m_v2StickInput.x) / Mathf.Sqrt((m_v2StickInput.x * m_v2StickInput.x) + (m_v2StickInput.y * m_v2StickInput.y)) * m_fRadius;
    }
    public void CheckCollision()
    {
    if (m_v2StickInput.x >= 0.2f || m_v2StickInput.x <= -0.2f || m_v2StickInput.y >= 0.2f || m_v2StickInput.y <= -0.2f)
    {
        Rotate();
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(GetComponent<CircleCollider2D>().offset.x, GetComponent<CircleCollider2D>().offset.y);
        RaycastHit2D[] Hit = new RaycastHit2D[1];
        int mCount = 0;
        int lCount = 0;
        int rCount = 0;
        m_v2StickInput.Normalize();
        mCount = Physics2D.Raycast(rayOrigin, m_v2StickInput, m_cfFilter, Hit, m_fRadius + 0.1f);
        lCount += Physics2D.Raycast(m_v2leftPoint, m_v2StickInput, m_cfFilter, Hit, m_fRadius + 0.1f);
        rCount += Physics2D.Raycast(m_v2rightPoint, m_v2StickInput, m_cfFilter, Hit, m_fRadius + 0.1f);
        Debug.DrawRay(m_v2leftPoint, m_v2StickInput, Color.cyan);
        Debug.DrawRay(m_v2rightPoint, m_v2StickInput, Color.cyan);
        Debug.DrawRay(rayOrigin, m_v2StickInput, Color.cyan);

            if (mCount > 0 && rCount <= 0 && lCount <= 0)
            {
                Debug.Log("Just Middle Hit");
            }
            else if (lCount > 0 && rCount <= 0 && mCount <= 0)
            {
                Debug.Log("Just Left Hit");
                if (Hit[0].normal.x > 0)
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.x < 0)
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed));
                }
                else if(Hit[0].normal.y > 0)
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed));
                }
                else
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed));
                }
            }
            else if(rCount > 0 && lCount <= 0 && mCount <= 0)
            {
                Debug.Log("Just Right Hit");
                if (Hit[0].normal.x > 0)
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.x < 0)
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.y > 0)
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed));
                }
                else
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed));
                }
            }
            else if(rCount > 0 && lCount <= 0 && mCount > 0)
            {
                Debug.Log("Middle Right Hit");
                if (Hit[0].normal.x > 0)
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.x < 0)
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.y > 0)
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed));
                }
                else
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed));
                }
            }
            else if (rCount <= 0 && lCount > 0 && mCount > 0)
            {
                Debug.Log("Middle Left Hit");
                if (Hit[0].normal.x > 0)
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.x < 0)
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed));
                }
                else if (Hit[0].normal.y > 0)
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed));
                }
                else
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed));
                }
            }
            else if(mCount > 0 && lCount > 0 && rCount > 0)
            {
                Debug.Log("Stop");
            }
            else if(rCount > 0 && lCount > 0 && mCount <= 0)
            {
                Debug.Log("Left and Right Hit");
            }
            else
            {
                transform.Translate(m_v2StickInput * (Time.deltaTime * m_fSpeed));
            }

        }
    }
}
