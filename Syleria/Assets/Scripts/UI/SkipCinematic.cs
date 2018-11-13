using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SkipCinematic : MonoBehaviour {

    public string sceneName;

    // Update is called once per frame
    void Update () {
        StartGameWithAnyButton();

    }
    public void StartGameWithAnyButton()
    {
        if (Input.GetButtonDown("Interact") || Input.GetButtonDown("Start"))
            SceneManager.LoadScene(sceneName);
    }
}
