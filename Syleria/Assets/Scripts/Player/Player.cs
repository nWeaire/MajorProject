//--------------------------------------------------------------------------------------
// Purpose: Holds player stats
//
// Description:  Allows for altering, getting and setting of Player Stats. Also clamps player stats
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Base Player Stats
    [SerializeField] private int m_nDamage = 10; // Damage stats to affect bullet damage
    [SerializeField] private int m_nCurrentHealth; // Current health of player
    [SerializeField] private int m_nMaxHealth = 100; // Max Health that current health can reach
    [SerializeField] private float m_nRange = 5.0f; // Max range of the bullets 
    [SerializeField] private float m_nFireRate = 2.0f; // Fire rate
    [SerializeField] private float m_fSpeed = 5.0f; // Speed of player
    #endregion 

    #region Getter and Setter Functions for Base Player Stats
    public float GetRange() {return this.m_nRange;} // Returns Range
    public int GetDamage() { return this.m_nDamage; } // Returns Damage
    public int GetCurrentHealth() { return this.m_nCurrentHealth; } // Returns Current health
    public int GetMaxHealth() { return this.m_nMaxHealth; } // Returns Max health
    public float GetFireRate() { return this.m_nFireRate; } // Returns Fire Rate
    public float GetMoveSpeed() { return this.m_fSpeed; } // Returns player speed

    public void SetMaxHealth(int maxHealth) { this.m_nMaxHealth = maxHealth; } // Sets max health 
    public void SetCurrentHealth(int currentHealth) { this.m_nCurrentHealth = currentHealth; } // sets current health
    public void SetDamage(int damage) { this.m_nDamage = damage; } // Sets player damage
    public void SetRange(float range) { this.m_nRange = range; } // Sets player range
    public void SetFireRate(float fireRate) { this.m_nFireRate = fireRate; } // Sets fire rate
    public void SetMoveSpeed(float speed) { this.m_fSpeed = speed; } // Sets player speed
    #endregion

    #region Stat Change Functions For base Player Stats
    public void AddMaxHealth(int maxHealth) { this.m_nMaxHealth += maxHealth; } // Adds value to current max health stat
    public void AddCurrentHealth(int currentHealth) { this.m_nCurrentHealth += currentHealth; } // Adds value to current health stat
    public void AddDamage(int damage) { this.m_nDamage += damage; } // Adds value to damage stat
    public void AddRange(float range) { this.m_nRange += range; } // Adds value to range stat
    public void AddFireRate(float fireRate) { this.m_nFireRate += fireRate; } // Adds value to fire rate stat
    public void AddMoveSpeed(float speed) { this.m_fSpeed += speed; } // Adds value to speed stat
    #endregion

    #region Upgrades
    public bool m_bPierce = false;
    public bool m_bTri = false;
    public bool m_bSplit = false;
    public bool m_bExpand = false;
    public bool m_bExplode = false;
    #endregion

    #region Upgrade Setters
    public void Pierce(bool True) { this.m_bPierce = True; }
    public void Tri(bool True) { this.m_bTri = True; }
    public void Split(bool True) { this.m_bSplit = True; }
    public void Expand(bool True) { this.m_bExpand = True; }
    public void Explode(bool True) { this.m_bExplode = True; }
    #endregion

    // Use this for initialization
    void Start ()
    {
        m_nCurrentHealth = m_nMaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Clamp(); // Clamp all stats to there maximum or minimum
    }

    //----------------------------------------------------------------------------------
    // Clamps all stats to there minimum and maximum values
    //----------------------------------------------------------------------------------
    private void Clamp()
    {
        if (m_nRange <= 0) // Clamps range to a minimum of 1
        {
            m_nRange = 1; 
        }
        if (m_nDamage <= 0) // Clamps Damage to a minimum of 1
        {
            m_nDamage = 1;
        }
        if (m_nMaxHealth <= 0) // Clamps Max Health to a minimum of 1
        {
            m_nMaxHealth = 1;
        }
        if (m_nCurrentHealth <= 0) // Clamps Current Health to a minimum of 0
        {
            m_nCurrentHealth = 0;
        }
        if(m_nCurrentHealth > m_nMaxHealth) // Clamps currents health to a maximum of max health
        {
            m_nCurrentHealth = m_nMaxHealth;
        }
    }
}
