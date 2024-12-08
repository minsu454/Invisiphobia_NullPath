using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode sprintKey;
    public KeyCode jumpKey;
    public KeyCode crouchKey;
    public KeyCode interactKey;
    public KeyCode tabletKey;

    public event Action<Vector3> playerMoveActionEvent;
    public event Action playerSprintActionEvent;
    public event Action playerJumpActionEvent;
    public event Action<bool> playerCrouchActionEvent;
    public event Action playerInteractActionEvent;
    public event Action playerTabletActionEvent;
    public event Action playerThrowActionEvent;
    public event Action<int> tabletSwitchActionEvent;

    public bool isSprinting = false;
    public bool isCrouched = false;
    public bool isHoldRightmouse = false;

    private readonly KeyCode[] alphaKeyArr = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
    };
    private const int alphaKeyNum = (int)KeyCode.Alpha1;


    public void Init(Player player)
    {
        sprintKey = player.PlayerMovement.sprintKey;
        jumpKey = player.PlayerMovement.jumpKey;
        crouchKey = player.PlayerMovement.crouchKey;
        interactKey = player.PlayerInteract.interactKey;
        //tabletKey = Player.Instance.PlayerInventory.tabletKey;
    }

    void Update()
    {
        OnPlayerSprint();
        OnPlayerJump();
        OnPlayerCrouch();
        OnPlayerInteract();
        OnPlayerTablet();
        OnPlayerThrow();
        OnTabletSwitch();
    }

    private void FixedUpdate()
    {
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
        if(Input.GetKeyDown(KeyCode.Tab))
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

    private void OnPlayerThrow()
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
            playerThrowActionEvent.Invoke();
        }
    }

}



