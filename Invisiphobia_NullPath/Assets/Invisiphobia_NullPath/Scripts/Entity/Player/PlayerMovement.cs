using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    public Camera playerCamera;

    #region Movement Variables
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    // Internal Variables
    private bool isWalking = false;
    #endregion

    #region Sprint
    //Sprint = 단거리 달리기, 코드에서는 보통 스테미나가 있는 경우가 많음
    public bool isZoomed = false;
    public bool enableSprint = true;
    public float sprintSpeed = 7f;
    public float sprintDuration = 5f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Sprint Bar
    [SerializeField] private Slider staminaBar;
    [SerializeField] private CanvasGroup sprintBarCanvasGroup; // CanvasGroup 추가
    public bool hideBarWhenFull = true;

    // Internal Variables
    public bool isSprinting = false;
    private float sprintRemaining = 1f;
    private bool isBarVisible = true; // Bar 상태를 추적
    private float fadeDuration = 0.5f; // 페이드 전환 시간
    #endregion

    #region Jump

    public bool enableJump = true;
    public float jumpPower = 5f;
    public bool isJumping = false;
    // Internal Variables
    public bool isGrounded = false;

    #endregion

    #region Crouch

    public float speedReduction = .5f;
    public bool holdToCrouch = true;
    [SerializeField] private float crouchHeight = 0.3f;
    [SerializeField] private float crouchSpeed = 5; //앉는 속도
    private Vector3 standingPosition; // 서 있는 상태의 카메라 위치
    private Vector3 crouchingPosition; // 앉았을 때의 카메라 위치
    public bool isCrouched = false;
    // Internal Variables


    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    public void Init(Player player)
    {
        standingPosition = playerCamera.transform.localPosition;
        crouchingPosition = standingPosition - new Vector3(0f, crouchHeight, 0f);

        jointOriginalPos = joint.localPosition;

        sprintRemaining = sprintDuration; // 스태미나 초기화
        if (staminaBar != null)
        {
            staminaBar.maxValue = sprintDuration; // 슬라이더 최대값 설정
            staminaBar.minValue = 0; // 슬라이더 최소값 설정
            staminaBar.value = sprintRemaining; // 현재 스태미나 동기화
        }

        if (sprintBarCanvasGroup != null)
        {
            sprintBarCanvasGroup.alpha = 1; // Bar 초기 상태
        }

        playerCamera = player.CameraController.playerCamera;
        isZoomed = player.CameraController.isZoomed;

        player.PlayerController.playerMoveActionEvent += Move;
        player.PlayerController.playerSprintActionEvent += Sprint;
        player.PlayerController.playerJumpActionEvent += Jump;
        player.PlayerController.playerCrouchActionEvent += Crouch;
    }

    private void Update()
    {
        CheckGround();

        if (enableHeadBob)
        {
            HeadBob();
        }

        if (!isSprinting)
        {
            sprintRemaining += 0.1f * Time.deltaTime * 10;
            sprintRemaining = Mathf.Clamp(sprintRemaining, 0, sprintDuration);

            if (sprintRemaining >= sprintDuration && hideBarWhenFull)
            {
                ShowSprintBar(false); // Bar 숨기기
            }
        }

        if (sprintBarCanvasGroup != null && sprintBarCanvasGroup.alpha > 0)
        {
            staminaBar.value = sprintRemaining;
        }
    }

    /// <summary>
    /// 플레이어 움직이는 함수
    /// </summary>
    private void Move(Vector3 targetVelocity)
    {
        if (playerCanMove)
        {
            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            // All movement calculations shile sprint is active
            if (enableSprint && sprintRemaining > 0f)
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                // Player is only moving when valocity change != 0
                // Makes sure fov change only happens during movement
                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;
                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            // All movement calculations while walking
            else
            {
                isSprinting = false;

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }
    }

    /// <summary>
    /// 플레이어 달리기 함수
    /// </summary>
    private void Sprint()
    {
        if (enableSprint)
        {
            ShowSprintBar(true); // Bar 표시
            isSprinting = true;

            isZoomed = false;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);

            sprintRemaining -= 0.1f * Time.deltaTime * 10;
            sprintRemaining = Mathf.Clamp(sprintRemaining, 0, sprintDuration);

            if (sprintRemaining <= 0)
            {
                isSprinting = false;
            }
        }
    }

    /// <summary>
    /// Sprint Bar 표시/숨기기
    /// </summary>
    private void ShowSprintBar(bool show)
    {
        if (sprintBarCanvasGroup == null || isBarVisible == show) return;

        isBarVisible = show;
        StopAllCoroutines(); // 중복 호출 방지
        StartCoroutine(FadeBar(show));
    }

    /// <summary>
    /// Sprint Bar 페이드 효과
    /// </summary>
    private IEnumerator FadeBar(bool show)
    {
        float startAlpha = sprintBarCanvasGroup.alpha;
        float targetAlpha = show ? 1 : 0;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            sprintBarCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        sprintBarCanvasGroup.alpha = targetAlpha;
    }
    // Sets isGrounded based on a raycast sent straigth down from the player object

    /// <summary>
    /// 바닥인지 확인하는 함수
    /// </summary>
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// 플레이어 점프하는 함수
    /// </summary>
    private void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isJumping = false;
        }
    }

    /// <summary>
    /// 플레이어 웅크리는 함수
    /// </summary>
    private void Crouch(bool isCrouched)
    {
        // 목표 위치 설정 (앉거나 서 있는 상태)
        Vector3 targetPosition = isCrouched ? crouchingPosition : standingPosition;

        this.isCrouched = isCrouched;
        // 카메라의 위치를 부드럽게 전환
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetPosition, Time.deltaTime * crouchSpeed);
    }

    /// <summary>
    /// 플레이어가 움직일 때(걸을 때, 달릴 때) 머리를 움직이는 함수
    /// </summary>
    private void HeadBob()
    {
        if (isWalking)
        {
            // Calculates HeadBob speed during sprint
            if (isSprinting)
            {
                timer += Time.deltaTime * (bobSpeed + sprintSpeed);
            }
            // Calculates HeadBob speed during crouched movement
            else if (isCrouched)
            {
                timer += Time.deltaTime * (bobSpeed * speedReduction);
            }
            // Calculates HeadBob speed during walking
            else
            {
                timer += Time.deltaTime * bobSpeed;
            }
            // Applies HeadBob movement
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
        }
        else
        {
            // Resets when play stops moving
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }
}

