using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class FleeMonsterController : MonsterController
{
    [Header("Fleeing")]
    [SerializeField] protected float minDistance;
    [SerializeField] protected float maxDistance;

    public PlayerState playerState;

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

        if (NavMesh.SamplePosition(fleeDestination, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
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
        const int angleStep = 10;
        Vector3 bestDestination = transform.position;
        float maxDistanceFromPlayer = 0f;

        for (float angle = -100; angle <= 100; angle += angleStep)
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
            float distanceFromPlayer = Vector3.Distance(navMeshPosition, targetTransform.position);

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

    // 랜덤 방향
    Vector3 GetRandomDirection()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized; // 2D 원 기준 랜덤 방향
        return new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    // NavMesh 위치 확인
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

    // 플레이어를 지나치지 않도록 각도 설정(플레이어를 바라본 180도를 제외하도록)
    bool IsValidFleeAngle(Vector3 position)
    {
        Vector3 directionToPlayer = (targetTransform.position - position).normalized;
        float signedAngleToPlayer = Vector3.SignedAngle(-transform.forward, directionToPlayer, Vector3.up); // 도망중에는 찍히지 않도록 하던가, 플레이어가 보는 방향의 범위와 몬스터가 바라보는방향의 범위 비교하던가..

        return signedAngleToPlayer > -105 && signedAngleToPlayer < 105;
    }

    // 경로 길이 확인
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

    protected override void AttackingUpdate()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(targetTransform.position, path))
        {
            agent.SetDestination(targetTransform.position);
        }

        if (targetDistance < 1f)     // 닿으면 주금
        {
            playerState.Die();
        }
    }

    protected void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
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
