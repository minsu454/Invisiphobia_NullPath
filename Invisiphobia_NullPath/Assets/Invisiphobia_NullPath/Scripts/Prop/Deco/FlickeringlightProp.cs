using Common.Yield;
using System.Collections;
using UnityEngine;

public class FlickeringlightProp : MonoBehaviour
{
    [SerializeField] GameObject Lights;
    [SerializeField] Light Light;

    [SerializeField] private float minInterval = 0.2f;
    [SerializeField] private float maxInterval = 1.0f;

    private bool isOn = true;

    private void Start()
    {
        StartCoroutine(CoFlicker());
    }

    private IEnumerator CoFlicker()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return YieldCache.WaitForSeconds(waitTime);

            Light.intensity = Random.Range(1f, 5f);
            isOn = !isOn;
            Lights.SetActive(isOn);
        }
    }
}
