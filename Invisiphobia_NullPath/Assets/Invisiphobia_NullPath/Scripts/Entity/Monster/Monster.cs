using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] private MonsterController myController;
    [SerializeField] private MeshRenderer myRenderer;

    public bool RendererActive { get { return myRenderer.enabled; } }

    public bool isRevealed = false;
    public override void Init()
    {
        base.Init();
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

    public void Invisible()
    {
        myRenderer.enabled = false;
        myController.SetState(AIStateType.Idle);
    }
}
