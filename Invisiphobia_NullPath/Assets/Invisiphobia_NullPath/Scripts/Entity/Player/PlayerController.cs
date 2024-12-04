using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode sprintKey;
    public KeyCode jumpKey;
    public KeyCode crouchKey;
    public KeyCode interactKey;

    public event Action<Vector3> playerMoveActionEvent;
    public event Action playerSprintActionEvent;
    public event Action playerJumpActionEvent;
    public event Action<bool> playerCrouchActionEvent;
    public event Action playerInteractActionEvent;

    public bool isSprinting = false;
    public bool isCrouched = false;

    private void Start()
    {
        sprintKey = Player.Instance.PlayerMovement.sprintKey;
        jumpKey = Player.Instance.PlayerMovement.jumpKey;
        crouchKey = Player.Instance.PlayerMovement.crouchKey;
        interactKey = Player.Instance.PlayerInteract.interactKey;
    }
    void Update()
    {
        OnPlayerSprint();
        OnPlayerJump();
        OnPlayerCrouch();
        OnPlayerInteract();
    }

    private void FixedUpdate()
    {
        OnPlayerMove();
    }

    private void OnPlayerInteract()
    {
        if(Input.GetKeyDown(interactKey))
        {
            playerInteractActionEvent.Invoke();
        }
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
        if (Input.GetKeyDown(jumpKey))
        {
            playerJumpActionEvent.Invoke();
        }
    }

    private void OnPlayerMove()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        playerMoveActionEvent.Invoke(targetVelocity);
    }

    private void OnPlayerCrouch()
    {
       if (Input.GetKeyDown(crouchKey) && !isCrouched)
       {
           isCrouched = true;
       }
       else if (Input.GetKeyUp(crouchKey) && isCrouched)
       {
           isCrouched = false;
       }
       playerCrouchActionEvent.Invoke(isCrouched);
    }

}



