using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStarGrid : MonoBehaviour {

    public GameObject m_gTest;
    private List<Vector2> m_av2NodePositions;
    private bool m_bGridSpawned = false;
    // Use this for initialization
	void Start ()
    {
        m_av2NodePositions = new List<Vector2>();
    }

	// Update is called once per frame
	void Update ()
    {
		if(m_av2NodePositions.Count != 0 && !m_bGridSpawned)
        {
            SpawnGrid();
            m_bGridSpawned = true;
        }
	}
    public void AddPosition(List<Vector2> list)
    {
        m_av2NodePositions.AddRange(list);
    }
    public void SpawnGrid()
    {
        foreach (Vector2 item in m_av2NodePositions)
        {
            //Instantiate(m_gTest, new Vector3(item.x, item.y), new Quaternion());
        }        
    }
}
