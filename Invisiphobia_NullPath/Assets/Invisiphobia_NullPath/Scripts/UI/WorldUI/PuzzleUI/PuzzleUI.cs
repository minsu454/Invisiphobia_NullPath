using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PuzzleUI : WorldUI<TabletStateType>
{
    public event Action OnCompletedEvent;                       //클리어시 이벤트
    public event Action<WorldUI<TabletStateType>> OnDestroyEvent;                         //삭제시 이벤트

    protected void OnComplete()
    {
        OnCompletedEvent?.Invoke();
        Destroy(gameObject);
    }

    protected override void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
        base.OnDestroy();
    }
}
