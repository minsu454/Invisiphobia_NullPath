using Common.Event;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterKillScene : MonsterKillScene
{
    [SerializeField] private Ease killEase;
    [SerializeField] private float duration;

    protected override void Kill()
    {
        transform.DOMove(Camera.main.transform.position, duration).SetEase(killEase).OnComplete(() =>
        {
            target.Die();
        });
    }
}
