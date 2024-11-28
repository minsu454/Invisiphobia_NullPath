using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    public float baseSpeed { get; set; } = 5f;
    public float moveSpeed { get; set; } = 5f;

    private float decelerationTime = 0.3f; // 감속 시간
    private float runSpeed = 1.5f;        // 달리는 속도
    private Vector2 currentSpeed;         // 현재 이동 속도
    //private Vector2 targetSpeed;          // 목표 속도
    private Vector2 speedVelocity;        // 보간용 속도


    [Header("PlayerJump")]
    [SerializeField] private float jumpForce = 0.5f;   // 점프 힘
    [SerializeField] private float gravity = 3f;    // 중력
    private Vector3 velocity;                         // 플레이어의 현재 속도
    [SerializeField] private bool isGrounded = false; // 지면 여부


    [Header("Camera")]
    [SerializeField] private Transform cameraTransform; // 카메라 Transform 참조


    [Header("PlayerStamina")]
    [SerializeField] GameObject stamina;
    public Slider staminaSlider;
    private Coroutine sliderCoroutine;


    private void Start()
    {
        Player.Instance.PlayerController.playerRunActionEvent += OnPlayerRun;
        Player.Instance.PlayerController.playerJumpActionEvent += OnPlayerJump;
        Player.Instance.PlayerController.playerMoveActionEvent += OnPlayerMove;

        //초기 플레이어 MoveMent값
        characterController = GetComponent<CharacterController>();
        currentSpeed = Vector2.zero;
        //targetSpeed = Vector2.zero;
        speedVelocity = Vector2.zero;

        //초기 플레이어 스테미나 값
        if (staminaSlider != null)
        {
            staminaSlider.value = 1f; // 초기값 설정
        }
    }

    private void Update()
    {
        ApplyGravity();
    }

    private void OnPlayerMove(Vector2 vec)
    { 
        // 입력에 따른 목표 속도 설정
        vec *= moveSpeed;


        // 현재 속도 업데이트
        if (Mathf.Abs(vec.x) > 0 || Mathf.Abs(vec.y) > 0)
        {
            // 입력이 있을 때 목표 속도로 설정
            currentSpeed.x = vec.x;
            currentSpeed.y = vec.y;
        }
        else
        {
            // 입력이 없을 때 감속 처리
            currentSpeed.x = Mathf.SmoothDamp(currentSpeed.x, 0, ref speedVelocity.x, decelerationTime);
            currentSpeed.y = Mathf.SmoothDamp(currentSpeed.y, 0, ref speedVelocity.y, decelerationTime);
        }

        // 로컬 좌표 기준으로 이동 방향 계산
        Vector3 moveDirection = (cameraTransform.right * currentSpeed.x + cameraTransform.forward * currentSpeed.y);
        moveDirection.y = 0f; // 수평 이동만 처리

        // 프레임 단위로 이동 거리 계산
        Vector3 finalMove = moveDirection.normalized * currentSpeed.magnitude * Time.deltaTime + new Vector3(0, velocity.y, 0);

        // 이동 적용
        characterController.Move(finalMove);
    }

    private void OnPlayerRun(float value)
    {
        moveSpeed = baseSpeed * runSpeed;
        staminaSlider.value = Mathf.Clamp(staminaSlider.value + value * 0.1f * Time.deltaTime, 0f, 1f);
    }
       
    private void OnPlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce);
            isGrounded = false;
        }
    }
    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            velocity.y -= gravity * Time.deltaTime; // 중력 적용
        }
    }
}
