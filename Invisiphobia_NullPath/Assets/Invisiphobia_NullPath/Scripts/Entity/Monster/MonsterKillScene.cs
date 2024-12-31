using Common.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class MonsterKillScene : MonoBehaviour
{
    [Header("MonsterKillScene")]
    [SerializeField] protected Transform lookTargetTr;
    protected ITargetable target;

    private event Action OnKillEvent;

    public void Init(Monster monster)
    {
        target = monster.MyController.Target;

        monster.MyState.MonsterKillingEvent += OnKill;
        OnKillEvent += monster.OnStop;
    }

    private void OnKill()
    {
        OnKillEvent.Invoke();
        EventManager.Dispatch(GameEventType.UseMove, false);
        EventManager.Dispatch(GameEventType.UseEsc, false);
        EventManager.Dispatch(GameEventType.UseFollowMouse, lookTargetTr);

        Kill();
    }

    protected abstract void Kill();
}
