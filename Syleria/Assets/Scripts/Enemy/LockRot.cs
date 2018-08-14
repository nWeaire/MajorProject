//--------------------------------------------------------------------------------------
// Purpose: Used on the slime to stop it from rotating.
//
// Description: Locks the objects rotation to 0,0,0.
//              
// Author: Callan Davies
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRot : MonoBehaviour
{
    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update ()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
