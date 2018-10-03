﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShake : MonoBehaviour {

    private Vector3 _originalPos;

    public void Shake(float duration, float amount)
    {
        this.StopAllCoroutines();

        this.StartCoroutine(cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        _originalPos = GetComponent<RectTransform>().localPosition;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {

            GetComponent<RectTransform>().localPosition = _originalPos + Random.insideUnitSphere * amount;
            //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1f);

            duration -= Time.deltaTime;

            yield return null;
        }

        GetComponent<RectTransform>().localPosition = _originalPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Shake(0.1f, 2f);
        }
    }

}
