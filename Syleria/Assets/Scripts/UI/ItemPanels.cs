//--------------------------------------------------------------------------------------
// Purpose: Logic for showing items on the ui
//
// Description:  Displays sprites of current items on ui elements
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanels : MonoBehaviour {

    public GameObject[] m_gItemPanels; // List of item panels to put sprites into

	void Start ()
    {
        for (int i = 0; i < m_gItemPanels.Length; i++) // For all item panels
        {
            m_gItemPanels[i].AddComponent<Panel>(); // Add panel script
            m_gItemPanels[i].SetActive(false);
        }      
	}
    //--------------------------------------------------------------------------------------
    // Finds an open image in Ui and replaces sprite with that of an item
    // Parameters:
    //      item Sprite: Sprite from the item you wish to display on the UI image
    //--------------------------------------------------------------------------------------
    public void AddItem(Sprite itemSprite) 
    {
        for (int i = 0; i < m_gItemPanels.Length; i++) // For all Panels
        {
            if(!m_gItemPanels[i].GetComponent<Panel>().m_isUsed) // Check if being used
            {
                m_gItemPanels[i].SetActive(true);
                m_gItemPanels[i].GetComponent<Panel>().m_ItemSprite = itemSprite; // Change sprite to that of the items
                m_gItemPanels[i].GetComponent<Panel>().m_isUsed = true; // Sets panels to being used so no other item can appear on it
                break;
            }
        }
    }
}
