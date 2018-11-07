using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLight : MonoBehaviour
{

    public GameObject[] m_Lights;
    public float m_DelayBetweenLights = 0.5f;
    private bool isTriggered = false;
    public float timer;
    private int index = 0;
    private void Start()
    {
        timer = m_DelayBetweenLights;
    }

    private void Update()
    {
        if (isTriggered)
        {
            timer += Time.deltaTime;
            if (timer > m_DelayBetweenLights && index < m_Lights.Length)
            {
                m_Lights[index].SetActive(true);
                timer = 0;
                index += 1;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isTriggered = true;
        }
    }
}
