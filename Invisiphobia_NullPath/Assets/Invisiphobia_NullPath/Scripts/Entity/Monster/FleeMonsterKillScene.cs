using Common.Event;
using Common.Yield;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeMonsterKillScene : MonsterKillScene
{
    [SerializeField] private GameObject myLight;

    protected override void Kill()
    {
        myLight.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = true;
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
        transform.eulerAngles = new Vector3(20f, transform.eulerAngles.y, transform.eulerAngles.z);
        yield return YieldCache.WaitForSeconds(2f);
        target.Die();
    }

    IEnumerator CoUnrevealKillAnimTime()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        yield return YieldCache.WaitForSeconds(2f);
        target.Die();
    }
}
