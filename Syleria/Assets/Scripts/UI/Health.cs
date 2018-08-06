using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [SerializeField]private Slider m_sHealthSlider;
    private GameObject m_gPlayer;


	// Use this for initialization
	void Start ()
    {
        m_gPlayer = GameObject.FindGameObjectWithTag("Player");
        m_sHealthSlider.minValue = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_sHealthSlider.maxValue = m_gPlayer.GetComponent<Player>().GetMaxHealth();
        m_sHealthSlider.value = m_gPlayer.GetComponent<Player>().GetCurrentHealth();
    }
}
