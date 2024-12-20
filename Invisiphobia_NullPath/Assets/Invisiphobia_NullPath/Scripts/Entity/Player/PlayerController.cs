using Common.Event;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    private KeyCode crouchKey = KeyCode.LeftControl;
    private KeyCode interactKey = KeyCode.E;
    private KeyCode tabletKey = KeyCode.Tab;
    private KeyCode putdownKey = KeyCode.Q;

    public event Action<Vector3> playerMoveActionEvent;
    public event Action playerSprintActionEvent;
    public event Action playerJumpActionEvent;
    public event Action<bool> playerCrouchActionEvent;
    public event Action playerInteractActionEvent;
    public event Action playerTabletActionEvent;
    public event Action playerZoomClickActionEvent;
    public event Action playerClickActionEvent;
    public event Action<int> tabletSwitchActionEvent;
    public event Action playerPutDownActionEvent;

    public bool isSprinting = false;
    public bool isCrouched = false;
    public bool isHoldRightmouse = false;

    private readonly KeyCode[] alphaKeyArr = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2,
    };
    private const int alphaKeyNum = (int)KeyCode.Alpha1;

    private bool useInput = true;


    public void Init(Player player)
    {
        EventManager.Subscribe(GameEventType.GameOver, UseInput);
    }

    void Update()
    {
        if (!useInput)
            return;

        OnPlayerSprint();
        OnPlayerJump();
        OnPlayerCrouch();
        OnPlayerInteract();
        OnPlayerTablet();
        OnPlayerClick();
        OnZoomClick();
        OnTabletSwitch();
        OnPlayerPutDown();
    }

    private void FixedUpdate()
    {
        if (!useInput)
            return;

        OnPlayerMove();
    }

    private void OnTabletSwitch()
    {
        foreach (KeyCode keyCode in alphaKeyArr)
        {
            if(Input.GetKeyDown(keyCode))
            {
                tabletSwitchActionEvent.Invoke((int)keyCode - alphaKeyNum);
            }
        }
    }
    private void OnPlayerInteract()
    {
        if(Input.GetKeyDown(interactKey))
        {
            playerInteractActionEvent.Invoke();
        }
    }

    private void OnPlayerTablet()
    {
        if(Input.GetKeyDown(tabletKey))
        {
            playerTabletActionEvent.Invoke();
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
        Vector3 targetVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
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

    /// <summary>
    /// 줌 클릭 함수
    /// </summary>
    private void OnZoomClick()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            isHoldRightmouse = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            isHoldRightmouse = false;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && isHoldRightmouse == true)
        {
            playerZoomClickActionEvent?.Invoke();
        }
    }

    /// <summary>
    /// 마우스 클릭 함수
    /// </summary>
    private void OnPlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isHoldRightmouse)
        {
            playerClickActionEvent?.Invoke();
        }
    }

    private void OnPlayerPutDown()
    {
        if(Input.GetKeyDown(putdownKey))
        {
            playerPutDownActionEvent.Invoke();
        }
    }

    private void UseInput(object args)
    {
        useInput = (bool)args;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.GameOver, UseInput);
    }
}



