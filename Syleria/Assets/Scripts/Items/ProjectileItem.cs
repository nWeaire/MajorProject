//--------------------------------------------------------------------------------------
// Purpose: To update the players current projectile upgrades, and control the item which contains the upgrade
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : MonoBehaviour {

    enum Upgrade {SHOT_PIERCE, SHOT_TRI, SHOT_SPLIT, SHOT_EXPAND, SHOT_BOUNCE}
    public bool m_bPickUp = false;
    [SerializeField] private Upgrade m_eUpgrade;
    private GameObject m_gPlayer;

    // Use this for initialization
    void Awake ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Interact") && m_bPickUp)
        {
            switch (m_eUpgrade)
            {
                case Upgrade.SHOT_PIERCE:
                    m_gPlayer.GetComponent<Player>().Pierce(true);
                    break;
                case Upgrade.SHOT_TRI:
                    m_gPlayer.GetComponent<Player>().Tri(true);
                    break;
                case Upgrade.SHOT_SPLIT:
                    m_gPlayer.GetComponent<Player>().Split(true);
                    break;
                case Upgrade.SHOT_EXPAND:
                    m_gPlayer.GetComponent<Player>().Expand(true);
                    break;
                case Upgrade.SHOT_BOUNCE:
                    m_gPlayer.GetComponent<Player>().Bounce(true);
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
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

}
