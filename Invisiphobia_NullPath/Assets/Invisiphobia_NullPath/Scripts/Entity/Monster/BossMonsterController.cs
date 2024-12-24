using Common.Event;
using MimicSpace;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossMonsterController : MonsterController
{
    Coroutine coroutine;
    private const float attackDistance = 3f;

    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(EntityManager.Instance.Player);

        agent.speed = runSpeed;

        monster.MyState.WanderingEvent += OnAttackingUpdate;

        monster.aiState = AIStateType.Wandering;

        gameObject.SetActive(false);
    }

    public override void PlayerAttackMonster()
    {
    }

    protected override void AttackingUpdate()
    {
        if (!agent.enabled)
            return;

        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(target.transform.position, path))
        {
            agent.SetDestination(target.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coroutine = StartCoroutine(CoTargetDistance());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    IEnumerator CoTargetDistance()
    {
        while (true)
        {
            if(targetDistance <= attackDistance)
            {
                target.Die();
                break;
            }
            yield return null;
        }
    }

    void OnAttackingUpdate()
    {
        monster.aiState = AIStateType.Attacking;
    }

    protected override void LookingAtTarget()
    {
        Vector3 directionToPlayer = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = lookRotation;
    }
}