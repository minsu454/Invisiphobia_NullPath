using Common.Yield;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OffUI : WorldUI<TabletStateType>
{
    [SerializeField] private Image popup;

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
        StartCoroutine(Copopup());
        yield return YieldCache.WaitForSeconds(5f);
        target.Die();
    }

    private IEnumerator Copopup()
    {
        popup.enabled = true;
        yield return YieldCache.WaitForSeconds(0.1f);
        popup.enabled = false;
    }
}