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

    [HideInInspector]public bool m_isUsed = false;
    [HideInInspector]public Sprite m_ItemSprite = null;

    private void Update()
    {
        if(m_ItemSprite)
        {
            this.GetComponent<Image>().sprite = m_ItemSprite;
        }
    }

}
