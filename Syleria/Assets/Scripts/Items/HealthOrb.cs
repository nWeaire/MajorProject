using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour
{
    public int m_nHealthRestored;

	// Use this for initialization
	void Start()
    { 
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().AddCurrentHealth(m_nHealthRestored);
            Destroy(this.gameObject);
        }
    }
}
