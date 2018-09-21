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
public class GM : MonoBehaviour {

    [SerializeField] private GameObject m_gPlayer;
    private Player m_Player;

    public GameObject m_EndGameUI;
    public GameObject m_runTimeObjects;
	// Use this for initialization
	void Start ()
    {
        m_Player = m_gPlayer.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine("EndGame");
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if(Input.GetKey("p"))
        {
            m_Player.SetCurrentHealth(0);
        }
    }

    public IEnumerator EndGame()
    {
        if(m_Player.GetCurrentHealth() <= 0)
        {
            m_EndGameUI.SetActive(true);
            Time.timeScale = 0;
        }
        yield return new WaitForSeconds(.3f);
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
