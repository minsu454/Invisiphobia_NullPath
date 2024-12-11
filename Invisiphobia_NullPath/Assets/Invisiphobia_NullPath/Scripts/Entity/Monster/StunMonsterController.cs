using Common.Yield;
using System.Collections;
using System.Threading;
using UnityEngine;

public class StunMonsterController : MonsterController
{
    private bool isStunned = false;

    public override void Init(Monster monster)
    {
        base.Init(monster);
        monster.MyState.StunEvent += SetStun;
    }

    private void SetStun()
    {
        StartCoroutine(CoSetStun());
    }

    private IEnumerator CoSetStun()
    {
        if (isStunned) yield break;

        isStunned = true;
        yield return YieldCache.WaitForSeconds(2f);

        isStunned = false;
        monster.aiState = AIStateType.Wandering;
        agent.speed = walkSpeed;
    }

    public override void PlayerAttackMonster()
    {
        monster.aiState = AIStateType.Stun;
        agent.speed = 0f;
    }
}
