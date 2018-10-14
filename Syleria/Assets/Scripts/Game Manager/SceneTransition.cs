using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    public GameObject m_gSceneObjects;
    public GameObject m_gWorldGeneration;
    private void Start()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            GameObject player = GameObject.Find("Player");
            player.transform.localPosition = Vector2.zero;
            m_gSceneObjects.SetActive(false);

            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            Destroy(GameObject.FindGameObjectWithTag("A*"));
            for (int i = 0; i < rooms.Length; i++)
            {
                Destroy(rooms[i]);
            }
            m_gWorldGeneration.GetComponent<WorldGeneration>().CreateWorld();
            Destroy(this.gameObject);
        }
    }
}
