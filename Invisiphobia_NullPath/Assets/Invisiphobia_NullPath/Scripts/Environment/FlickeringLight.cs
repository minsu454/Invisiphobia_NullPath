using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light light;

    [SerializeField] private float minTime = 0.05f;
    [SerializeField] private float maxTime = 1.0f;

    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;

    private void Start()
    {
        if (light != null)
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

            light.intensity = Random.Range(minIntensity, maxIntensity);

            if (Random.value > 0.7f)
            {
                light.enabled = false;
                yield return new WaitForSeconds(waitTime);
                light.enabled = true;
            }
            yield return new WaitForSeconds(waitTime);
        }

    }
}
