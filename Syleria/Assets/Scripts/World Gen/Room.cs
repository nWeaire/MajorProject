using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public GameObject northDoor;
    public GameObject southDoor;
    public GameObject eastDoor;
    public GameObject westDoor;

    public GameObject northCorridor;
    public GameObject southCorridor;
    public GameObject eastCorridor;
    public GameObject westCorridor;

    public GameObject centre;
    public GameObject shop;
    public GameObject boss;
    
    public bool openNorth;
    public bool openSouth;
    public bool openEast;
    public bool openWest;

    public bool centreOn;
    public bool shopOn;
    public bool bossOn;


    private void Start()
    {
        //openNorth = false;
        //openSouth = false;
        //openEast = false;
        //openWest = false;

        //centreOn = false;
        //shopOn = false;
        //bossOn = false;
    }

    private void Update()
    {
        if (centreOn)
        {
            centre.SetActive(true);
        }
        if (shopOn)
        {
            shop.SetActive(true);
        }
        if (bossOn)
        {
            boss.SetActive(true);
        }

        if (openNorth)
        {
            northDoor.SetActive(false);
            northCorridor.SetActive(true);
        }
        if (openSouth)
        {
            southDoor.SetActive(false);
            southCorridor.SetActive(true);
        }
        if (openEast)
        {
            eastDoor.SetActive(false);
            eastCorridor.SetActive(true);
        }
        if (openWest)
        {
            westDoor.SetActive(false);
            westCorridor.SetActive(true);
        }

    }



}
