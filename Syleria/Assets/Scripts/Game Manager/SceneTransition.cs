//--------------------------------------------------------------------------------------
// Purpose: Transistioning to a new floor in the dungeon
//
// Description:  Turns off and deletes required game objects before rebuilding the entire level
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    public GameObject m_gSceneObjects;
    public WorldGeneration m_gWorldGeneration;
    private GameObject m_aMinimapTiles;
    private void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            m_aMinimapTiles = GameObject.Find("MinimapTiles");
            m_aMinimapTiles.SetActive(false);         
            m_gSceneObjects = GameObject.FindGameObjectWithTag("SceneObjects");
            m_gWorldGeneration = GameObject.FindObjectOfType<WorldGeneration>();
            GameObject player = GameObject.Find("Player");
            GameObject.FindGameObjectWithTag("Companion").transform.localPosition = Vector2.zero;
            player.transform.localPosition = Vector2.zero;
            m_gSceneObjects.SetActive(false);

            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            Destroy(GameObject.FindGameObjectWithTag("A*"));
            for (int i = 0; i < rooms.Length; i++)
            {
                Destroy(rooms[i]);
            }
            m_gWorldGeneration.m_nFloorNum += 1;
            m_gWorldGeneration.CreateWorld();

            Destroy(this.gameObject);
        }
    }
}
