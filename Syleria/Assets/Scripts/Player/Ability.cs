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
    private GameObject m_gCompanion;
    #endregion

    #region Global
    [SerializeField] private float m_fAbilityCD = 5;
    public float m_fAbilityCDTimer = 0;
    private bool m_bAbility = true;
    private bool m_bIsAbility = false;
    #endregion

    #region Slash
    [SerializeField] private PolygonCollider2D m_cSlashCollider;
    [SerializeField] private float m_fSlashRange;
    [SerializeField] private float m_fSlashWidth = 3;
    [SerializeField] private float m_fSlashDuration = 0.1f;
    [SerializeField] private int m_nSlashDamage = 50;
    private float m_fSlashDurationTimer = 0;
    #endregion

    #region Taunt
    [SerializeField] private float m_fTauntRange = 5.0f;
    [SerializeField] private float m_fTauntRadius = 3.0f;
    [SerializeField] private float m_fTauntSpeed = 0.5f;
    [SerializeField] private float m_fTauntDuration = 2.5f;
    private bool m_bEndPosFound = false;
    private bool m_bIsTaunting = false;
    private float m_fTauntDurationTimer = 0;
    private GameObject[] m_aEnemies;
    private Vector2 m_v2EndPos;
    #endregion

    // Use this for initialization
    void Start ()
    {
        m_gCompanion = GameObject.FindGameObjectWithTag("Companion");
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
                Taunt();
                break;
            case Companion.BIRD:

                break;
            default:
                break;
        }
    }

    public void Slash()
    {
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsAbility && m_bAbility)
        {
            m_bIsAbility = true;
            m_bAbility = false;
        }

        if (m_fSlashDurationTimer >= m_fSlashDuration)
        {
            m_cSlashCollider.enabled = false;
            m_bIsAbility = false;
            m_fSlashDurationTimer = 0;
        }
        if(m_bIsAbility) // If Slashing
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
        if(!m_bIsAbility)
        {
            m_fAbilityCDTimer += Time.deltaTime;
        }
        if(m_fAbilityCDTimer >= m_fAbilityCD)
        {
            m_bAbility = true;
            m_fAbilityCDTimer = 0;
        }
    }

    public void Taunt()
    {
        m_aEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsAbility && m_bAbility)
        {
            m_gCompanion.transform.position = this.transform.position;
            m_bIsAbility = true;
            m_bAbility = false;
        }

        if (m_fTauntDurationTimer >= m_fTauntDuration)
        {
            //m_cSlashCollider.enabled = false;
            m_bIsAbility = false;
            m_bIsTaunting = false;
            m_fTauntDurationTimer = 0;
            for (int i = 0; i < m_aEnemies.Length; i++)
            {
                 m_aEnemies[i].GetComponent<Enemy>().m_bTaunted = false;
            }
        }
        if (m_bIsAbility)
        {
            m_fAbilityCDTimer = 0f;
            Vector2 aimDirection = m_Aim.transform.up;
            aimDirection.Normalize();
            if (!m_bEndPosFound)
            {
                m_v2EndPos = new Vector2((aimDirection.x * m_fTauntRange) + this.transform.position.x, (aimDirection.y * m_fTauntRange) + this.transform.position.y);
                m_bEndPosFound = true;
            }
            Vector2 dirToEndPos = (m_v2EndPos - (Vector2)this.transform.position);
            dirToEndPos.Normalize();
            m_gCompanion.transform.Translate(dirToEndPos * m_fTauntSpeed * Time.deltaTime);

            if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsTaunting)
            {
                m_bIsTaunting = true;
                for (int i = 0; i < m_aEnemies.Length; i++)
                {
                    if (Vector2.Distance(m_aEnemies[i].transform.position, m_gCompanion.transform.position) <= m_fTauntRadius)
                    {
                        m_aEnemies[i].GetComponent<Enemy>().m_bTaunted = true;
                    }
                }
                m_bEndPosFound = false;
            }
           
        }
        if(m_bIsTaunting)
        {
            m_fTauntDurationTimer += Time.deltaTime;
        }
        if (!m_bAbility)
        {
            m_fAbilityCDTimer += Time.deltaTime;
        }
        if (m_fAbilityCDTimer >= m_fAbilityCD)
        {
            m_bAbility = true;
            m_fAbilityCDTimer = 0;
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
