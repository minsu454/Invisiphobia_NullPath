using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity , IDetectable
{
    [SerializeField] private MonsterController controller;
    [SerializeField] private MeshRenderer renderer;

    public bool isRevealed = false;
    public override void Init()
    {
        base.Init();
    }

    private void Awake()
    {
        
    }

    public void Detected()
    {

    }

    public void Revealed()
    {
        //controller.isRevealed = true;
        if (!isRevealed)
        {
            isRevealed = true;
            renderer.enabled = true;

            controller.SetState(AIState.Wandering);
        }
    }

    public void BecomeInvisible()
    {
        if (isRevealed)
        {
            isRevealed = false;

            if (renderer != null)
            {
                renderer.enabled = false;
            }

            if (controller != null)
            {
                controller.SetState(AIState.Idle);
            }
        }
    }
}
