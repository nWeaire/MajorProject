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

    #region Companion
    [SerializeField] Companion m_eCompanion = Companion.BIRD;
    #endregion

    #region Slash
    [SerializeField] private PolygonCollider2D m_cSlashCollider;
    [SerializeField] private float m_fSlashRange;
    [SerializeField] private float m_fSlashWidth = 3;
    [SerializeField] private float m_fSlashCD = 5;
    [SerializeField] private float m_fSlashDuration = 0.1f;
    [SerializeField] private int m_nSlashDamage = 50;
    private bool m_bIsSlashing = false;
    private bool m_bSlash = true;
    private float m_fSlashCDTimer = 0;
    private float m_fSlashDurationTimer = 0;
    #endregion

    #region Taunt
    [SerializeField] private float m_fTauntRange;
    [SerializeField] private float m_fTauntRadius;

    #endregion

    // Use this for initialization
    void Start ()
    {
    
    }
	
	// Update is called once per frame
	void Update ()
    {

        switch (m_eCompanion)
        {
            case Companion.FOX:
                Slash();
                break;
            case Companion.TURTLE:

                break;
            case Companion.BIRD:

                break;
            default:
                break;
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
            collision.gameObject.GetComponent<Enemy>().TakeDamage(m_nSlashDamage);
        }
    }
}
