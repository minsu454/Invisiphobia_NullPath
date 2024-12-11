using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] private MonsterController myController;
    public MonsterController MyController { get { return myController; } }

    [SerializeField] private MeshRenderer myRenderer;

    public bool RendererActive { get { return myRenderer.enabled; } }

    public AIStateType aiState;
    [SerializeField] private MonsterState myState;
    public MonsterState MyState { get { return myState; } }

    MapIcon IDetectable.MapIcon => mapIcon;
    private MapIcon mapIcon;

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    public bool IsDetectTablet { get; set; }

    #region Test(우선순위 문제)
    [Header("MapIcon")]
    [SerializeField] private GameObject mapIconPrefab;

    private void Start()
    {
        myState.Init(this);
        mapIcon.Init(transform);
        myController.Init(this);
    }
    #endregion

    public override void Init()
    {
        //myState.Init(this);
        //mapIcon.Init(transform);
        //myController.Init(this);

        GameObject go = Instantiate(mapIconPrefab);
        mapIcon = go.GetComponent<MapIcon>();
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

        aiState = AIStateType.Wandering;
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

    public void SetMapIconToWall(bool active)
    {
        if (active)
        {
            mapIcon.Detected();
        }
        else
        {
            mapIcon.Invisible();
        }
    }
}
