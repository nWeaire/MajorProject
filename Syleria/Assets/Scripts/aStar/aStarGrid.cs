using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStarGrid : MonoBehaviour {

    struct GridNode
    {
        public bool Walkable;
        public Vector2 Index;
        public Vector2 WorldPosition;
    }

    private GridNode[,] m_grid;
    public int m_nGridHeight = 1;
    public int m_nGridWidth = 1;
    public GameObject m_gNode;
    public LayerMask m_WalkableLayer;

	// Use this for initialization
	void Start ()
    {
        m_grid = new GridNode[m_nGridWidth, m_nGridHeight];
        for (int i = 0; i < m_nGridWidth; i++)
        {
            for (int j = 0; j < m_nGridHeight; j++)
            {
                m_grid[i, j].Walkable = Physics2D.Raycast((this.transform.position + new Vector3(i, j)) + new Vector3(0, 0, -1), Vector3.forward, 0.1f, m_WalkableLayer);
                m_grid[i, j].Index.x = i;
                m_grid[i, j].Index.y = j;
                m_grid[i, j].WorldPosition = this.transform.position + new Vector3(i, j);

                Debug.DrawRay((this.transform.position + new Vector3(i, j)) + new Vector3(0, 0, -1), Vector3.forward, Color.red, 10.0f);

                if (m_grid[i, j].Walkable)
                {
                    Instantiate(m_gNode, (this.transform.position + new Vector3(i, j)) , new Quaternion());
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
