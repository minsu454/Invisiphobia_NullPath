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
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        GetComponent<Rigidbody>().isKinematic = true;
        //transform.eulerAngles = new Vector3(15f, transform.eulerAngles.y, transform.eulerAngles.z);
        if (monster.StateType != PropStateType.Revealed)
        {
            StartCoroutine(CoUnrevealKillAnimTime());
        }
        else
        {
            StartCoroutine(CoKillAnimTime());
        }
    }

    IEnumerator CoKillAnimTime()
    {
        yield return YieldCache.WaitForSeconds(2f);
        target.Die();
    }

    IEnumerator CoUnrevealKillAnimTime()
    {
        yield return YieldCache.WaitForSeconds(2f);
        target.Die();
    }
}
