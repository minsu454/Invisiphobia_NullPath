using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy : Prop
{
    [SerializeField] private AudioClip dummyWatch;
    [SerializeField] private float stopFollowDistance = 1f;

    Transform targetTr;
    public bool followWatch = false;

    Camera playerCamera;
    public float fieldOfViewAngle = 55f;
    private bool isSoundPlayed = false;

    public void Start()
    {
        targetTr = EntityManager.Instance.Player.transform;
        playerCamera = Camera.main;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float distanceToPlayer = Vector3.Distance(transform.position, other.transform.position);

            // 거리 조건을 추가하여 플레이어가 멀리 있으면 followWatch 활성화
            if (distanceToPlayer > stopFollowDistance)
            {
                followWatch = true;
            }
            else
            {
                followWatch = false;
            }
        }
    }

    void Update()
    {
        if (!IsChildMeshActive())
        {
            // 자식 Mesh가 비활성화 상태면 실행하지 않음
            return;
        }

        if (followWatchWhenBehindPlayer())
        {
            // 플레이어가 마네킹을 보고 있으면 따라보지 않음
            isSoundPlayed = false;
            return;
        }
        followWatchPlayer();
    }

    bool IsChildMeshActive()
    {
        // 자식 MeshRenderer 중 하나라도 활성화되어 있으면 true 반환
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in meshRenderers)
        {
            if (renderer.enabled)
                return true;
        }
        return false;
    }

    void followWatchPlayer()
    {
        if (!followWatch || targetTr == null)
            return;

        // 사운드가 한 번도 재생되지 않았다면 실행
        if (!isSoundPlayed)
        {
            Managers.Sound.SFX3DPlay(dummyWatch, gameObject.transform, 2f);
            isSoundPlayed = true; // 사운드가 재생되었음을 표시
        }

        Vector3 direction = targetTr.position - transform.position;
        direction.y = 0; // Y축 고정
        transform.rotation = Quaternion.LookRotation(direction);
    }

    bool followWatchWhenBehindPlayer()
    {
        // 마네킹의 위치에서 플레이어 카메라의 방향으로 벡터 계산
        Vector3 directionToDummy = (transform.position - playerCamera.transform.position).normalized;
        float angle = Vector3.Angle(playerCamera.transform.forward, directionToDummy);

        return angle <= fieldOfViewAngle;
    }
}
