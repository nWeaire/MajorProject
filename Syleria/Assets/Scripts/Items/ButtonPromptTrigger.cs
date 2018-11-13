using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPromptTrigger : MonoBehaviour {

    private GameObject m_gButtonPrompt;

    private void Awake()
    {
        m_gButtonPrompt = GameObject.FindGameObjectWithTag("ButtonPrompt");
        m_gButtonPrompt.GetComponent<Image>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            m_gButtonPrompt.GetComponent<Image>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            m_gButtonPrompt.GetComponent<Image>().enabled = false;
        }
    }
}
