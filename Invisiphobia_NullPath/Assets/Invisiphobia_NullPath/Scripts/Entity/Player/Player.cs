using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance;

    public PlayerController PlayerController {  get; private set; }
    public PlayerState PlayerState { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerAnimation PlayerAnimation { get; private set; }
    public PlayerInteract PlayerInteract { get; private set; }
    public CameraController CameraController { get; private set; }
    public PlayerInventory PlayerInventory { get; private set; }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Instance = this;
        PlayerController = GetComponent<PlayerController>();
        PlayerState = GetComponent<PlayerState>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
        CameraController = GetComponent<CameraController>();
        PlayerInteract = GetComponent<PlayerInteract>();
        PlayerInventory = GetComponent<PlayerInventory>();
        PlayerInteract.Init(this);
    }

}
