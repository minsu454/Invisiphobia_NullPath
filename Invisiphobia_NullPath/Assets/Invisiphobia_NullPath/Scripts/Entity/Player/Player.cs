using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController; } }

    [SerializeField] private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement { get { return playerMovement; } }

    [SerializeField] private PlayerAnimation playerAnimation;
    public PlayerAnimation PlayerAnimation { get { return playerAnimation; } }

    [SerializeField] private PlayerInteract playerInteract;
    public PlayerInteract PlayerInteract { get { return playerInteract; } }

    [SerializeField] private CameraController cameraController;
    public CameraController CameraController { get { return cameraController; } }

    [SerializeField] private PlayerInventory playerInventory;
    public PlayerInventory PlayerInventory { get { return playerInventory; } }

    [SerializeField] private PlayerState playerState;
    public PlayerState PlayerState { get { return playerState; } }

    public override void Init()
    {
        PlayerController.Init(this);
        PlayerInventory.Init(this);
        PlayerMovement.Init(this);
        PlayerInteract.Init(this);
        CameraController.Init(this);
        PlayerState.Init(this);
    }

}
