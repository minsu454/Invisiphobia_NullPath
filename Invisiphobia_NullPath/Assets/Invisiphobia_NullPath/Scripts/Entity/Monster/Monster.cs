using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] private MonsterController myController;
    [SerializeField] private MeshRenderer myRenderer;

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
        
        //isRevealed = true;
        myRenderer.enabled = true;

        myController.SetState(AIStateType.Wandering);
    }
    /*
    public void BecomeInvisible()
    {
        if (isRevealed)
        {
            isRevealed = false;

            if (myRenderer != null)
            {
                myRenderer.enabled = false;
            }

            if (myController != null)
            {
                myController.SetState(AIState.Idle);
            }
        }
    }
    */
}
