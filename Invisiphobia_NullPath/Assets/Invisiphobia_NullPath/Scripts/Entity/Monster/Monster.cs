using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [Header("Controller")]
    [SerializeField] private MonsterController myController;
    public MonsterController MyController { get { return myController; } }

    [Header("KillScene")]
    [SerializeField] private bool useKillScene = false;
    [SerializeField] private MonsterKillScene myKillScene;
    public MonsterKillScene MyKillScene { get { return myKillScene; } }

    [Header("Sound")]
    [SerializeField] private MonsterSound monsterSound;
    public MonsterSound myMonsterSound { get { return monsterSound; } }

    [Header("Renderer")]
    public Renderer myRenderer;

    [Header("Anim")]
    [SerializeField] private bool useAnim = false;
    [SerializeField] private MonsterAnim myAnimation;

    
    public bool RendererActive { get { return myRenderer.enabled; } }

    private AIStateType aiState;
    public AIStateType AiState 
    {  
        get { return aiState; } 
        set 
        {
            if (aiState == value)
            {
                return;
            }
            changeStateEvent?.Invoke();
            aiState = value;
        }
    }

    public event Action changeStateEvent;

    [SerializeField] private MonsterState myState;
    public MonsterState MyState { get { return myState; } }

    MapIcon IDetectable.MapIcon => mapIcon;
    private MapIcon mapIcon;

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    public bool IsDetectTablet { get; set; }

    [Header("MapIcon")]
    [SerializeField] private GameObject mapIconPrefab;

    public override void Init()
    {
        GameObject go = Instantiate(mapIconPrefab);
        mapIcon = go.GetComponent<MapIcon>();

        myState.Init(this);
        mapIcon.Init(transform);
        myMonsterSound.Init(this);

        if (useAnim)
            myAnimation.Init(this);

        myController.Init(this);

        if (useKillScene)
            myKillScene.Init(this);
    }

    public virtual void Detected()
    {
        StateType = PropStateType.Detected;
        mapIcon.Detected();
        myController.Detacted();
    }

    public virtual void Detecting()
    {
        StateType = PropStateType.Detecting;
        mapIcon.Detecting();
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

        myController.Revealed();
        StateType = PropStateType.Revealed;
        myRenderer.enabled = true;
        mapIcon.Revealed();

        AiState = AIStateType.Reveal;
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

    public void OnStop()
    {
        AiState = AIStateType.None;
    }
}
