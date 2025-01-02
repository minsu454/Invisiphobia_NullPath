using System;
using System.Threading;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    public event Action IdleEvent;
    public event Action WanderingEvent;
    public event Action AttackingEvent;
    public event Action FleeingEvent;
    public event Action MonsterFleeingEvent;
    public event Action MonsterKillingEvent;

    private Monster monster;

    public void Init(Monster monster)
    {
        this.monster = monster;
    }

    private void Update()
    {
        switch (monster.AiState)
        {
            case AIStateType.Idle:
                IdleEvent?.Invoke();
                break;
            case AIStateType.Wandering:
                WanderingEvent?.Invoke();
                break;
            case AIStateType.Attacking:
                AttackingEvent?.Invoke();
                break;
            case AIStateType.Fleeing:
                FleeingEvent?.Invoke();
                break;
            case AIStateType.MonsterFleeing:
                MonsterFleeingEvent?.Invoke();
                break;
            case AIStateType.killing:
                MonsterKillingEvent?.Invoke();
                break;
        }
    }
}