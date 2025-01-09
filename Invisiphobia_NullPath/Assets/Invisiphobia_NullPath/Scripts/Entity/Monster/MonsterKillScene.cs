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
    protected IDetectable monster;

    private event Action OnKillEvent;

    public void Init(Monster monster)
    {
        target = monster.MyController.Target;
        this.monster = monster;

        monster.MyState.MonsterKillingEvent += OnKill;
        OnKillEvent += monster.OnStop;
    }

    private void OnKill()
    {
        OnKillEvent.Invoke();
        EventManager.Dispatch(GameEventType.UseMove, false);
        EventManager.Dispatch(GameEventType.UseEsc, false);
        EventManager.Dispatch(GameEventType.UseFollowMouse, lookTargetTr);
        EventManager.Dispatch(GameEventType.UseCrossHair, false);
        EventManager.Dispatch(GameEventType.UseTabletPause, true);

        Kill();
    }

    protected abstract void Kill();
}
