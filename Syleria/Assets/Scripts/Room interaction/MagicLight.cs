using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLight : MonoBehaviour
{

    public GameObject[] m_Lights; // Array of lights to turn on
    public float m_DelayBetweenLights = 0.5f; // Delay between each light activating
    private bool isTriggered = false; // If Player has triggered the object
    public float timer; // Timer for the lights activation
    private int index = 0; // Index of lights
    private void Start()
    {
        timer = m_DelayBetweenLights; // Timer set to delay so first light switches on immeadiantly
    }

    private void Update()
    {
        if (isTriggered) // If object is triggered
        {
            timer += Time.deltaTime; // Timer updated on deltaTime
            if (timer > m_DelayBetweenLights && index < m_Lights.Length) // If timer is greater then delay and there are still lights to turn on
            {
                m_Lights[index].SetActive(true); // Sets lights to true
                timer = 0; // Timer set to 0
                index += 1; // Add one to index
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") // If colliding with player
        {
            isTriggered = true; // Sets triggered to true
        }
    }
}
