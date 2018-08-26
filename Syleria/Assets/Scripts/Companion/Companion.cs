using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour {
    public List<Node> list;
    public Pathing m_aStar;
    public GameObject m_gPlayer;
    public GameObject m_gNode;
    public float m_fSpeed = 0.1f;
    // Use this for initialization
    void Start ()
    {
        m_aStar = m_aStar.GetComponent<Pathing>();
        list = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position);
        for (int i = 0; i <= list.Count; i++)
        {
            if(list[i+1] != null)
            {
                Debug.DrawLine(list[i].WorldPosition, list[i + 1].WorldPosition, Color.red, 2.0f);
            }  
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        list = m_aStar.FindPath(this.transform.position, m_gPlayer.transform.position);
        transform.position = list[0].WorldPosition;
	}
}
