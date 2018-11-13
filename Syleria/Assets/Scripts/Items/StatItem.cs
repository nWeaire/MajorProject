//--------------------------------------------------------------------------------------
// Purpose: To update the players current Stat upgrades, and control the item which contains the upgrade
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatItem : MonoBehaviour {

    enum Stat{ATTACK_DAMAGE, ATTACK_RANGE, ATTACK_FIRERATE, MOVE_SPEED, HEALTH_CURRENT, HEALTH_MAX,
        SHOT_PIERCE, SHOT_TRI, SHOT_SPLIT, SHOT_EXPAND, SHOT_BOUNCE } // Enum for all changable stats
    [SerializeField] private Stat[] m_eStatToChange; // Reference to enum for stat to change
    private GameObject m_gPlayer; // Reference to player
    public int[] m_nStatChange; // Amount to change stat by
    public bool m_bPickUp = false; // If able to be picked up
    public GameObject m_gButtonPrompt;

    private void Awake()
    {
        m_gButtonPrompt = GameObject.FindGameObjectWithTag("ButtonPrompt");
        m_gButtonPrompt.GetComponent<Image>().enabled = false;
    }

    //--------------------------------------------------------------------------------------
    // When colliding with player
    // Gets reference to player script
    // Sets pickup bool to true so Player can interact with item
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") // If colliding with player
        {
            m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Find Player
            m_bPickUp = true; // Set pick up to true
            m_gButtonPrompt.GetComponent<Image>().enabled = true;
        }
    }

    //--------------------------------------------------------------------------------------
    // When no longer colliding with player
    // Sets pickup bool to false so Player can no longer interact with item
    //--------------------------------------------------------------------------------------
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bPickUp = false; // Set pickup to false if player moves away
            m_gButtonPrompt.GetComponent<Image>().enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_gButtonPrompt.GetComponent<Image>().enabled = true;
        }
    }

    //--------------------------------------------------------------------------------------
    // When player hits the interact button and pickup is true
    // Cycles through stats to change and Stat change amount
    // Applies changes to stats on player script and sets any abilities to true
    // Draws item panels with newly picked up item
    //--------------------------------------------------------------------------------------
    private void Update()
    { 
        if(Input.GetButtonDown("Interact") && m_bPickUp) // If item in range and pressing interact button
        {
            for (int i = 0; i < m_eStatToChange.Length; i++) // For all stats in start change list
            {
                switch (m_eStatToChange[i]) // Switch statment for stats to change
                {
                    case Stat.ATTACK_DAMAGE:
                        m_gPlayer.GetComponent<Player>().AddDamage(m_nStatChange[i]); // Adds to damage stat by int from stat to change
                        break;
                    case Stat.ATTACK_RANGE:
                        m_gPlayer.GetComponent<Player>().AddRange(m_nStatChange[i]); // Adds to Range stat by int from stat to change
                        break;
                    case Stat.ATTACK_FIRERATE:
                        m_gPlayer.GetComponent<Player>().AddFireRate(m_nStatChange[i]); // Adds to Fire rate stat by int from stat to change
                        break;
                    case Stat.MOVE_SPEED:
                        m_gPlayer.GetComponent<Player>().AddMoveSpeed(m_nStatChange[i]); // Adds to Move speed stat by int from stat to change
                        break;
                    case Stat.HEALTH_CURRENT:
                        m_gPlayer.GetComponent<Player>().AddCurrentHealth(m_nStatChange[i]); // Adds to Current Health stat by int from stat to change
                        break;
                    case Stat.HEALTH_MAX:
                        m_gPlayer.GetComponent<Player>().AddMaxHealth(m_nStatChange[i]); // Adds to Max health stat by int from stat to change
                        break;
                    case Stat.SHOT_PIERCE:
                        m_gPlayer.GetComponent<Player>().Pierce(true); // Turns piercing to true
                        break;
                    case Stat.SHOT_TRI:
                        m_gPlayer.GetComponent<Player>().Tri(true); // Turns tri shot to true
                        break;
                    case Stat.SHOT_SPLIT:
                        m_gPlayer.GetComponent<Player>().Split(true); // Turns split to true
                        break;
                    case Stat.SHOT_EXPAND:
                        m_gPlayer.GetComponent<Player>().Expand(true); // Turns expand to true
                        break;
                    case Stat.SHOT_BOUNCE:
                        m_gPlayer.GetComponent<Player>().Bounce(true); // Turns bounce to true
                        break;
                    default:
                        break;
                }
            }
            if (GameObject.FindGameObjectWithTag("ItemPanel")) // If item panel exists
            {
                GameObject.FindGameObjectWithTag("ItemPanel").GetComponent<ItemPanels>().AddItem(this.GetComponent<SpriteRenderer>().sprite); // Display item on item panel
            }
            gameObject.SetActive(false); // Sets this game object to inactive
        }
    }
}
