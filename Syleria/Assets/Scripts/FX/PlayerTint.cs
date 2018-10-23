using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTint : MonoBehaviour {

    public SpriteRenderer playerSprite;
    private SpriteMask mask;

	// Use this for initialization
	void Start () {
        playerSprite = GetComponentInParent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        mask.sprite = playerSprite.sprite;
	}
}
