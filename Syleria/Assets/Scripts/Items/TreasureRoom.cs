//--------------------------------------------------------------------------------------
// Purpose: To track several items and prevent the player from grabbing multiple.
// Author: Hamish Smithers
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour {
    public float horizontalOffest;
    public List<GameObject> treasuresList = new List<GameObject>();

    private List<GameObject> roomTreasures = new List<GameObject>();

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject GO = Instantiate(treasuresList[Random.Range(0, treasuresList.Count)],transform.position+Vector3.zero+(Vector3.right*(-horizontalOffest+(horizontalOffest*i))),Quaternion.identity,this.transform) as GameObject;
            roomTreasures.Add(GO);
        }
    }


    private void Update()
    {
        for (int i = 0; i < roomTreasures.Count; i++)
        {
            if(roomTreasures[i].activeInHierarchy == false)
            {
                for(int j = 0; j < roomTreasures.Count; j++)
                {
                    roomTreasures[j].SetActive(false);
                }
                gameObject.SetActive(false);
            }
        }
    }

}
