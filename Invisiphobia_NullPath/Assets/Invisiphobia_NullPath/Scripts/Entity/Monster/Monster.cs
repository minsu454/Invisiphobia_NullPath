using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity, IDetectable
{
    [SerializeField] public MonsterController myController;
    [SerializeField] public MeshRenderer myRenderer;

    public bool RendererActive { get { return myRenderer.enabled; } }

    MapIcon IDetectable.MapIcon => mapIcon;
    [SerializeField] public MapIcon mapIcon;

    public PropStateType StateType { get; protected set; } = PropStateType.None;

    public bool isRevealed = false;

    public override void Init()
    {
        mapIcon.Init();
    }

    public virtual void Detected() // 디텍터에 감지되었을 때
    {
        StateType = PropStateType.Detected;
        mapIcon.Detected();
    }

    public virtual void Detecting() // 감지되는 중(원돌아가는거)
    {
        StateType = PropStateType.Detecting;
    }

    public void DetectCompleted() // 원이 다 찼을 때
    {
        StateType = PropStateType.DetectCompleted;
    }

    public virtual void Revealed() // 태블릿 내릴때
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

    public virtual void Invisible() // 초기화 - 감지범위 벗어났을 때
    {
        StateType = PropStateType.None;
        mapIcon.Invisible();
        myRenderer.enabled = false;
    }

    public void SetFillAmount(float value) // 돌아가는 바
    {
        mapIcon.SetFillAmount(value);
    }
}
