//--------------------------------------------------------------------------------------
// Purpose: To track several items and prevent the player from grabbing multiple.
// Author: Hamish Smithers
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour {
    public float verticalOffset;
    public float horizontalOffest;
    public List<GameObject> treasuresList = new List<GameObject>();
    public float treasureDespawnDelay;
    private List<GameObject> roomTreasures = new List<GameObject>();
    public GameObject destroyEffect;
    private bool active = true;
    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject GO = Instantiate(treasuresList[Random.Range(0, treasuresList.Count)],transform.position+(Vector3.down*verticalOffset)+(Vector3.right*(-horizontalOffest+(horizontalOffest*i))),Quaternion.identity,this.transform) as GameObject;
            roomTreasures.Add(GO);
        }
        active = true;
    }


    private void Update()
    {
        if (active)
        {
            for (int i = 0; i < roomTreasures.Count; i++)
            {
                if (roomTreasures[i].activeInHierarchy == false)
                {
                    int chosen = i;
                    Invoke("DeleteTreasure", treasureDespawnDelay);
                    for (int j = 0; j < roomTreasures.Count; j++)
                    {
                        if (j != chosen)
                        {
                            GameObject GO = Instantiate(destroyEffect, roomTreasures[j].gameObject.transform.position, Quaternion.identity);
                            Destroy(GO, 2f);
                        }
                    }
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void DeleteTreastures()
    {
        for (int j = 0; j < roomTreasures.Count; j++)
        {
            roomTreasures[j].SetActive(false);
        }
        gameObject.SetActive(false);
    }

}
