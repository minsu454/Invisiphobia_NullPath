using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy2 : Prop
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float dummyForce = 2f;
    private bool isFallen = false;    // 마네킹이 이미 쓰러졌는지 체크
    public float forceMagnitude = 5f; // 마네킹이 쓰러질 때의 힘 크기

    [SerializeField] private AudioClip fallClip;
    [SerializeField] MeshRenderer meshRenderer;
    public bool isPlayerInTrigger = false; // Player가 Trigger에 들어왔는지 여부 확인
    void Start()
    {
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    void Update()
    {
        if (meshRenderer.enabled && isPlayerInTrigger)
        {
            TryFall();
        }
    }

    void TryFall()
    {
        if (!isFallen) // 한 번만 쓰러지도록 설정
        {
            isFallen = true;
            rb.isKinematic = false; // 물리 연산 활성화

            // 로컬 정면 방향을 기준으로 힘 추가
            Vector3 forwardDirection = transform.forward * dummyForce + new Vector3(0f, 0.5f, 0f);
            rb.AddForce(forwardDirection.normalized * forceMagnitude, ForceMode.Impulse);

            Managers.Sound.SFX3DPlay(fallClip, transform);
        }
    }
}
