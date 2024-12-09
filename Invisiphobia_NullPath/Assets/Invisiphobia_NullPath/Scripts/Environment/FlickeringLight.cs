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

    public GameObject middleJoint;
    public float checkDistance = 5f;
    [SerializeField] TriggerDetector triggerDetector;
    /// <summary>
    /// 기준 거리 내에 있는 오브젝트를 반환
    /// </summary>
    /// <returns>리스트로 반환되는 오브젝트들</returns>

    private void Start()
    {
        triggerDetector.EnterEvent += TriggerEnter;

        if (mylight != null)
        {
            StartCoroutine(CoLightFliker());
        }
    }

    public void TriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            middleJoint.SetActive(false);
        }
    }

    /// <summary>
    /// 전등이 땅과 닿았을 때 호출
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    private void OnCollisionEnter(Collision collision)
    {
        //여기에 땅과 램프 충돌 시 조건 추가
        HandleLampTouchGround(); // 원하는 작업 실행k
    }


    /// <summary>
    /// 전등이 땅에 닿았을 때
    /// </summary>
    private void HandleLampTouchGround()
    {
        //여기에 땅에 떨어졌을 때 전등 깨지는 소리 추가
        mylight.enabled = false;
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
