using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player Instance;


    public PlayerController PlayerController;
    public PlayerState PlayerState;
    public PlayerMovement PlayerMovement;


    private void Start()
    {
        Instance = this;
        PlayerController = GetComponent<PlayerController>();
        PlayerState = GetComponent<PlayerState>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }

}
