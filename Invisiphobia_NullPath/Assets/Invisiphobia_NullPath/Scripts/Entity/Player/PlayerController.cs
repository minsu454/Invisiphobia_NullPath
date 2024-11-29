using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public event Action<bool> playerRunActionEvent;
    public event Action playerJumpActionEvent;
    public event Action<Vector2> playerMoveActionEvent;
    private bool isRunning = false;

    void Update()
    {
        PlayerMove();
        PlayerRun();
        PlayerJump();
    }


    private void PlayerRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }

        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            playerRunActionEvent.Invoke(false);
        }

        if(isRunning)
        {
            playerRunActionEvent.Invoke(true);
        }
    }

    private void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerJumpActionEvent.Invoke();
        }
    }

    private void PlayerMove()
    {
        // 입력 값
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        playerMoveActionEvent.Invoke(new Vector2(horizontal, vertical));
    }
}



