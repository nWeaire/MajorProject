//--------------------------------------------------------------------------------------
// Purpose: Handles Health bar for UI
//
// Description:  Checks players max and current health stat and updates health bar
//
// Author: Nicholas Weaire
//
// Contributors: Hamish Smithers
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [SerializeField] private Slider m_sHealthSlider; // Reference to health slider
    [SerializeField] private Text m_tHealthNumber; // Reference to health text
    private GameObject m_gPlayer; // Reference to player object
    public int m_nSliderMinimumValue; //Value to set minimum slider value to

    public float m_fSliderLerpAmount; //Value copntrolling Lerp on Slider
    public float m_fColourLerpAmount; //Value controlling transition between colours

    private int m_nPreviousHealth; //Tracking the previous health value

    public Color m_cStandardColour = Color.white;
    public Color m_cFlashColour = Color.white;
    public Color m_cCurrentColour = Color.white;

    public float m_fHealthBarShakeIntensity;
    public float m_fHealthBarShakeDuration;
    public float m_fCameraShakeIntensity;
    public float m_fCameraShakeDuration;

    // Use this for initialization
    void Awake()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player"); // Sets reference to player
        m_sHealthSlider.minValue = m_nSliderMinimumValue; // Sets minimum value of slider
        m_tHealthNumber = m_sHealthSlider.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        m_tHealthNumber.text = m_gPlayer.GetComponent<Player>().GetCurrentHealth().ToString() + "/" + m_gPlayer.GetComponent<Player>().GetMaxHealth().ToString();
        m_sHealthSlider.maxValue = m_gPlayer.GetComponent<Player>().GetMaxHealth(); // Sets max value of health slider based on player max health stat
                                                                                    //m_sHealthSlider.value = m_gPlayer.GetComponent<Player>().GetCurrentHealth(); // Sets value of helath slider based on player current health stat
        m_sHealthSlider.value = Mathf.Lerp(m_sHealthSlider.value, m_gPlayer.GetComponent<Player>().GetCurrentHealth(), m_fSliderLerpAmount);

        if (m_gPlayer.GetComponent<Player>().GetCurrentHealth() < m_nPreviousHealth && m_gPlayer.GetComponent<Player>().GetCurrentHealth() > 0)
        { //Checks if the player has lost health
            
            m_cCurrentColour.r = m_cFlashColour.r;
            m_cCurrentColour.g = m_cFlashColour.g;
            m_cCurrentColour.b = m_cFlashColour.b;
            Camera.main.GetComponent<BenShake>().Shake(m_fCameraShakeDuration, m_fCameraShakeIntensity);

            m_sHealthSlider.gameObject.GetComponent<UIShake>().Shake(m_fHealthBarShakeDuration, m_fHealthBarShakeIntensity);

        }

        m_cCurrentColour.r = Mathf.Lerp(m_cCurrentColour.r, m_cStandardColour.r, m_fColourLerpAmount);
        m_cCurrentColour.g = Mathf.Lerp(m_cCurrentColour.g, m_cStandardColour.g, m_fColourLerpAmount);
        m_cCurrentColour.b = Mathf.Lerp(m_cCurrentColour.b, m_cStandardColour.b, m_fColourLerpAmount);
        m_sHealthSlider.targetGraphic.color = m_cCurrentColour;
        m_nPreviousHealth = m_gPlayer.GetComponent<Player>().GetCurrentHealth();
    }
}
