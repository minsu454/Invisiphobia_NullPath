using Common.Event;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FleeMonsterController : MonsterController
{
    [Header("Fleeing")]
    [SerializeField] protected float minDistance;
    [SerializeField] protected float maxDistance;
    private bool isFleeing = false;

    private const float attackDistance = 2f;

    /// <summary>
    /// 몬스터의 상태를 초기화하고 타겟을 플레이어로 설정
    /// </summary>
    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(EntityManager.Instance.Player);
        monster.myRenderer.enabled = false;

        agent.speed = runSpeed;

        monster.MyState.WanderingEvent += OnWanderingUpdate;
    }

    /// <summary>
    /// 플레이어가 몬스터를 공격했을 때 몬스터를 도망 상태로 전환
    /// </summary>
    public override void PlayerAttackMonster()
    {
        if (!isFleeing)
        {
            monster.AiState = AIStateType.MonsterFleeing;
            isFleeing = true;

            Vector3 fleeDestination = GetSafeFleeDestination();

            if (NavMesh.SamplePosition(fleeDestination, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                StartCoroutine(FleeAndTransitionToWandering());
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 도망을 친 후 처음 스폰자리로 순간이동
    /// </summary>
    private IEnumerator FleeAndTransitionToWandering()
    {
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        ResetToSpawnPoint();
        isFleeing = false;
    }

    /// <summary>
    /// 몬스터 도망 위치 계산
    /// </summary>
    Vector3 GetSafeFleeDestination()
    {
        const int angleStep = 10;
        Vector3 bestDestination = transform.position;
        float maxDistanceFromPlayer = 0f;

        for (float angle = 100; angle <= 260; angle += angleStep)
        {
            Vector3 fleeDirection = Quaternion.Euler(0, angle, 0) * -transform.forward;
            Vector3 potentialDestination = transform.position + fleeDirection * maxDistance;

            if (!TryGetValidNavMeshPosition(potentialDestination, out Vector3 navMeshPosition))
                continue;

            if (!IsValidFleeAngle(navMeshPosition))
                continue;

            if (!IsValidPathLength(navMeshPosition, out float pathLength))
                continue;

            // 플레이어로부터 가장 먼 위치 계산
            float distanceFromPlayer = Vector3.Distance(navMeshPosition, target.transform.position);

            if (distanceFromPlayer > maxDistanceFromPlayer)
            {
                maxDistanceFromPlayer = distanceFromPlayer;
                bestDestination = navMeshPosition;
            }
        }

        if (bestDestination == transform.position)
        {
            bestDestination = monsterSpawnPoint;
        }

        return bestDestination;
    }

    /// <summary>
    /// 주어진 위치가 NavMesh에서 유효한지 검사
    /// </summary>
    bool TryGetValidNavMeshPosition(Vector3 position, out Vector3 navMeshPosition)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            navMeshPosition = hit.position;
            return true;
        }
        navMeshPosition = Vector3.zero;
        return false;
    }

    /// <summary>
    /// 플레이어를 지나치지 않는 각도인지 검사
    /// </summary>
    bool IsValidFleeAngle(Vector3 position)
    {
        Vector3 directionToPlayer = (target.transform.position - position).normalized;
        float signedAngleToPlayer = Vector3.SignedAngle(-transform.forward, directionToPlayer, Vector3.up);

        return signedAngleToPlayer > -105 && signedAngleToPlayer < 105;
    }
    /// <summary>
    /// 이동 경로 길이가 유효한지 검사
    /// </summary>
    bool IsValidPathLength(Vector3 destination, out float pathLength)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            pathLength = GetPathLength(path);
            return pathLength <= maxDistance;
        }
        pathLength = 0f;
        return false;
    }

    /// <summary>
    /// 몬스터 공격 상태 동작
    /// </summary>
    protected override void AttackingUpdate()
    {
        if (!agent.enabled)
            return;

        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(target.transform.position, path))
        {
            agent.SetDestination(target.transform.position);
        }

        if (targetDistance < attackDistance)     // 닿으면 주금
        {
            agent.speed = 0f;
            target.Die();
        }
    }

    /// <summary>
    /// 스폰 지점으로 위치 초기화 후 행동 사이클 리셋
    /// </summary>
    protected void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
    }

    /// <summary>
    /// 실제 이동 경로 길이 계산
    /// </summary>
    protected float GetPathLength(NavMeshPath path)
    {
        float totalLength = 0f;

        if (path.corners.Length < 2)    // 코너(좌표)가 2개 미만이면 거리가 없음
            return totalLength;

        for (int i = 0; i < path.corners.Length - 1; i++)   // 코너 간 거리 계산
        {
            totalLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);  // totalLength에 거리 더해서 총 경로 길이 반환
        }

        return totalLength;
    }

    /// <summary>
    /// 몬스터 행동 사이클 리셋
    /// </summary>
    void ResetCycle()
    {
        monster.AiState = AIStateType.Idle;
        monster.ResetCycle();
    }

    /// <summary>
    /// 플레이어와 충돌 시 게임 오버 트리거
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Kill();
        }
    }

    /// <summary>
    /// Wandering 이벤트 상태 업데이트
    /// </summary>
    void OnWanderingUpdate()
    {
        monster.AiState = AIStateType.Attacking;
    }

    /// <summary>
    /// 감지되었을 때 플레이어를 바라보도록 하는 함수
    /// </summary>
    protected override void LookingAtTarget()
    {
        Vector3 directionToPlayer = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = lookRotation;
    }
}
