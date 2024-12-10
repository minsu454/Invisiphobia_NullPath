using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] private MonsterController myController;
    public MonsterController MyController { get { return myController; } }

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

    #region Test
    private void Start()
    {
        mapIcon.Init(transform);
        myController.Init(this);
    }
    #endregion

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
        mapIcon.Invisible();
        myRenderer.enabled = false;
    }

    public void ResetCycle()
    {
        if (IsDetectTablet)
        {
            mapIcon.Invisible();
            myRenderer.enabled = false;
            Detected();
        }
        else
        {
            Invisible();
        }
    }

    public void SetFillAmount(float value)
    {
        mapIcon.SetFillAmount(value);
    }
}
