using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerController : Entity
{
    //애를 쓰면 rigidbody에서 구현해야 하는 중력, 물리작용을 직접 구현해야됨
    [SerializeField] private CharacterController characterController;

    [Header("PlayerSpeed")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;

    private float decelerationTime = 0.3f; // 감속 시간   

    private float runSpeed = 1.5f;        // 달리는 속도
    private Vector2 currentSpeed;         // 현재 이동 속도 
    private Vector2 targetSpeed;          // 목표 속도 
    private Vector2 speedVelocity;        // 보간용 속도


    [Header("PlayerJump")]
    [SerializeField] private float jumpForce = 2f;   // 점프 힘
    [SerializeField] private float gravity = 9.8f;    // 중력
    private Vector3 velocity;                         // 플레이어의 현재 속도
    [SerializeField] private bool isGrounded = false; // 지면 여부



    void Start()
    {
        //초기화 작업
        characterController = GetComponent<CharacterController>();
        currentSpeed = Vector2.zero;
        targetSpeed = Vector2.zero;
        speedVelocity = Vector2.zero;
    }

    void Update()
    {
        PlayerRun();
        PlayerJump();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        PlayerMove();
    }

    private void PlayerMove()
    {
        float Vertical = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");

        targetSpeed.x = Horizontal * moveSpeed;
        targetSpeed.y = Vertical * moveSpeed;

        // Wasd 누르고 있을 떄(0이 아닐 경우) 
        if (Vertical != 0 || Horizontal != 0)
        {
            currentSpeed.x = targetSpeed.x;
            currentSpeed.y = targetSpeed.y;
        }
        else
        {
            // Wasd 뗐을 경우(0이 됬을 경우) 부드럽게 감속 처리
            currentSpeed.x = Mathf.SmoothDamp(currentSpeed.x, 0, ref speedVelocity.x, decelerationTime);
            currentSpeed.y = Mathf.SmoothDamp(currentSpeed.y, 0, ref speedVelocity.y, decelerationTime);
        }
        // 로컬 좌표를 기준으로 이동 방향 계산
        Vector3 moveDirection = (transform.right * currentSpeed.x + transform.forward * currentSpeed.y).normalized;


        // 최종 플레이어 이동  코드
        Vector3 move = new Vector3(currentSpeed.x, velocity.y, currentSpeed.y) * Time.deltaTime;
        characterController.Move(move);
    }

    private void PlayerRun()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = baseSpeed * runSpeed;
        }
        else
        {
            moveSpeed = baseSpeed;
        }
    }

    private void PlayerJump()
    {
        if(Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity); // 초기 점프 속도 계산
            isGrounded = false;
        }
    }

    private void ApplyGravity()
    {
        // 지면 체크
        if (characterController.isGrounded == true)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        velocity.y -= gravity * Time.deltaTime; // 중력 적용
    }
}



