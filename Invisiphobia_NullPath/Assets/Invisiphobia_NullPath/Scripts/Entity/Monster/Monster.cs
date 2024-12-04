using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] private MonsterController myController;
    [SerializeField] private MeshRenderer myRenderer;

    public bool RendererActive { get { return myRenderer.enabled; } }

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    public bool isRevealed = false;
    public override void Init()
    {
        base.Init();
    }

    public void Detected()
    {
        StateType = PropStateType.Detected;
    }

    public void Revealed()
    {
        StateType = PropStateType.Revealed;
        myRenderer.enabled = true;

        myController.SetState(AIStateType.Wandering);
    }

    public void Invisible()
    {
        StateType = PropStateType.None;

        myRenderer.enabled = false;
        myController.SetState(AIStateType.Idle);
    }
}
