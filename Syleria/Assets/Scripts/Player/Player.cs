using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int m_nDamage = 10;
    [SerializeField] private int m_nCurrentHealth;
    [SerializeField] private int m_nMaxHealth = 100;
    [SerializeField] private int m_nRange = 5;


    // Use this for initialization
    void Start ()
    {
        m_nCurrentHealth = m_nMaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Clamp();
        testChange();
    }

    public int GetRange() {return this.m_nRange;}
    public int GetDamage() { return this.m_nDamage; }
    public int GetCurrentHealth() { return this.m_nCurrentHealth; }
    public int GetMaxHealth() { return this.m_nMaxHealth; }

    public void SetMaxHealth(int maxHealth) { this.m_nMaxHealth = maxHealth; }
    public void SetCurrentHealth(int currentHealth) { this.m_nCurrentHealth = currentHealth; }
    public void SetDamage(int damage) { this.m_nDamage = damage; }
    public void SetRange(int range) { this.m_nRange = range; }

    public void AddMaxHealth(int maxHealth) { this.m_nMaxHealth += maxHealth; }
    public void AddCurrentHealth(int currentHealth) { this.m_nCurrentHealth += currentHealth; }
    public void AddDamage(int damage) { this.m_nDamage += damage; }
    public void AddRange(int range) { this.m_nRange += range; }

    private void Clamp()
    {
        if (m_nRange <= 0)
        {
            m_nRange = 1;
        }
        if (m_nDamage <= 0)
        {
            m_nDamage = 1;
        }
        if (m_nMaxHealth <= 0)
        {
            m_nMaxHealth = 1;
        }
        if (m_nCurrentHealth <= 0)
        {
            m_nCurrentHealth = 0;
        }
        if(m_nCurrentHealth > m_nMaxHealth)
        {
            m_nCurrentHealth = m_nMaxHealth;
        }
    }

    private void testChange()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddRange(5);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddRange(-5);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddCurrentHealth(20);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddCurrentHealth(-20);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            AddMaxHealth(20);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddMaxHealth(-20);
        }
    }
}
