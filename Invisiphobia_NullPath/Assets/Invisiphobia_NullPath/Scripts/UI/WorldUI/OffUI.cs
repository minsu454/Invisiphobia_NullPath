using Common.Yield;
using System.Collections;
using UnityEngine;

public class OffUI : WorldUI<TabletStateType>
{
    private ITargetable target;

    private Coroutine dieTimer;

    public override void Init(IActiveStatable<TabletStateType> subject)
    {
        target = EntityManager.Instance.Player;
    }

    public override void Subscribe(IActiveStatable<TabletStateType> subject)
    {
        dieTimer = StartCoroutine(CoDieTimer());
    }

    public override void Unsubscribe(IActiveStatable<TabletStateType> subject)
    {
        StopCoroutine(dieTimer);
        dieTimer = null;
        gameObject.SetActive(false);
    }

    private IEnumerator CoDieTimer()
    {
        Debug.Log("Start");
        yield return YieldCache.WaitForSeconds(5f);
        target.Die();
    }
}