using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FleeMonsterController : MonsterController
{
    public override void PlayerAttackMonster()
    {
        monster.aiState = AIStateType.MonsterFleeing;
        agent.speed = runSpeed;

        Vector3 fleeDestination = GetSafeFleeDestination();

        if (NavMesh.SamplePosition(fleeDestination, out NavMeshHit hit, maxWanderDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            StartCoroutine(FleeAndTransitionToWandering());
        }
    }

    private IEnumerator FleeAndTransitionToWandering()
    {
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

        LookingAtPlayerUpdate();
        ResetToSpawnPoint();
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
            float distanceFromPlayer = Vector3.Distance(navMeshPosition, playerTransform.position);
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
        Vector3 directionToPlayer = (playerTransform.position - position).normalized;
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
}
