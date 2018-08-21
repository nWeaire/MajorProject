using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class aStarTiles : MonoBehaviour {

    [HideInInspector] public List<Vector2> m_v2TilePositions;
    // Use this for initialization
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    m_v2TilePositions.Add(new Vector2(x + tilemap.origin.x + 0.5f, y + tilemap.origin.y + 0.5f));                   
                }
                else{}
            }
        }
        this.GetComponentInParent<aStarGrid>().AddPosition(m_v2TilePositions);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
