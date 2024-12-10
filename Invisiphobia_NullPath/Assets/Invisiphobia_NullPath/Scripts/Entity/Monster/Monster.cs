using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] private MonsterController myController;
    [SerializeField] private MeshRenderer myRenderer;

    public bool RendererActive { get { return myRenderer.enabled; } }

    MapIcon IDetectable.MapIcon => mapIcon;
    [SerializeField] private MapIcon mapIcon;

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    public bool IsDetectTablet { get; set; }

    public bool isRevealed = false;

    public override void Init()
    {
        //mapIcon.Init(transform);
        //myController.Init(this);
    }

    private void Start()
    {
        mapIcon.Init(transform);
        myController.Init(this);
    }

    public virtual void Detected()
    {
        StateType = PropStateType.Detected;
        mapIcon.Detected();
    }

    public virtual void Detecting()
    {
        StateType = PropStateType.Detecting;
    }

    public void DetectCompleted()
    {
        StateType = PropStateType.DetectCompleted;
    }

    public virtual void Revealed()
    {
        if (StateType != PropStateType.DetectCompleted)
        {
            Detected();
            return;
        }

        StateType = PropStateType.Revealed;
        myRenderer.enabled = true;
        mapIcon.Revealed();

        myController.SetState(AIStateType.Wandering);
    }

    public virtual void Invisible()
    {
        StateType = PropStateType.None;
        //mapIcon.Invisible();
        //myRenderer.enabled = false;
        mapIcon.Invisible();
        myRenderer.enabled = false;
        //ResetCycle();

    }

    public void ResetCycle()
    {
        Invisible();
        Detected();
    }

    public void SetFillAmount(float value)
    {
        mapIcon.SetFillAmount(value);
    }
}
