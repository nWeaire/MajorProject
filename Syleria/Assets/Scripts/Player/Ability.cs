//--------------------------------------------------------------------------------------
// Purpose: Handles all abilities for player
//
// Description:  Handles all collision, interaction and features of abilities
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {

    enum Companion {FOX, TURTLE, BIRD}

    public GameObject m_Aim;

    #region Shield
    [SerializeField] private float m_fShieldCD = 3.0f; // Cooldown for shield
    [SerializeField] private float m_fShieldDuration = 2.0f; // Duration of shield
    private float m_fShieldCDTimer = 3.0f; // Shield cooldown timer
    private float m_fShieldDurationTimer = 0.0f; // Shield duration timer
    private bool m_bIsShielded = false; // Checks if shielded
    [SerializeField] private CircleCollider2D m_cCircleCollider;
    [SerializeField] private SpriteRenderer m_cSpriteRenderer;
    #endregion

    #region Companion
    [SerializeField] Companion m_eCompanion = Companion.BIRD;
    #endregion

    #region Slash
    [SerializeField] private PolygonCollider2D m_cSlashCollider;
    [SerializeField] private float m_fSlashRange;
    [SerializeField] private float m_fSlashWidth = 3;
    [SerializeField] private float m_fSlashCD = 5;
    [SerializeField] private float m_fSlashDuration = 0.1f;
    private bool m_bIsSlashing = false;
    private bool m_bSlash = true;
    private float m_fSlashCDTimer = 0;
    private float m_fSlashDurationTimer = 0;
    #endregion

    // Use this for initialization
    void Start ()
    {
        m_fShieldCDTimer = m_fShieldCD; // Sets cooldown timer to cooldown to allow for immediate use of shield
    }
	
	// Update is called once per frame
	void Update ()
    {

        switch (m_eCompanion)
        {
            case Companion.FOX:
                Slash();
                this.gameObject.tag = "Untagged";
                break;
            case Companion.TURTLE:

                break;
            case Companion.BIRD:
                this.gameObject.tag = "Shield";
                Shield();
                break;
            default:
                break;
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks for input
    // Creates circle colider and sprite around player
    // Handles shield duration and cooldown
    //--------------------------------------------------------------------------------------
    public void Shield()
    {
        if (Input.GetAxisRaw("Ability") > 0 && !m_bIsShielded && m_fShieldCDTimer >= m_fShieldCD) // Checks if left trigger is pressed and shield is available
        {
            m_cCircleCollider.enabled = true; // Turns on shield sprite
            m_cSpriteRenderer.enabled = true; // Turns on shield collider
            m_bIsShielded = true; // is shielded to true
        }

        if (m_bIsShielded) // If shielded
        {
            m_fShieldDurationTimer += Time.deltaTime; // Start duration timer
        }
        else if (!m_bIsShielded) // if not shielded
        {
            m_fShieldCDTimer += Time.deltaTime; // Start cooldown timer
        }

        if (m_fShieldDurationTimer >= m_fShieldDuration) // If shield duration timer is greater then duration
        {
            m_cSpriteRenderer.enabled = false; // Turn shield sprite off
            m_cCircleCollider.enabled = false; // Turn shield collider off
            m_fShieldDurationTimer = 0; // Set duration timer to 0
            m_bIsShielded = false; // Set shield to false
            m_fShieldCDTimer = 0; // Set cooldown timer to 0
        }
    }

    public void Slash()
    {
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsSlashing && m_bSlash)
        {
            m_bIsSlashing = true;
            m_bSlash = false;
        }

        if (m_fSlashDurationTimer >= m_fSlashDuration)
        {
            m_cSlashCollider.enabled = false;
            m_bIsSlashing = false;
            m_fSlashDurationTimer = 0;
        }
        if(m_bIsSlashing) // If Slashing
        {
            m_fSlashDurationTimer += Time.deltaTime;
            Vector2 aimDirection = m_Aim.transform.up;
            aimDirection.Normalize();
            m_cSlashCollider.enabled = true;
            Vector2[] slashPoints = new Vector2[3];

            Vector2 m_v2leftPoint = aimDirection * m_fSlashRange;
            m_v2leftPoint += new Vector2(-aimDirection.y, aimDirection.x) / Mathf.Sqrt((aimDirection.x * aimDirection.x) + (aimDirection.y * aimDirection.y)) * m_fSlashWidth;

            Vector2 m_v2rightPoint = aimDirection * m_fSlashRange;
            m_v2rightPoint -= new Vector2(-aimDirection.y, aimDirection.x) / Mathf.Sqrt((aimDirection.x * aimDirection.x) + (aimDirection.y * aimDirection.y)) * m_fSlashWidth;

            slashPoints[0] = new Vector2(0, -0.5f);
            slashPoints[1] = new Vector2(m_v2rightPoint.x, m_v2rightPoint.y);
            slashPoints[2] = new Vector2(m_v2leftPoint.x, m_v2leftPoint.y);
            m_cSlashCollider.points = slashPoints;        
        }
        if(!m_bIsSlashing)
        {
            m_fSlashCDTimer += Time.deltaTime;
        }
        if(m_fSlashCDTimer >= m_fSlashCD)
        {
            m_bSlash = true;
            m_fSlashCDTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(10);
        }
    }
}
