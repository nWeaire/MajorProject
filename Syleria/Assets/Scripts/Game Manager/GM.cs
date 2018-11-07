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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GM : MonoBehaviour {

    [SerializeField] private GameObject m_gPlayer;
    private Player m_Player;
    public bool isPaused = false;
    public bool isGameOver = false;
    public GameObject m_gPauseMenu;
    public GameObject m_EndGameUI;
    public GameObject m_runTimeObjects;
    private float timer = 0;
    public GameObject m_DeathAnimation = null;
    private bool m_isDeathAnimation = false;
    // Use this for initialization
    void Start ()
    {
        m_Player = m_gPlayer.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        EndGame();
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if(Input.GetButtonDown("Start"))
        {
            if(isPaused && isGameOver == false)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        //m_runTimeObjects.SetActive(false);
        m_EndGameUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Unpause()
    {
        //m_runTimeObjects.SetActive(true);
        m_EndGameUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void EndGame()
    {
        if (m_Player.GetCurrentHealth() <= 0)
        {
            GameObject background = GameObject.Find("End Background");
            timer += 0.05f;
            background.GetComponent<Image>().color = new Color(0, 0, 0, timer);
            if (timer >= 1)
            {
                if (!m_isDeathAnimation)
                {
                    Instantiate(m_DeathAnimation, GameObject.FindGameObjectWithTag("DeathAnimation").transform.position + (Vector3.forward * 4), Quaternion.identity, GameObject.FindGameObjectWithTag("DeathAnimation").transform);
                    m_isDeathAnimation = true;
                }
                m_EndGameUI.SetActive(true);
                Image[] images = m_EndGameUI.GetComponentsInChildren<Image>();
                for (int i = 0; i < images.Length; i++)
                {
                    images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, timer - 4f);
                }
                Text[] texts = m_EndGameUI.GetComponentsInChildren<Text>();
                for (int i = 0; i < texts.Length; i++)
                {
                    texts[i].color = new Color(texts[i].color.r, texts[i].color.g, texts[i].color.b, timer - 4f);
                }

            }
            if (timer > 4)
            {
                Time.timeScale = 0;
                isPaused = true;
                isGameOver = true;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
