using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenShake : MonoBehaviour {

    private Vector3 _originalPos;

    public void Shake(float duration, float amount)
    {
        this.StopAllCoroutines();

        this.StartCoroutine(cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            _originalPos = this.transform.localPosition;
            this.transform.localPosition = _originalPos + Random.insideUnitSphere * amount;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1f);

            duration -= Time.deltaTime;

            yield return null;
        }

        this.transform.localPosition = _originalPos;
    }

    
}
