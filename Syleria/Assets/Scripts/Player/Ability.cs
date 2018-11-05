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
using UnityEngine.UI;
public class Ability : MonoBehaviour
{

    enum CompanionSelected { FOX, TURTLE, BIRD } // Enum containing all possible companions
    public GameObject m_Aim; // Reference to aiming retical on player
    private GameObject m_gPlayer; // Reference to playerStats
    public ContactFilter2D m_wallLayer; // Contact Filter for wall layer
    public GameObject m_gFoxAbilityIcon;
    public GameObject m_gTurtleAbilityIcon;
    public GameObject m_gFoxCDAbilityIcon;
    public GameObject m_gTurtleCDAbilityIcon;

    #region Companion
    [SerializeField] CompanionSelected m_eCompanionSelected = CompanionSelected.FOX; // Current companion selected 
    //public GameObject m_gBird; // Reference to Bird
    public GameObject m_gFox; // Reference to Fox
    public GameObject m_gTurtle; // Reference to Turtle
    public GameObject m_gTaunt; // Reference to taunt
    public GameObject m_gSlash;
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
    [SerializeField] private float m_fWhirlWindRadius = 3f; // Range of slash
    [SerializeField] private float m_fSlashWidth = 3; // Width of slash
    [SerializeField] private float m_fSlashDuration = 0.1f; // Duration slash exists
    [SerializeField] private int m_nSlashDamage = 50; // Slash damage
    private float m_fSlashDurationTimer = 0; // Duration timer for slash
    #endregion

    #region Taunt
    [SerializeField] private float m_fTauntRange = 5.0f; // Range of taunt
    [SerializeField] private float m_fTauntRadius = 3.0f; // Radius of taunt
    [SerializeField] private float m_fTauntSpeed = 0.5f; // Movement of taunt
    [SerializeField] private float m_fTauntDuration = 2.5f; // Taunt duration
    private bool m_bEndPosFound = false; // Bool for end position
    private bool m_bIsTaunting = false; // Bool for taunting
    private float m_fTauntDurationTimer = 0; // Taunt duration timer
    private GameObject[] m_aEnemies; // List of enemies
    private Vector2 m_v2EndPos; // End position of taunt
    private Vector2 m_v2DirToEndPos; // Direction to end position
    public int m_nAfterShockDamage = 10; // Damage for aftershock
    #endregion

    #region Sounds
    public AudioClip m_sSlashSFX;
    public AudioClip m_sTauntSFX;
    #endregion

    private void Awake()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("GM").GetComponent<GM>().isPaused == false)
        {
            m_aEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Finds all game objects with enemy tag
            if (m_aEnemies.Length == 0)
            {
                m_aEnemies = null;
            }
            ChangeCompanion(); // Check which companion is selected

            switch (m_eCompanionSelected) // Switch statement based on companion picked
            {
                case CompanionSelected.FOX:
                    if (m_gPlayer.GetComponent<Player>().m_bWhirlwind)
                    {
                        Whirlwind();
                    }
                    else
                    {
                        Slash(); // Check if slashing
                    }
                    break;
                case CompanionSelected.TURTLE:
                    Taunt(); // Check if taunting
                    break;
                //case CompanionSelected.BIRD:          
                //    break;
                default:
                    break;
            }

            if (!m_bAbility) // If ability isn't available
            {
                m_fAbilityCDTimer += Time.deltaTime; // ability cool down timer active
                UpdateCooldownIcon();
            }
            if (m_fAbilityCDTimer >= m_fAbilityCD) // If ability cool down timer greater then cooldown
            {
                m_bAbility = true; // Ability available
                m_gTurtleCDAbilityIcon.SetActive(false);
                m_gFoxCDAbilityIcon.SetActive(false);
                m_fAbilityCDTimer = 0; // Cooldown timer set to 0
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // Checks for input to change the selected companion
    // Updates bool for new companion
    //--------------------------------------------------------------------------------------
    private void ChangeCompanion()
    {
        if (!m_bIsAbility)
        {
            if (Input.GetButtonDown("Fox") && m_eCompanionSelected != CompanionSelected.FOX) // If fox button down
            {
                if (m_eCompanionSelected == CompanionSelected.TURTLE) // If current companion is the turtle
                {
                    m_gFox.transform.position = m_gTurtle.transform.position; // Sets transform for fox to turtle position
                }
                m_eCompanionSelected = CompanionSelected.FOX; // Sets selected companion to fox
                m_gFox.gameObject.SetActive(true); // Sets fox to active
                m_gTurtle.gameObject.SetActive(false); // Sets turtle to inactive
                m_gFoxAbilityIcon.SetActive(true);
                m_gTurtleAbilityIcon.SetActive(false);
            }
            if (Input.GetButtonDown("Turtle") && m_eCompanionSelected != CompanionSelected.TURTLE) // If turtle button down
            {
                if (m_eCompanionSelected == CompanionSelected.FOX) // If current companion is fox
                {
                    m_gTurtle.transform.position = m_gFox.transform.position; // Sets turtle position to fox position
                }
                m_eCompanionSelected = CompanionSelected.TURTLE; // Sets selected companion to turtle
                m_gFox.gameObject.SetActive(false); // Sets fox to inactive
                m_gTurtle.gameObject.SetActive(true); // Sets turtle to active
                m_gFoxAbilityIcon.SetActive(false);
                m_gTurtleAbilityIcon.SetActive(true);
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // Handles the logic behind slash, Checks for input, direction and damage of slash                    
    //--------------------------------------------------------------------------------------
    public void Slash()
    {
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsAbility && m_bAbility || Input.GetKey(KeyCode.Space) && !m_bIsAbility && m_bAbility) // Checks for slash input, and ability is available
        {
            GetComponent<AudioSource>().clip = m_sSlashSFX;
            GetComponent<AudioSource>().Play();
            m_bIsAbility = true; // Ability in use to true
            m_bAbility = false; // Ability available to false
        }

        if (m_fSlashDurationTimer >= m_fSlashDuration) // If slash duration timer is greater then slash duration
        {
            m_cSlashCollider.enabled = false; // Slash collider set to active
            m_bIsAbility = false; // Is ability in use to false
            m_fSlashDurationTimer = 0; // Slash duration to 0
        }
        if (m_bIsAbility) // If Slashing
        {
            m_fSlashDurationTimer += Time.deltaTime; // slash duration timer increased
            Vector2 aimDirection = m_Aim.transform.up; // Sets aim direction
            aimDirection.Normalize(); // Normalize aim direction
            m_cSlashCollider.enabled = true; // Slash collider to true
            Vector2[] slashPoints = new Vector2[3]; // Array of slash points for polygon collider

            Vector2 m_v2leftPoint = aimDirection * m_fSlashRange; // Left points starting point set
                                                                  // Left point sets width of polygon slash point
            m_v2leftPoint += new Vector2(-aimDirection.y, aimDirection.x) / Mathf.Sqrt((aimDirection.x * aimDirection.x) + (aimDirection.y * aimDirection.y)) * m_fSlashWidth;

            Vector2 m_v2rightPoint = aimDirection * m_fSlashRange; // Right points set for polygon collider
            m_v2rightPoint -= new Vector2(-aimDirection.y, aimDirection.x) / Mathf.Sqrt((aimDirection.x * aimDirection.x) + (aimDirection.y * aimDirection.y)) * m_fSlashWidth;

            slashPoints[0] = new Vector2(0, -0.5f); // Sets initial point for slash polygon collider
            slashPoints[1] = new Vector2(m_v2rightPoint.x, m_v2rightPoint.y); // Sets right point for slash polygon collider
            slashPoints[2] = new Vector2(m_v2leftPoint.x, m_v2leftPoint.y); // Sets left point for slash polygon collider
            m_cSlashCollider.points = slashPoints; // Sets polygon collider points
            m_gSlash.SetActive(true); // Sets slash sprite to true
            float m_fAngle = Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg; // Gets angle based on direction of the right stick input
            m_gSlash.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -m_fAngle)); // Rotates retical    

        }
    }

    //--------------------------------------------------------------------------------------
    // Checks for taunt input, cooldown and ability parameters
    // Taunts enemies on ability activation in radius around companion
    //--------------------------------------------------------------------------------------
    public void Taunt()
    {
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsAbility && m_bAbility && !m_bTriggerDown) // Checks for taunt button and ability available
        {
            m_gTurtle.transform.position = this.transform.position; // Sets turtle position to player position
            m_bIsAbility = true; // Sets ability in use to true
            m_bAbility = false; // Sets ability availability to false
            m_bTriggerDown = true; // Sets trigger down to true
        }

        if (m_fTauntDurationTimer >= m_fTauntDuration) // Taunt duration timer greater than taunt duration
        {
            m_bIsAbility = false; // Ability not in use
            m_bIsTaunting = false; // Is taunt is false
            m_fTauntDurationTimer = 0; // Taunt duration time set to 0
            if (m_aEnemies != null)
            {

                for (int i = 0; i < m_aEnemies.Length; i++) // For every enemy
                {
                    if (m_aEnemies[i] != null)
                    {
                        if (m_gPlayer.GetComponent<Player>().m_bAfterShock) // If aftershock is active
                        {
                            m_aEnemies[i].GetComponent<Enemy>().TakeDamage(m_nAfterShockDamage); // Deals damage to enemies
                        }
                        m_aEnemies[i].GetComponent<Enemy>().m_bTaunted = false; // Sets all enemies to be untaunted
                    }
                }
            }
            m_bEndPosFound = false;
            //m_gTaunt.SetActive(false); // Taunt to false
        }
        if (m_bIsAbility) // Is ability in use
        {
            m_gTurtle.GetComponent<Companion>().m_eState = Companion.State.TAUNT; // Sets turtle to taunt state
            m_fAbilityCDTimer = 0f; // Sets cooldown timer to 0
            Vector2 aimDirection = m_Aim.transform.up; // Sets aim direction
            aimDirection.Normalize(); // Normalizes aim direction

            if (!m_bEndPosFound) // If no end position
            {
                RaycastHit2D[] hit = new RaycastHit2D[1]; // Array of raycast hit objects
                int count = 0; // Count of objects hit
                count = Physics2D.Raycast(m_Aim.transform.position, aimDirection, m_wallLayer, hit, m_fTauntRange); // Checks for objects in range of taunt range
                if (count > 0) // If objects hit
                {
                    m_v2EndPos = new Vector2(hit[0].point.x, hit[0].point.y); // End position set to hit position
                }
                else
                {
                    // Sets end position to taunt range from the player
                    m_v2EndPos = new Vector2((aimDirection.x * m_fTauntRange) + this.transform.position.x, (aimDirection.y * m_fTauntRange) + this.transform.position.y);
                }
                m_v2DirToEndPos = (m_v2EndPos - (Vector2)this.transform.position); // Direction to end position 
                m_v2DirToEndPos.Normalize(); // Normalize direction to end position
                m_bEndPosFound = true; // End position found
            }

            if (!m_bIsTaunting) // If not taunting
            {
                m_gTurtle.transform.Translate(m_v2DirToEndPos * m_fTauntSpeed * Time.deltaTime); // Translates turtle position towards end points
            }

            if (Input.GetAxisRaw("Ability") < 0.2f) // If ability button not down
            {
                m_bTriggerDown = false; // Trigger down to false
            }
            // If turtle has reached the end position or the ability button has been retriggered
            if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsTaunting && !m_bTriggerDown || Vector2.Distance(m_gTurtle.transform.position, m_v2EndPos) <= 0.8f && !m_bIsTaunting)
            {
                GetComponent<AudioSource>().clip = m_sTauntSFX;
                GetComponent<AudioSource>().Play();
                m_bIsTaunting = true; // Taunting set to true
                m_gTaunt.SetActive(true); // Taunt sprite set to true
                if (m_aEnemies != null)
                {
                    for (int i = 0; i < m_aEnemies.Length; i++) // For all enemies
                    {
                        if (Vector2.Distance(m_aEnemies[i].transform.position, m_gTurtle.transform.position) <= m_fTauntRadius) // Checks if enemies are within taunt radius
                        {
                            m_aEnemies[i].GetComponent<Enemy>().m_bTaunted = true; // Sets enemies state to taunted
                        }
                    }
                }
            }

        }

        if (m_bIsTaunting) // If taunting
        {
            m_fTauntDurationTimer += Time.deltaTime; // Taunt duration timer active
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // If trigger hits enemy tags
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(m_nSlashDamage); // Deals damage to enemies
        }
    }

    void Whirlwind()
    {
        if (Input.GetAxisRaw("Ability") > 0.2f && !m_bIsAbility && m_bAbility)
        {
            m_bAbility = false; // Ability available to false
            for (int i = 0; i < m_aEnemies.Length; i++) // For all enemies
            {
                if (Vector2.Distance(m_aEnemies[i].transform.position, this.transform.position) <= m_fWhirlWindRadius) // Checks if enemies are within taunt radius
                {
                    m_aEnemies[i].GetComponent<Enemy>().TakeDamage(m_nSlashDamage);
                }
            }
        }
    }

    private void UpdateCooldownIcon()
    {
        if (m_eCompanionSelected == CompanionSelected.FOX)
        {
            m_gFoxCDAbilityIcon.SetActive(true);
            float iconFillAmount = ((m_fAbilityCDTimer / m_fAbilityCD) - 1) * -1;
            m_gFoxCDAbilityIcon.GetComponent<Image>().fillAmount = iconFillAmount;
            float fDashCD = (m_fAbilityCD - m_fAbilityCDTimer) * 10;
            int nDashCD = (int)fDashCD;
            fDashCD = nDashCD;
            fDashCD = (fDashCD / 10f) + 0.1f;
            if (fDashCD % 1 == 0)
                m_gFoxCDAbilityIcon.GetComponentInChildren<Text>().text = fDashCD.ToString() + ".0";
            else
                m_gFoxCDAbilityIcon.GetComponentInChildren<Text>().text = fDashCD.ToString();

            if (m_gFoxCDAbilityIcon.GetComponent<Image>().fillAmount < 0.02)
            {
                m_gFoxCDAbilityIcon.GetComponent<Image>().fillAmount = 0;
            }
        }
        else
        {
            m_gTurtleCDAbilityIcon.SetActive(true);
            float iconFillAmount = ((m_fAbilityCDTimer / m_fAbilityCD) - 1) * -1;
            m_gTurtleCDAbilityIcon.GetComponent<Image>().fillAmount = iconFillAmount;
            float fDashCD = (m_fAbilityCD - m_fAbilityCDTimer) * 10;
            int nDashCD = (int)fDashCD;
            fDashCD = nDashCD;
            fDashCD = (fDashCD / 10f) + 0.1f;
            if (fDashCD % 1 == 0)
                m_gTurtleCDAbilityIcon.GetComponentInChildren<Text>().text = fDashCD.ToString() + ".0";
            else
                m_gTurtleCDAbilityIcon.GetComponentInChildren<Text>().text = fDashCD.ToString();

            if (m_gTurtleCDAbilityIcon.GetComponent<Image>().fillAmount < 0.02)
            {
                m_gTurtleCDAbilityIcon.GetComponent<Image>().fillAmount = 0;
            }
        }
    }
}
