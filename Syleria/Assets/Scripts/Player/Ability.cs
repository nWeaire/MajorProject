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

public class Ability : MonoBehaviour
{

    enum CompanionSelected { FOX, TURTLE, BIRD } // Enum containing all possible companions

    public GameObject m_Aim; // Reference to aiming retical on player

    public ContactFilter2D m_wallLayer; // Contact Filter for wall layer
    
    #region Companion
    [SerializeField] CompanionSelected m_eCompanionSelected = CompanionSelected.FOX; // Current companion selected 
    private GameObject m_gCompanion; // Reference to companion
    #endregion

    #region Global
    [SerializeField] private float m_fAbilityCD = 5; // Global ability cooldown
    public float m_fAbilityCDTimer = 0; // Ability cooldown timer
    private bool m_bAbility = true; // Ability is available
    private bool m_bIsAbility = false; // IsAbility being used
    private bool m_bTriggerDown = false; // IsTrigger down
    #endregion

    #region Slash
    [SerializeField] private PolygonCollider2D m_cSlashCollider; // Polygon collisder for slash
    [SerializeField] private float m_fSlashRange; // Range of slash
    [SerializeField] private float m_fSlashWidth = 3; // Width of slash
    [SerializeField] private float m_fSlashDuration = 0.1f; // Duration slash exists
    [SerializeField] private int m_nSlashDamage = 50; // Slash damage
    private float m_fSlashDurationTimer = 0; // Duration timer for slash
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
    private Vector2 m_v2DirToEndPos;
    #endregion

    // Use this for initialization
    void Start()
    {
        
        m_gCompanion = GameObject.FindGameObjectWithTag("Companion");
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_eCompanionSelected)
        {
            case CompanionSelected.FOX:
                Slash();
                break;
            case CompanionSelected.TURTLE:
                Taunt();
                break;
            case CompanionSelected.BIRD:

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
        if (m_bIsAbility) // If Slashing
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
        if (!m_bIsAbility)
        {
            m_fAbilityCDTimer += Time.deltaTime;
        }
        if (m_fAbilityCDTimer >= m_fAbilityCD)
        {
            m_bAbility = true;
            m_fAbilityCDTimer = 0;
        }
    }

    public void Taunt()
    {
        m_aEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsAbility && m_bAbility && !m_bTriggerDown)
        {
            m_gCompanion.transform.position = this.transform.position;
            m_bIsAbility = true;
            m_bAbility = false;
            m_bTriggerDown = true;
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
            m_gCompanion.GetComponent<Companion>().m_eState = Companion.State.TAUNT;
            m_fAbilityCDTimer = 0f;
            Vector2 aimDirection = m_Aim.transform.up;
            aimDirection.Normalize();

            if (!m_bEndPosFound)
            {
                RaycastHit2D[] hit = new RaycastHit2D[1];
                int count = 0;
                count = Physics2D.Raycast(m_Aim.transform.position, aimDirection, m_wallLayer, hit, m_fTauntRange);
                if (count > 0)
                {
                    m_v2EndPos = new Vector2(hit[0].point.x, hit[0].point.y);
                }
                else
                {
                    m_v2EndPos = new Vector2((aimDirection.x * m_fTauntRange) + this.transform.position.x, (aimDirection.y * m_fTauntRange) + this.transform.position.y);
                }
                m_v2DirToEndPos = (m_v2EndPos - (Vector2)this.transform.position);
                m_v2DirToEndPos.Normalize();
                m_bEndPosFound = true;
            }

            if (!m_bIsTaunting)
            {

                m_gCompanion.transform.Translate(m_v2DirToEndPos * m_fTauntSpeed * Time.deltaTime);
            }

            if(Input.GetAxisRaw("Ability") < 0.2f)
            {
                m_bTriggerDown = false;
            }

            if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsTaunting && !m_bTriggerDown || Vector2.Distance(m_gCompanion.transform.position, m_v2EndPos) <= 0.8f && !m_bIsTaunting)
            {
                m_bIsTaunting = true;

                for (int i = 0; i < m_aEnemies.Length; i++)
                {
                    if (Vector2.Distance(m_aEnemies[i].transform.position, m_gCompanion.transform.position) <= m_fTauntRadius)
                    {
                        m_aEnemies[i].GetComponent<Enemy>().m_bTaunted = true;
                    }
                }
            }

        }

        if (m_bIsTaunting)
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
            m_bEndPosFound = false;
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
