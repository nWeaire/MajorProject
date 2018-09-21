using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatItem : MonoBehaviour {

    enum Stat{ATTACK_DAMAGE, ATTACK_RANGE, ATTACK_FIRERATE, MOVE_SPEED, HEALTH_CURRENT, HEALTH_MAX }
    [SerializeField] private Stat m_eStatToChange;
    private GameObject m_gPlayer;
    public int m_nStatChange = 0;
    public bool m_bPickUp = false;
	// Use this for initialization
	void Awake ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_bPickUp = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bPickUp = false;
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Interact") && m_bPickUp)
        {
            switch (m_eStatToChange)
            {
                case Stat.ATTACK_DAMAGE:
                    m_gPlayer.GetComponent<Player>().AddDamage(m_nStatChange);
                    break;
                case Stat.ATTACK_RANGE:
                    m_gPlayer.GetComponent<Player>().AddRange(m_nStatChange);
                    break;
                case Stat.ATTACK_FIRERATE:
                    m_gPlayer.GetComponent<Player>().AddFireRate(m_nStatChange);
                    break;
                case Stat.MOVE_SPEED:
                    m_gPlayer.GetComponent<Player>().AddMoveSpeed(m_nStatChange);
                    break;
                case Stat.HEALTH_CURRENT:
                    m_gPlayer.GetComponent<Player>().AddCurrentHealth(m_nStatChange);
                    break;
                case Stat.HEALTH_MAX:
                    m_gPlayer.GetComponent<Player>().AddMaxHealth(m_nStatChange);
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
