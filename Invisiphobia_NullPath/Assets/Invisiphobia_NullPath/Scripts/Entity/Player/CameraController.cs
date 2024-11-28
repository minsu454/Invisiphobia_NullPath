using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tripolygon.UModelerX.Runtime.IHotspotFilterRule;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float Sensitivity = 1f;  
    
    private float smoothSpeed = 0.1f;      

    private float camXRot = 0f;             // 상하 회전(x축을 기준으로 회전하게)
    private float camYRot = 0f;             // 좌우 회전(y축을 기준으로 회전하게)

    [SerializeField]
    private Transform playerTransform; // 플레이어 오브젝트

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 1.8f, 0f); // 플레이어와 카메라의 상대적인 위치 (예: 눈높이)

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        CameraRotate();
        CameraPosition();
    }

    private void CameraRotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X"); // 마우스 좌우 움직임
        float mouseY = Input.GetAxisRaw("Mouse Y"); // 마우스 상하 움직임

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f) // 입력이 있을 때만 반응
        {
            // 좌우 이동 (Yaw) - 플레이어와 카메라의 회전
            camYRot += mouseX * Sensitivity;

            // 상하 이동 (Pitch) - 카메라는 상하만 회전, 플레이어는 회전하지 않음
            camXRot -= mouseY * Sensitivity;
            camXRot = Mathf.Clamp(camXRot, -90f, 90f);

            // 부드럽게 감속 로직 (회전 보간)
            camYRot = Mathf.Lerp(camYRot, camYRot, smoothSpeed);
            camXRot = Mathf.Lerp(camXRot, camXRot, smoothSpeed);

            // 카메라 회전 적용 (상하 회전은 카메라만, 좌우 회전은 카메라와 플레이어가 함께)
            transform.localRotation = Quaternion.Euler(camXRot, camYRot, 0f);

            // 플레이어 회전 적용 (좌우 회전)
            playerTransform.localRotation = Quaternion.Euler(0f, camYRot, 0f);
        }
    }

    private void CameraPosition()
    {
        // 카메라를 플레이어의 위치로 업데이트
        // 플레이어의 위치 + 오프셋 위치를 계산하여 카메라 위치 설정
        Vector3 targetPosition = playerTransform.position + offset;

        // 카메라 위치를 부드럽게 이동 (Smooth Follow)
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}


