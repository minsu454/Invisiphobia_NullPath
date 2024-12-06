using Common.Yield;
using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light mylight;
    [SerializeField] private Transform lampTransform;
    [SerializeField] private Transform[] cylinders; // 두 개 이상의 실린더를 배열로 설정
    [SerializeField] private float minTime = 0.05f;
    [SerializeField] private float maxTime = 1.0f;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float tiltAngle = 30f;       // 초기 기울어짐 각도
    [SerializeField] private float tiltSpeed = 2f;       // 기울어지는 속도
    private bool isTilting = false;

    [SerializeField] private float shakeAmount = 5f;     // 흔들림 각도
    [SerializeField] private float shakeSpeed = 5f;      // 흔들림 속도
    [SerializeField] private float shakeDuration = 2f;   // 흔들림 지속 시간
    private bool isShaking = false;

    private Coroutine shakeCoroutine;

    private void Start()
    {
        if (cylinders == null || cylinders.Length == 0)
        {
            Debug.LogError("Cylinders are not assigned.");
            return;
        }

        CoTiltAndShake();

        if (mylight != null)
        {
            StartCoroutine(CoLightFliker());
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

    public void CoTiltAndShake()
    {
        if (!isTilting && !isShaking)
        {
            bool tiltRight = Random.Range(0, 2) == 0; // 랜덤 방향 결정
            int cylinderIndex = Random.Range(0, cylinders.Length); // 랜덤으로 실린더 선택
            StartCoroutine(CoTiltLamp(tiltRight, cylinders[cylinderIndex])); // 선택된 실린더 전달
        }
    }

    private IEnumerator CoTiltLamp(bool tiltRight, Transform cylinder)
    {
        isTilting = true;

        // 실린더의 아래 끝 위치 계산 (기준점을 아래 끝으로 설정)
        Vector3 cylinderBottomPosition = cylinder.position - (cylinder.up * (cylinder.localScale.y / 2));

        float targetAngle = tiltRight ? tiltAngle : -tiltAngle;
        float elapsedTime = 0f;
        float startAngle = 0f;

        // 1. 기울어지는 동작
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * tiltSpeed;
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime);

            // 전등의 기준을 cylinder의 아래 끝으로 하고, 그 방향으로 회전하도록 설정
            lampTransform.position = cylinderBottomPosition;
            lampTransform.rotation = Quaternion.Euler(0f, 0f, currentAngle);

            yield return null;
        }

        // 기울어진 상태 유지
        lampTransform.rotation = Quaternion.Euler(0f, 0f, targetAngle);

        isTilting = false;

        // 2. 흔들림 호출
        StartCoroutine(CoOnceShakeLamp(targetAngle));
    }

    private IEnumerator CoOnceShakeLamp(float baseAngle)
    {
        isShaking = true;
        float elapsedTime = 0f;

        // 흔들림 동작
        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float offset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            lampTransform.localRotation = Quaternion.Euler(0f, 0f, baseAngle + offset);
            yield return null;
        }

        // 흔들림 종료 후 원래 각도로 고정
        lampTransform.localRotation = Quaternion.Euler(0f, 0f, baseAngle);
        isShaking = false;
    }

    private IEnumerator CoRepeatShakeLamp(float baseAngle)
    {
        isShaking = true;

        while (true)
        {
            float offset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            lampTransform.localRotation = Quaternion.Euler(0f, 0f, baseAngle + offset);
            yield return null;
        }
    }

    public void StartShaking(float baseAngle)
    {
        if (!isShaking)
        {
            shakeCoroutine = StartCoroutine(CoRepeatShakeLamp(baseAngle));
        }
    }

    public void StopShaking()
    {
        if (isShaking)
        {
            StopCoroutine(shakeCoroutine);
            isShaking = false;
        }
    }
}
