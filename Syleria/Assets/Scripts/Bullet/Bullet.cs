using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float LifeSpan = 0.5f;
    public float timer = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if (LifeSpan <= timer)
            DestroyObject(this.gameObject);
        transform.Translate(Vector2.up);
	}
}
