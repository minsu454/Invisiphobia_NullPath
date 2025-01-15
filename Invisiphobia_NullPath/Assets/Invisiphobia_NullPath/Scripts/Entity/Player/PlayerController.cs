using Common.Event;
using Common.Setting;
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
    public event Action <bool> playerSprintActionEvent;
    //public event Action playerJumpActionEvent;
    public event Action<bool> playerCrouchActionEvent;
    public event Action playerInteractActionEvent;
    public event Action playerTabletActionEvent;
    public event Action playerZoomClickActionEvent;
    public event Action<bool> playerWheelClickActionEvent;
    public event Action playerClickActionEvent;
    public event Action<int> tabletSwitchActionEvent;
    public event Action playerPutDownActionEvent;
    public event Action playerEscActionEvent;

    public bool isSprinting = false;
    public bool isCrouched = false;
    public bool isHoldRightmouse = false;
    public bool isHoldWheelmouse = false;

    private readonly KeyCode[] alphaKeyArr = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2,
    };
    private const int alphaKeyNum = (int)KeyCode.Alpha1;

    private bool useInput = true;
    private bool useTabletInput = true;
    private bool useEsc = true;
    private bool useWheel = false;

    public void Init(Player player)
    {
        EventManager.Subscribe(GameEventType.UseInput, UseInput);
        EventManager.Subscribe(GameEventType.UseTabletInput, UseTabletInput);
        EventManager.Subscribe(GameEventType.UseEsc, UseEsc);
        EventManager.Subscribe(GameEventType.UseWheelClick, UseWheel);
    }

    void Update()
    {
        if (useEsc)
            OnPlayerEsc();

        if (useWheel)
            OnWheelClick();

        if (!useInput)
            return;

        OnPlayerSprint();
        OnPlayerCrouch();
        OnPlayerInteract();
        OnPlayerClick();
        OnZoomClick();
        OnPlayerPutDown();

        if (!useTabletInput)
            return;

        OnPlayerTablet();
        OnTabletSwitch();
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
        if (Input.GetKeyDown(sprintKey) && !SettingManager.RunHold)
        {
            if (!isSprinting)
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }

        if (SettingManager.RunHold)
        {
            if (Input.GetKeyDown(sprintKey))
            {
                isSprinting = true;
            }
            else if (Input.GetKeyUp(sprintKey))
            {
                isSprinting = false;
            }
        }

        playerSprintActionEvent.Invoke(isSprinting);
    }

    private void OnPlayerMove()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        playerMoveActionEvent.Invoke(targetVelocity);
    }

    private void OnPlayerCrouch()
    {
        if (Input.GetKeyDown(crouchKey) && !SettingManager.CrouchHold)
        {
            if (!isCrouched)
            {
                isCrouched = true;
            }
            else
            {
                isCrouched = false;
            }
        }
        
        if (SettingManager.CrouchHold)
        {
            if (Input.GetKeyDown(crouchKey))
            {
                isCrouched = true;
            }
            else if (Input.GetKeyUp(crouchKey))
            {
                isCrouched = false;
            }
        }

        playerCrouchActionEvent.Invoke(isCrouched);
    }

    /// <summary>
    /// 스크롤 클릭 함수
    /// </summary>
    private void OnWheelClick()
    {
        isHoldWheelmouse = false;

        if (Input.GetKey(KeyCode.Mouse2))
        {
            isHoldWheelmouse = true;
        }

        playerWheelClickActionEvent?.Invoke(isHoldWheelmouse);
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

    private void OnPlayerEsc()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            playerEscActionEvent.Invoke();
        }
    }

    private void UseInput(object args)
    {
        useInput = (bool)args;
    }

    private void UseTabletInput(object args)
    {
        useTabletInput = (bool)args;
    }

    private void UseEsc(object args)
    {
        useEsc = (bool)args;
    }

    private void UseWheel(object args)
    {
        useWheel = (bool)args;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEventType.UseInput, UseInput);
        EventManager.Unsubscribe(GameEventType.UseTabletInput, UseTabletInput);
        EventManager.Unsubscribe(GameEventType.UseEsc, UseEsc);
        EventManager.Unsubscribe(GameEventType.UseWheelClick, UseWheel);
    }
}



