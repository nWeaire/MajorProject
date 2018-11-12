//--------------------------------------------------------------------------------------
// Purpose: Handles collision and movement of the player
//
// Description:  Handles all collision, movement and sprite changes for player
//
// Author: Nicholas Weaire
//
// Contributor: Callan Davies - Animations and Particle Systems. 
//--------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Movement : MonoBehaviour
{
    enum MoveDirection { UP, DOWN, LEFT, RIGHT }; // Enum for direction the player is moving
    public Material m_mUpDash;
    public Material m_mDownDash;
    public Material m_mLeftDash;
    public Material m_mRightDash;

    [SerializeField] private ContactFilter2D m_cfFilter; // Contact filter for collision detection
    private GameObject m_gPlayer; // Reference to player 

    #region Sprites
    [SerializeField] public GameObject m_gSprite; // Reference to player sprite
    public GameObject m_gDashIcon;
    public Animator m_Animator;
    private ParticleSystem m_ParticleSystem;
    private Renderer m_Renderer;
    #endregion

    #region Dash Variables
    private bool m_bIsDashing = false; // Is player dashing
    private float m_fDashTimer = 0; // Timer for dash lerp 
    public float m_fDashCDTimer = 0; // Timer for dash cooldown
    private bool m_bDash = true; // Is dash off cooldown
    private Vector2 m_v2DashInput; // Input for dash
    private Vector2 m_v2EndDashPos; // Position dash will end in 
    private Vector2 m_v2StartDashPos; // Position dash starts in
    [SerializeField] private float m_fDashDistance = 3; // Total dash distance
    [SerializeField] private float m_fDashSpeed = 8; // Speed of dash
    [SerializeField] private float m_fDashCD = 0.5f; // Cooldown of dash
    [SerializeField] private float m_fImmunityTime = 1f;
    private float m_fITimer = 0;
    private bool m_bImmunity = false;
    #endregion

    #region Movement Variables
    private float m_fRadius; // Radius of movement collider
    private float m_fSpeed; // Speed of player movement
    private Vector2 m_v2rightPoint; // Right collision position
    private Vector2 m_v2leftPoint; // left collision position 
    private Vector2 m_v2StickInput; // Left stick input
    private Vector2 m_v2Offset; // Offset of circle collider
    #endregion

    #region Direction Variables
    private MoveDirection tempDir; // Temporary direction of player movement
    private MoveDirection dir = MoveDirection.LEFT; // Current direction player is facing
    #endregion

    void Start()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Gets reference to player
        m_Animator = m_gSprite.GetComponent<Animator>();
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Renderer = GetComponent<Renderer>();
        m_fRadius = GetComponent<CircleCollider2D>().radius; // Gets radius of player
        m_v2Offset = GetComponent<CircleCollider2D>().offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GM").GetComponent<GM>().isPaused == false)
        {
            m_v2StickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Gets the left stick input of controller

            m_fSpeed = m_gPlayer.GetComponent<Player>().GetMoveSpeed(); // Gets player movement speed
            tempDir = dir; // Saves direction player is moving
            Move(); // Calls move function to check for collision and move Player
            Direction(); // Changes the direction character is facing
            Dash(); // Player dash ability
            UpdateModel(); // Update model to face correct direction
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks for player input and faces the player in the direction of movement
    //--------------------------------------------------------------------------------------
    void Direction()
    {
        if (m_v2StickInput.x > 0.2f && m_v2StickInput.y > -0.5f && m_v2StickInput.y < 0.5f) // Checks if the left analog stick is in the right quadrant
        {
            dir = MoveDirection.RIGHT; // Sets player direction to Right
        }
        else if (m_v2StickInput.x < -0.2f && m_v2StickInput.y > -0.5f && m_v2StickInput.y < 0.5f) // Checks if the left analog stick is in the left quadrant
        {
            dir = MoveDirection.LEFT; // Sets player direction to Left
        }
        else if (m_v2StickInput.y > 0.2f && m_v2StickInput.x > -0.5f && m_v2StickInput.x < 0.5f) // Checks if the left analog stick is in the top quadrant
        {
            dir = MoveDirection.UP; // Sets player direction to Up
        }
        else if (m_v2StickInput.y < -0.2f && m_v2StickInput.x > -0.5f && m_v2StickInput.x < 0.5f) // Checks if the left analog stick is in the bottom quadrant
        {
            dir = MoveDirection.DOWN; // Sets player direction to Down
        }

    }

    //--------------------------------------------------------------------------------------
    // Checks for Player dash input
    // Checks for collision for dash
    // Sets end point for dash based on collision, direction and dash distance
    // Lerps to final dash position based on dash speed
    //--------------------------------------------------------------------------------------
    void Dash()
    {
        Vector2 rayOrigin = (Vector2)transform.position + new Vector2(GetComponent<CircleCollider2D>().offset.x, GetComponent<CircleCollider2D>().offset.y); // Gets origin of ray
        m_v2DashInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // Gets input of the left analog stick
        m_v2DashInput.Normalize(); // Normalizes input
        if ((Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash) && (m_v2DashInput.x > 0.2f ||
            m_v2DashInput.x < -0.2f ||
            m_v2DashInput.y > 0.2f ||
            m_v2DashInput.y < -0.2f))
        { // checks for dash input and if dash in available and a direction Player is moving
            this.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            this.GetComponent<AudioSource>().Play();
            m_bIsDashing = true; // Is dashing set to true untill end of dash
            int count = 0; // Count of collisions detected
            RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of objects the ray collides with
            count = Physics2D.Raycast(rayOrigin, m_v2DashInput, m_cfFilter, Hit, m_fDashDistance); // Ray casts in direction of movement
            Debug.DrawRay(rayOrigin, m_v2DashInput, Color.red); // Draws a debug ray to show the dash direction
            if (count > 0) // Checks if anything collided with the ray
            {
                m_v2EndDashPos.x = Hit[0].point.x - (m_v2DashInput.x * m_fRadius) - m_v2Offset.x; // Sets x end point of dash based on where ray hit and circle collider offset
                m_v2EndDashPos.y = Hit[0].point.y - (m_v2DashInput.y * m_fRadius) - m_v2Offset.y; // Sets y end point of dash based on where ray hit and circle collider offset
            }
            else // If nothing hit
            {
                m_v2EndDashPos = (Vector2)rayOrigin + (m_v2DashInput * m_fDashDistance) - m_v2Offset; // End position of dash set based on dash distance
            }
            m_v2StartDashPos = transform.position; // Sets start dash position based on player position
        }
        else if
            ((Input.GetButtonDown("Fire3") && !m_bIsDashing && m_bDash) && (m_v2DashInput.x > -0.2f ||
             m_v2DashInput.x < 0.2f ||
             m_v2DashInput.y > -0.2f ||
             m_v2DashInput.y < 0.2f))
        { // If dash input but not movement direction
            if (dir == MoveDirection.UP) // If Player is facing upwards
            {
                m_bIsDashing = true; // Dashing is true
                int count = 0; // Number of collisions with ray
                RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of collisions with ray
                count = Physics2D.Raycast(rayOrigin, Vector2.up, m_cfFilter, Hit, m_fDashDistance); // Raycast up 
                if (count > 0) // If any collisions detected
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.up.x * m_fRadius) - m_v2Offset.x; // Sets x end point of dash based on where ray hit and circle collider offset
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.up.y * m_fRadius) - m_v2Offset.y; // Sets y end point of dash based on where ray hit and circle collider offset
                }
                else // If nothing hit
                {
                    m_v2EndDashPos = (Vector2)rayOrigin + (Vector2.up * m_fDashDistance) - m_v2Offset; // End position of dash set based on dash distance
                }
                m_v2StartDashPos = transform.position; // Sets start dash position based on player position
            }
            else if (dir == MoveDirection.DOWN) // If Player is facing down
            {
                m_bIsDashing = true; // Dashing is true
                int count = 0; // Number of collisions with ray
                RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of collisions with ray
                count = Physics2D.Raycast(rayOrigin, Vector2.down, m_cfFilter, Hit, m_fDashDistance); // Raycast Down 
                if (count > 0) // If any collisions detected
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.down.x * 0.5f) - m_v2Offset.x; // Sets x end point of dash based on where ray hit and circle collider offset
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.down.y * 0.5f) - m_v2Offset.y; // Sets y end point of dash based on where ray hit and circle collider offset
                }
                else // If nothing hit
                {
                    m_v2EndDashPos = (Vector2)rayOrigin + (Vector2.down * m_fDashDistance) - m_v2Offset; // End position of dash set based on dash distance
                }
                m_v2StartDashPos = transform.position; // Sets start dash position based on player position
            }
            else if (dir == MoveDirection.RIGHT) // If Player is facing right
            {
                m_bIsDashing = true; // Dashing is true
                int count = 0; // Number of collisions with ray
                RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of collisions with ray
                count = Physics2D.Raycast(rayOrigin, Vector2.right, m_cfFilter, Hit, m_fDashDistance); // Raycast right               
                if (count > 0) // If any collisions detected
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.right.x * 0.5f) - m_v2Offset.x; // Sets x end point of dash based on where ray hit and circle collider offset
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.right.y * 0.5f) - m_v2Offset.y; // Sets y end point of dash based on where ray hit and circle collider offset
                }
                else // If nothing hit
                {
                    m_v2EndDashPos = (Vector2)rayOrigin + (Vector2.right * m_fDashDistance) - m_v2Offset; // End position of dash set based on dash distance
                }
                m_v2StartDashPos = transform.position; // Sets start dash position based on player position
            }
            else if (dir == MoveDirection.LEFT) // If Player is facing left
            {
                m_bIsDashing = true; // Dashing is true
                int count = 0; // If any collisions detected
                RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of collisions with ray
                count = Physics2D.Raycast(rayOrigin, Vector2.left, m_cfFilter, Hit, m_fDashDistance); // Raycast left  
                if (count > 0) // If any collisions detected
                {
                    m_v2EndDashPos.x = Hit[0].point.x - (Vector2.left.x * 0.5f) - m_v2Offset.x; // Sets x end point of dash based on where ray hit and circle collider offset
                    m_v2EndDashPos.y = Hit[0].point.y - (Vector2.left.y * 0.5f) - m_v2Offset.y; // Sets y end point of dash based on where ray hit and circle collider offset
                }
                else // If nothing hit
                {
                    m_v2EndDashPos = (Vector2)rayOrigin + (Vector2.left * m_fDashDistance) - m_v2Offset; // End position of dash set based on dash distance
                }
                m_v2StartDashPos = transform.position; // Sets start dash position based on player position
            }
        }

        if (m_bIsDashing) // If dashing
        {
            var Emmission = m_ParticleSystem.emission;
            Emmission.rateOverTime = 100f;
            UpdateDashParticle();
            m_ParticleSystem.Play();
            m_fDashTimer += Time.deltaTime * m_fDashSpeed; // Updates dash timer
            transform.position = Vector2.Lerp(m_v2StartDashPos, m_v2EndDashPos, m_fDashTimer); // Lerps to new position    
            m_bImmunity = true;
            if (Vector2.Distance(this.transform.position, m_v2EndDashPos) <= 0.05f) // If with in 0.05f of the end dash position end dash 
            {
                m_bIsDashing = false; // Turn dashing to false
                m_fDashTimer = 0; // Reset dash timer
                m_bDash = false; // Dash off cooldown
                m_fDashCDTimer = 0; // Dash cooldown timer to 0
            }

        }

        if (m_bImmunity)
        {
            GetComponentInChildren<CapsuleCollider2D>().enabled = false; // Turns of the capsule collider to avoid enemy projectiles
            m_fITimer += Time.deltaTime;
            if (m_fITimer >= m_fImmunityTime)
            {
                GetComponentInChildren<CapsuleCollider2D>().enabled = true; // Turns of the capsule collider to avoid enemy projectiles
                m_bImmunity = false;
                m_fITimer = 0;
            }
        }

        if (!m_bDash) // if dash on cooldown
        {
            UpdateCooldownIcon();
            m_fDashCDTimer += Time.deltaTime; // Start cooldown timer
            if (m_fDashCDTimer >= m_fDashCD) // if cooldown timer greater then cooldown
            {
                m_gDashIcon.SetActive(false);
                m_bDash = true; // Sets dash to true or available
            }
            //m_ParticleSystem.Stop();
            var Emmission = m_ParticleSystem.emission;
            Emmission.rateOverTime = 10f;
        }

    }

    //--------------------------------------------------------------
    //  Logics for the Dash cooldown icon on the UI
    //--------------------------------------------------------------
    private void UpdateCooldownIcon()
    {
        m_gDashIcon.SetActive(true); // Sets the dash icon on the UI to true
        float iconFillAmount = ((m_fDashCDTimer / m_fDashCD) - 1) * -1; // Sets the radial fill amount based on the Cooldown timer and the Cooldown length
        m_gDashIcon.GetComponent<Image>().fillAmount = iconFillAmount; // Sets fill amount
        float fDashCD = (m_fDashCD - m_fDashCDTimer) * 10;
        int nDashCD = (int)fDashCD;
        fDashCD = nDashCD;
        fDashCD = (fDashCD / 10f) + 0.1f;
        if (fDashCD % 1 == 0)
            m_gDashIcon.GetComponentInChildren<Text>().text = fDashCD.ToString() + ".0";
        else
            m_gDashIcon.GetComponentInChildren<Text>().text = fDashCD.ToString();

        if (m_gDashIcon.GetComponent<Image>().fillAmount < 0.02)
        {
            m_gDashIcon.GetComponent<Image>().fillAmount = 0;
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks the direction of movement
    // Updates sprite based on direction
    //--------------------------------------------------------------------------------------
    public void UpdateModel()
    {
        // If the player is inputting movement
        if (m_v2StickInput.x < -0.2f || m_v2StickInput.y < -0.2f || m_v2StickInput.x > 0.2f || m_v2StickInput.y > 0.2f)
        {
            // Moving is true
            m_Animator.SetBool("isMoving", true);
        }
        else
        {
            // Moving is false
            m_Animator.SetBool("isMoving", false);
        }
        // Set animator direction to the direction the player is moving
        m_Animator.SetInteger("Direction", (int)dir);
    }

    //--------------------------------------------------------------------------------------
    // Checks the direction of movement
    // Updates Material based on direction
    //--------------------------------------------------------------------------------------
    public void UpdateDashParticle()
    {
        if((int)dir == 0)
        {
            m_Renderer.material = m_mUpDash;
            m_Renderer.sortingOrder = 6;
        }
        else if((int)dir == 1)
        {
            m_Renderer.material = m_mDownDash;
            m_Renderer.sortingOrder = 4;
        }
        else if ((int)dir == 2)
        {
            m_Renderer.material = m_mLeftDash;
            m_Renderer.sortingOrder = 4;
        }
        else if ((int)dir == 3)
        {
            m_Renderer.material = m_mRightDash;
            m_Renderer.sortingOrder = 4;
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks the direction of the left analog input
    // Updates the left and right point for collision detection rays
    //--------------------------------------------------------------------------------------
    void Rotate()
    {
        m_v2StickInput.Normalize(); // Normalize left analog input
        m_v2leftPoint = this.transform.position; // Left point set to player position
        m_v2leftPoint += GetComponent<CircleCollider2D>().offset; // Adds offset to the left point position
        m_v2leftPoint += new Vector2(-m_v2StickInput.y, m_v2StickInput.x) / Mathf.Sqrt((m_v2StickInput.x * m_v2StickInput.x) + (m_v2StickInput.y * m_v2StickInput.y)) * m_fRadius; // Sets left point for collision
        m_v2rightPoint = this.transform.position; // Right point set to player position
        m_v2rightPoint += GetComponent<CircleCollider2D>().offset; // Adds offset to the right point position
        m_v2rightPoint -= new Vector2(-m_v2StickInput.y, m_v2StickInput.x) / Mathf.Sqrt((m_v2StickInput.x * m_v2StickInput.x) + (m_v2StickInput.y * m_v2StickInput.y)) * m_fRadius; // Sets right point for collision
    }

    //--------------------------------------------------------------------------------------
    // Checks for collision based on left and right points
    // Ray casts in direction of movement for collision
    // Moves in direction if movement is possible
    // Checks if sliding is possible against wall if movement isn't
    //--------------------------------------------------------------------------------------
    public void Move()
    {
        if (m_v2StickInput.x >= 0.2f || m_v2StickInput.x <= -0.2f || m_v2StickInput.y >= 0.2f || m_v2StickInput.y <= -0.2f) // Checks for left analog stick input
        {
            Rotate(); // Rotates the left and right positions for collision detection
            Vector2 rayOrigin = (Vector2)transform.position + new Vector2(GetComponent<CircleCollider2D>().offset.x, GetComponent<CircleCollider2D>().offset.y); // Gets ray origin based on player position and collider offset
            RaycastHit2D[] Hit = new RaycastHit2D[1]; // List of collisions detected
            int mCount = 0; // Middle ray count
            int lCount = 0; // Left ray count
            int rCount = 0; // Right ray count 
            m_v2StickInput.Normalize(); // Normalise stick input 
            mCount = Physics2D.Raycast(rayOrigin, m_v2StickInput, m_cfFilter, Hit, m_fRadius + 0.1f); // Middle ray cast
            lCount += Physics2D.Raycast(m_v2leftPoint, m_v2StickInput, m_cfFilter, Hit, m_fRadius + 0.1f); // Left ray cast
            rCount += Physics2D.Raycast(m_v2rightPoint, m_v2StickInput, m_cfFilter, Hit, m_fRadius + 0.1f); // Right ray cast
            if (mCount > 0 && rCount <= 0 && lCount <= 0) // if only the middle ray hits
            {
            }
            else if (lCount > 0 && rCount <= 0 && mCount <= 0) // if only the left ray hits
            {
                if (Hit[0].normal.x > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.x < 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.y > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
            }
            else if (rCount > 0 && lCount <= 0 && mCount <= 0) // if only the right ray hits
            {
                if (Hit[0].normal.x > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.x < 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.y > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
            }
            else if (rCount > 0 && lCount <= 0 && mCount > 0) // If middle and right rays hit
            {
                if (Hit[0].normal.x > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.x < 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.y > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
            }
            else if (rCount <= 0 && lCount > 0 && mCount > 0) // If the left and middle ray hit
            {
                if (Hit[0].normal.x > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.up * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.x < 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.down * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else if (Hit[0].normal.y > 0) // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.left * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
                else // Checks normal for sliding direction
                {
                    transform.Translate(Vector2.right * (Time.deltaTime * m_fSpeed)); // Slides against wall
                }
            }
            else if (mCount > 0 && lCount > 0 && rCount > 0) // If all rays hit
            {
            }
            else if (rCount > 0 && lCount > 0 && mCount <= 0) // If right and left ray hit but not middle
            {
            }
            else
            {
                transform.Translate(m_v2StickInput * (Time.deltaTime * m_fSpeed)); // If not rays hit move in direction of input
            }

        }
    }
}
