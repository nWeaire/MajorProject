//--------------------------------------------------------------------------------------
// Purpose: Keep Hitslash effect on enemy for short duration
// Author: Hamish Smithers
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSlash : MonoBehaviour {

    public Enemy target;
    public SpriteRenderer targetRender;

	// Use this for initialization
	void Start () {
        if (target)
        {
            targetRender = target.GetComponent<SpriteRenderer>();
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (targetRender)
        {
            transform.position = targetRender.bounds.center;
        }
	}
}
