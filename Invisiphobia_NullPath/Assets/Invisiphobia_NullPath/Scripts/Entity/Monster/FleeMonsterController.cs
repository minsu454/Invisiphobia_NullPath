using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FleeMonsterController : MonsterController
{
    [Header("Wandering")]
    [SerializeField] protected float minWanderDistance;
    [SerializeField] protected float maxWanderDistance;
    [SerializeField] protected int minWanderingCount;
    [SerializeField] protected int maxWanderingCount;
    protected int wanderingCount;

    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(Player.Instance.transform);

        agent.speed = runSpeed;

        monster.MyState.IdleEvent += LookingAtPlayerUpdate;
        monster.MyState.WanderingEvent += OnWanderingUpdate;
    }

    public override void PlayerAttackMonster()
    {
        monster.aiState = AIStateType.MonsterFleeing;

        Vector3 fleeDestination = GetSafeFleeDestination();

        if (NavMesh.SamplePosition(fleeDestination, out NavMeshHit hit, maxWanderDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // 
            StartCoroutine(FleeAndTransitionToWandering());
        }
    }

    private IEnumerator FleeAndTransitionToWandering()
    {
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        ResetToSpawnPoint();
        LookingAtPlayerUpdate();
    }

    Vector3 GetSafeFleeDestination()    // 도망 위치 계산
    {
        const int maxAttempts = 30;
        Vector3 bestDestination = transform.position;
        float maxDistanceFromPlayer = 0f;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = GetRandomDirection();
            Vector3 potentialDestination = transform.position + randomDirection * maxWanderDistance;

            if (!TryGetValidNavMeshPosition(potentialDestination, out Vector3 navMeshPosition))
                continue;

            if (!IsValidFleeAngle(navMeshPosition))
                continue;

            if (!IsValidPathLength(navMeshPosition, out float pathLength))
                continue;

            // 플레이어로부터 가장 멀어진 위치
            float distanceFromPlayer = Vector3.Distance(navMeshPosition, targetTransform.position);
            if (distanceFromPlayer > maxDistanceFromPlayer)
            {
                maxDistanceFromPlayer = distanceFromPlayer;
                bestDestination = navMeshPosition;
            }
        }

        return bestDestination;
    }

    // 랜덤 방향
    Vector3 GetRandomDirection()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized; // 2D 원 기준 랜덤 방향
        return new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    // NavMesh 위치 확인
    bool TryGetValidNavMeshPosition(Vector3 position, out Vector3 navMeshPosition)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxWanderDistance, NavMesh.AllAreas))
        {
            navMeshPosition = hit.position;
            return true;
        }
        navMeshPosition = Vector3.zero;
        return false;
    }

    // 플레이어를 지나치지 않도록 각도 설정(플레이어를 바라본 180도를 제외하도록)
    bool IsValidFleeAngle(Vector3 position)
    {
        Vector3 directionToPlayer = (targetTransform.position - position).normalized;
        float angleToPlayer = Vector3.Angle(-transform.forward, directionToPlayer);
        return angleToPlayer > 90;
    }

    // 경로 길이 확인
    bool IsValidPathLength(Vector3 destination, out float pathLength)
    {
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            pathLength = GetPathLength(path);
            return pathLength >= minWanderDistance && pathLength <= maxWanderDistance;
        }
        pathLength = 0f;
        return false;
    }

    protected override void AttackingUpdate()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(targetTransform.position, path))
        {
            agent.SetDestination(targetTransform.position);
        }
    }

    protected void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
    }

    protected void LookingAtPlayerUpdate()
    {
        if (targetDistance < lookAtPlayerDistance)
        {
            Vector3 directionToPlayer = (targetTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

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

    void ResetCycle()
    {
        monster.aiState = AIStateType.Idle;
        monster.ResetCycle();
    }

    void OnWanderingUpdate()
    {
        monster.aiState = AIStateType.Attacking;
    }
}
