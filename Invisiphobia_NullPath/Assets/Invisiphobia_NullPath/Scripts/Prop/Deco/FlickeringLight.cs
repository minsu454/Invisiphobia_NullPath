using Common.Yield;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light mylight;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;

    [SerializeField] private AudioClip filckClip;
    [SerializeField] private AudioClip shakeClip;
    //[SerializeField] private AudioClip fractureclip;

   public GameObject middleJoint;
    public float checkDistance = 5f;
    [SerializeField] TriggerDetector triggerDetector;

    [SerializeField] private Rigidbody lampRigidbody; // 전등의 Rigidbody
    [SerializeField] private float forceAmount = 10f; // 가할 힘의 크기

    private Coroutine coLightFliker;

    private bool isPlay;

    /// <summary>
    /// 기준 거리 내에 있는 오브젝트를 반환
    /// </summary>
    /// <returns>리스트로 반환되는 오브젝트들</returns>
    private void Start()
    {
        triggerDetector.EnterEvent += TriggerEnter;
        triggerDetector.ExitEvent += TriggerExit;
    }

    private void TriggerEnter(Collider other)
    {
        if (isPlay)
            return;

        if(other.CompareTag("Player"))
        {
            middleJoint.SetActive(false);
            if (lampRigidbody != null)
            {
                Vector3 downwardForce = Vector3.down * forceAmount;
                lampRigidbody.AddForce(downwardForce, ForceMode.Force); // ForceMode를 필요에 따라 조정
            }

            if (mylight != null)
            {
                coLightFliker = StartCoroutine(CoLightFliker());
            }

            isPlay = true;
        }

    }

    private void TriggerExit(Collider other)
    {
        if(coLightFliker != null)
            StopCoroutine(coLightFliker);
    }

    /// <summary>
    /// 전등이 땅과 닿았을 때 호출
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.layer == LayerMask.NameToLayer("Ground"))
        HandleLampTouchGround(); // 원하는 작업 실행
    }


    /// <summary>
    /// 전등이 땅에 닿았을 때
    /// </summary>
    private void HandleLampTouchGround()
    {
        //Managers.Sound.SFX3DPlay(fractureclip, transform);
        mylight.enabled = false;
    }

    private IEnumerator CoLightFliker()
    {
        while (true)
        {
            mylight.intensity = Random.Range(minIntensity, maxIntensity);

            if (Random.value > 0.7f)
            {
                mylight.enabled = false;
                yield return YieldCache.WaitForSeconds(waitTime);
                Managers.Sound.SFX3DPlay(filckClip, transform);
                mylight.enabled = true;
            }
            yield return YieldCache.WaitForSeconds(waitTime * 2); // 실행 간격 늘리기
            Managers.Sound.SFX3DPlay(shakeClip, transform, 0.7f);
        }
    }
}
