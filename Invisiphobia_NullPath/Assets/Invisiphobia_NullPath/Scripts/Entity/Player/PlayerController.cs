using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public event Action<float> playerRunActionEvent;
    public event Action playerJumpActionEvent;
    public event Action<Vector2> playerMoveActionEvent;

    void Update()
    {
        PlayerMove();
        PlayerRun();
        PlayerJump();
    }


    private void PlayerRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerRunActionEvent.Invoke(-1f);
        }
        else
        {
            playerRunActionEvent.Invoke(1f);
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
        playerMoveActionEvent.Invoke(new Vector2(horizontal,vertical));
    }
}




