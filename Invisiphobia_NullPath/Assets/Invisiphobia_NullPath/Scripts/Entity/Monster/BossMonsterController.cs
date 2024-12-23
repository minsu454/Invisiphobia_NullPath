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

    /// <summary>
    /// 몬스터의 상태를 초기화하고 타겟을 플레이어로 설정
    /// </summary>
    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(EntityManager.Instance.Player.transform);

        agent.speed = runSpeed;

        monster.MyState.WanderingEvent += OnAttackingUpdate;

        monster.aiState = AIStateType.Wandering;

        gameObject.SetActive(false);
    }

    public override void PlayerAttackMonster()
    {
    }

    /// <summary>
    /// NavMesh에 유효한 타겟 위치 업데이트
    /// </summary>
    protected override void AttackingUpdate()
    {
        if (!agent.enabled)
            return;

        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(targetTransform.position, path))
        {
            agent.SetDestination(targetTransform.position);
        }
    }

    /// <summary>
    /// 콜라이더 충돌 중 플레이어와의 거리 계산
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            coroutine = StartCoroutine(CoTargetDistance());
        }
    }

    /// <summary>
    /// 콜라이더에서 빠져나오면 코루틴을 중지
    /// </summary>
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

    /// <summary>
    /// 플레이어가 공격 범위 내에 들어오면 게임 오버
    /// </summary>
    IEnumerator CoTargetDistance()
    {
        while (true)
        {
            if(targetDistance <= attackDistance)
            {
                EventManager.Dispatch(GameEventType.GameOver, false);
                break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 몬스터 상태를 항상 Attacking으로 업데이트
    /// </summary>
    void OnAttackingUpdate()
    {
        monster.aiState = AIStateType.Attacking;
    }

    /// <summary>
    /// 플레이어를 바라보도록 하는 함수
    /// </summary>
    protected override void LookingAtTarget()
    {
        Vector3 directionToPlayer = (targetTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = lookRotation;
    }
}