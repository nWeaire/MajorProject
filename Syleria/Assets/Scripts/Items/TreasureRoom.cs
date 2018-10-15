//--------------------------------------------------------------------------------------
// Purpose: To track several items and prevent the player from grabbing multiple.
// Author: Hamish Smithers
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour {

    public List<GameObject> treasures = new List<GameObject>();

    private void Update()
    {
        for (int i = 0; i < treasures.Count; i++)
        {
            if(treasures[i].activeInHierarchy == false)
            {
                for(int j = 0; j < treasures.Count; j++)
                {
                    treasures[j].SetActive(false);
                }
                gameObject.SetActive(false);
            }
        }
    }

}
