using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy2 : Prop
{
    [SerializeField]
    private Rigidbody rb;
    private bool isFallen = false;    // 마네킹이 이미 쓰러졌는지 체크
    public float forceMagnitude = 5f; // 마네킹이 쓰러질 때의 힘 크기
    [SerializeField] private AudioClip fallClip;

    void Start()
    {
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFallen)
        {
            isFallen = true; // 한 번만 쓰러지도록 설정
            rb.isKinematic = false; // 물리 연산 활성화

            Managers.Sound.SFX3DPlay(fallClip, transform);
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * forceMagnitude, ForceMode.Impulse);
        }
    }
}
