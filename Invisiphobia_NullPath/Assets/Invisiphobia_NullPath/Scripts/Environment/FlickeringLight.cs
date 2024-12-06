using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light mylight;

    [SerializeField] private float minTime = 0.05f;
    [SerializeField] private float maxTime = 1.0f;

    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;

    private void Start()
    {
        if (mylight != null)
        {
            StartCoroutine(CoLightFliker());
        }
        else
        {

        }
    }

    private IEnumerator CoLightFliker()
    {
        while (true)
        {
            float waitTime = Random.Range(minTime, maxTime);

            mylight.intensity = Random.Range(minIntensity, maxIntensity);

            if (Random.value > 0.7f)
            {
                mylight.enabled = false;
                yield return YieldCache.WaitForSeconds(waitTime);
                mylight.enabled = true;
            }
            yield return YieldCache.WaitForSeconds(waitTime);
        }
    }
}
