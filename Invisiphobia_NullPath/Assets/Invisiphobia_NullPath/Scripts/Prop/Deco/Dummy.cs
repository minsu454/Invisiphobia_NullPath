using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Dummy : Prop
{
    Transform targetTr;
    public bool followWatch = false;

    Camera playerCamera;
    public float fieldOfViewAngle = 55f;
    public void Start()
    {
        targetTr = EntityManager.Instance.Player.transform;
        playerCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            followWatch = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            followWatch = false;
    }

    void Update()
    {
        if (followWatchWhenBehindPlayer())
        {
            // 플레이어가 마네킹을 보고 있으면 따라보지 않음
            return;
        }
        followWatchPlayer();
    }

    void followWatchPlayer()
    {
        if (!followWatch || targetTr == null)
            return;

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
