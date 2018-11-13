//--------------------------------------------------------------------------------------
// Purpose: All over arching game functions
//
// Description:  Game Management including ending, restarting and quitting game
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GM : MonoBehaviour {

    [SerializeField] private GameObject m_gPlayer; // Reference to player gameObject
    private Player m_Player; // Reference to player script
    public bool isPaused = false; // Bool for pausing game
    public bool isGameOver = false; // Bool for game over
    public GameObject m_gPauseMenu; // Pause menu ui
    public GameObject m_EndGameUI; // End game ui
    public GameObject m_runTimeObjects; // Run time objects
    private float timer = 0; // Timer for end game ui fade in
    public GameObject m_DeathAnimation = null; // Death animation to play on game over
    private bool m_isDeathAnimation = false; // Is death animation spawned
    private bool m_bRunTimeUI = true;

    void Start ()
    {
        m_Player = m_gPlayer.GetComponent<Player>(); // Finding player script
	}
	
	void Update ()
    {
        EndGame(); // Check for GameOver State
        if (Input.GetKey("escape")) // If escape key is pressed
        {
            Application.Quit(); // Quits application on escape key press
        }
        if(Input.GetButtonDown("Start")) // If start button is hit
        {
            if(isPaused && isGameOver == false) // unPause game as long as game isn't over and is paused
            {
                Unpause(); // Unpauses game
            }
            else
            {
                Pause(); // If game is unpaused, pause game
            }
        }
    }

    //------------------------------------------------------------
    // Pauses game, Sets time scale to 0
    // Sets isPaused bool to true
    // Sets Pause UI to active
    //------------------------------------------------------------
    public void Pause()
    {
        //m_runTimeObjects.SetActive(false);
        m_gPauseMenu.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject = GameObject.Find("Resume Button");
        Time.timeScale = 0;
        isPaused = true;
    }

    //------------------------------------------------------------
    // UnPauses game, Sets time scale to 1
    // Sets isPaused bool to false
    // Sets Pause UI to inactive
    //------------------------------------------------------------
    public void Unpause()
    {
        //m_runTimeObjects.SetActive(true);
        m_gPauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    //------------------------------------------------------------
    // Checks if players health is 0 or less
    // Turns on end game UI, sets death animation render texture to true
    // Fades to black and then fades in UI on timers
    //------------------------------------------------------------
    public void EndGame()
    { 
        if (m_Player.GetCurrentHealth() <= 0) // If player's health is 0 or less
        {
            GameObject background = GameObject.Find("End Background"); // Find End Game UI
            if(m_bRunTimeUI) // If RunTimeUI is true
            {
                GameObject.FindGameObjectWithTag("RunTime UI").SetActive(false); // Turn off runTimeUI
                m_bRunTimeUI = false; // Set run time ui to false
            }
            timer += 0.05f; // Starts timer
            background.GetComponent<Image>().color = new Color(0, 0, 0, timer); // Scales background colours opactity based on timer
            if (timer >= 1) // When timer reaches one secend and background is pure black
            {
                if (!m_isDeathAnimation) // If Death Animation false
                {
                    // Instantiates death animation in front of render texture camera
                    Instantiate(m_DeathAnimation, GameObject.FindGameObjectWithTag("DeathAnimation").transform.position + (Vector3.forward * 4), Quaternion.identity, GameObject.FindGameObjectWithTag("DeathAnimation").transform);
                    m_isDeathAnimation = true; // Death animation is true
                }
                m_EndGameUI.SetActive(true); // End game ui is set to active
                GameObject.Find("EventSystem").GetComponent<EventSystem>().firstSelectedGameObject = GameObject.Find("Restart Button");
                Image[] images = m_EndGameUI.GetComponentsInChildren<Image>(); // Finds all images in endgameUI
                for (int i = 0; i < images.Length; i++) // For all images
                {
                    images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, timer - 4f); // Scales opacity based on timer
                }
                Text[] texts = m_EndGameUI.GetComponentsInChildren<Text>(); // Finds all text
                for (int i = 0; i < texts.Length; i++) // For all text
                {
                    texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, timer - 4f); // Scales text opacity based on timer
                }

            }
            if (timer > 4) // If timer is greater then 4
            {
                Time.timeScale = 0; // Timescale is set to 0
                isPaused = true; // Game is paused
                isGameOver = true; // Game is over
            }
        }
    }
    
    //------------------------------------------------------------
    // Quits applicaton
    //------------------------------------------------------------
    public void QuitGame()
    {
        Application.Quit();
    }

    //------------------------------------------------------------
    // Reloads current scene
    //------------------------------------------------------------
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
