//--------------------------------------------------------------------------------------
// Purpose: Class to hold variables for panels containing items
//
// Description: Updates image sprite for panel to the item sprite
//
// Author: Nicholas Weaire
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Panel : MonoBehaviour {

    [HideInInspector]public bool m_isUsed = false; // If used up with item
    [HideInInspector]public Sprite m_ItemSprite = null; // Item sprite to display

    private void Update()
    {
        if(m_ItemSprite) // If item sprite is set
        {
            this.GetComponent<Image>().sprite = m_ItemSprite; // Set sprite to item sprite
        }
    }

}
