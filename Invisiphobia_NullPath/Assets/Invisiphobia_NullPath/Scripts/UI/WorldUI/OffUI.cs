using Common.Yield;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class OffUI : WorldUI<TabletStateType>
{
    [SerializeField] private Image battery;
    [SerializeField] private float duration = 20f;

    private ITargetable target;

    private Coroutine dieTimer;
    private Tween imageTween;

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
        imageTween.Kill();
        imageTween = null;
        StopCoroutine(dieTimer);
        dieTimer = null;

        battery.color = Color.red;
        gameObject.SetActive(false);
    }

    private IEnumerator CoDieTimer()
    {
        imageTween = battery.DOFade(0f, 0.1f).SetLoops(-1, LoopType.Yoyo);
        yield return YieldCache.WaitForSeconds(duration);
        imageTween.Kill();
        target.Die();
    }
}