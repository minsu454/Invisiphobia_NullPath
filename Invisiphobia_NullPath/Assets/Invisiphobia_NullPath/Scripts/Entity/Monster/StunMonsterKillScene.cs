using Common.Event;
using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunMonsterKillScene : MonsterKillScene
{
    protected override void Kill()
    {
        StartCoroutine(CoKillAnimTime());
    }

    IEnumerator CoKillAnimTime()
    {
        yield return YieldCache.WaitForSeconds(4.17f);
        target.Die();
    }
}
