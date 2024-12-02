using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode sprintKey;
    public KeyCode jumpKey;
    public KeyCode crouchKey;

    public event Action playerMoveActionEvent;
    public event Action playerSprintActionEvent;
    public event Action playerJumpActionEvent;
    public event Action playerCrouchActionEvent;

    private bool isSprinting = false;
    public bool enableJump = true;
    private bool isGrounded = false;
    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public bool isCrouched = false;

    private void Start()
    {
        sprintKey = Player.Instance.PlayerMovement.sprintKey;
        jumpKey = Player.Instance.PlayerMovement.jumpKey;
        crouchKey = Player.Instance.PlayerMovement.crouchKey;

        isSprinting = Player.Instance.PlayerMovement.isSprinting;
        enableJump = Player.Instance.PlayerMovement.enableJump;
        isGrounded = Player.Instance.PlayerMovement.isGrounded;
        enableCrouch = Player.Instance.PlayerMovement.enableCrouch;
        holdToCrouch = Player.Instance.PlayerMovement.holdToCrouch;
        isCrouched = Player.Instance.PlayerMovement.isCrouched;
    }
    void Update()
    {
        OnPlayerMove();
        OnPlayerSprint();
        OnPlayerJump();
        OnPlayerCrouch();
    }


    private void OnPlayerSprint()
    {
        if (Input.GetKeyDown(sprintKey))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(sprintKey))
        {
            isSprinting = false;
        }

        if(isSprinting)
        {
            playerSprintActionEvent.Invoke();
        }
    }

    private void OnPlayerJump()
    {
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            playerJumpActionEvent.Invoke();
        }
    }

    private void OnPlayerMove()
    {
        playerMoveActionEvent.Invoke();
    }

    private void OnPlayerCrouch()
    {
        if (enableCrouch)
        {
            //holdToCrouch = 숙일 때 키를 꾹 눌러서 숙일지 여부
            if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
            {
                playerCrouchActionEvent.Invoke();
            }

            if (Input.GetKeyDown(crouchKey) && holdToCrouch)
            {
                isCrouched = true;
                playerCrouchActionEvent.Invoke();
            }
            else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
            {
                isCrouched = false;
                playerCrouchActionEvent.Invoke();
            }
        }
    }

}



