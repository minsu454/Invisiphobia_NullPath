using Common.Event;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterKillScene : MonsterKillScene
{
    protected override void Kill()
    {
        transform.DOMove(target.transform.position, 3f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            target.Die();
        });
    }
}
