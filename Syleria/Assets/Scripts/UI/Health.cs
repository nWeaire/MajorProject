using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [SerializeField]private Slider m_sHealthSlider;
    private GameObject m_oPlayer;


	// Use this for initialization
	void Start ()
    {
        m_oPlayer = GameObject.FindGameObjectWithTag("Player");
        m_sHealthSlider.minValue = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_sHealthSlider.maxValue = m_oPlayer.GetComponent<Player>().GetMaxHealth();
        m_sHealthSlider.value = m_oPlayer.GetComponent<Player>().GetCurrentHealth();
    }
}
