using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tripolygon.UModelerX.Runtime.IHotspotFilterRule;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float Sensitivity = 20f;  
    
    private float smoothSpeed = 0.1f;      
    private float threshold = 0.1f;          // 마우스 이동의 속도 임계값

    private float camXRot = 0f;             // 상하 회전(x축을 기준으로 회전하게)
    private float camYRot = 0f;             // 좌우 회전(y축을 기준으로 회전하게)
    private Vector3 lastMousePosition;      // 마지막 마우스 위치

    [SerializeField]
    private Transform playerTransform; // 플레이어 오브젝트

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 1.8f, 0f); // 플레이어와 카메라의 상대적인 위치 (예: 눈높이)

    private void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        CameraRotate();
        CameraPosition();
    }

    private void CameraRotate()
    {
        //mouseDelta로 구해야한다. mousePosition x >> 애는 마우스가 해상도 위치를 기준으로함.
        Vector3 mousePosition = Input.mousePosition; //mouseDelta
        Vector3 mouseDelta = mousePosition - lastMousePosition;

        lastMousePosition = mousePosition;

        float mouseSpeed = mouseDelta.magnitude;

        if (mouseSpeed > threshold)
        {
            // 좌우 이동 (Yaw) - 플레이어와 카메라의 회전
            camYRot += mouseDelta.x * Sensitivity * Time.deltaTime;

            // 상하 이동 (Pitch) - 카메라는 상하만 회전, 플레이어는 회전하지 않음
            camXRot -= mouseDelta.y * Sensitivity * Time.deltaTime;
            camXRot = Mathf.Clamp(camXRot, -90f, 90f);

            // 카메라 회전 적용 (상하 회전은 카메라만, 좌우 회전은 카메라와 플레이어가 함께)
            transform.localRotation = Quaternion.Euler(camXRot, camYRot, 0f);

            // 플레이어 회전 적용 (좌우 회전)
            playerTransform.localRotation = Quaternion.Euler(0f, camYRot, 0f);
        }
        else
        {
            // 마우스 속도가 느려지면 부드럽게 감속
            float targetXRotation = Mathf.Clamp(camXRot, -90f, 90f);
            float targetYRotation = camYRot;

            // 부드럽게 회전
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetXRotation, targetYRotation, 0f), smoothSpeed);

            // 플레이어 회전 부드럽게 적용
            playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, Quaternion.Euler(0f, targetYRotation, 0f), smoothSpeed);
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


