using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour {

    public string sceneName;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartGameWithAnyButton()
    {
        if(Input.GetButtonDown("Interact") || Input.GetButtonDown("Start"))
        SceneManager.LoadScene(sceneName);
    }

}
