using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode sprintKey;
    public KeyCode jumpKey;
    public KeyCode zoomKey;
    public KeyCode crouchKey;

    public Vector3 targetVelocity;

    public event Action playerRunActionEvent;
    public event Action playerJumpActionEvent;
    public event Action playerMoveActionEvent;
    private bool isRunning = false;

    private void Start()
    {
        sprintKey = Player.Instance.PlayerMovement.sprintKey;
        jumpKey = Player.Instance.PlayerMovement.jumpKey;
        zoomKey = Player.Instance.CameraController.zoomKey;
        crouchKey = Player.Instance.PlayerMovement.crouchKey;
    }
    void Update()
    {
        OnPlayerMove();
        OnPlayerSprint();
        OnPlayerJump();
        OnPlayerZoom();
        OnPlayerCrouch();
    }


    private void OnPlayerSprint()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isRunning = true;
        }

        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            playerRunActionEvent.Invoke();
        }

        if(isRunning)
        {
            playerRunActionEvent.Invoke();
        }
    }

    private void OnPlayerJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerJumpActionEvent.Invoke();
        }
    }

    private void OnPlayerMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            playerMoveActionEvent.Invoke();
        }
    }

    private void OnPlayerZoom()
    {

    }

    private void OnPlayerCrouch()
    {

    }
}



