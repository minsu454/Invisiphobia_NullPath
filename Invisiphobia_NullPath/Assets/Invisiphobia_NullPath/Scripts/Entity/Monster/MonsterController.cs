using Common.Timer;
using Common.Yield;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("AI")]
    [SerializeField] private float detectDistance;
    [SerializeField] private float safeDistance;
    [SerializeField] private float lookAtPlayerDistance;
    private AIStateType aiState;

    [Header("Wandering")]
    [SerializeField] private float minWanderDistance;
    [SerializeField] private float maxWanderDistance;
    [SerializeField] private int minWanderingCount;
    [SerializeField] private int maxWanderingCount;
    private int wanderingCount;


    [Header("Combat")]
    [SerializeField] private float attackDistance;
    private const float fieldOfView = 180f;

    private float playerDistance;
    private bool isHiding;
    private bool isStunned = false;

    [Header("NavMeshAgent")]
    [SerializeField] private NavMeshAgent agent;
    private Transform playerTransform;
    private NavMeshHit hit;

    public Vector3 monsterSpawnPoint { get; private set; }

    private Monster monster;
    private Coroutine timer;
    private bool canWander = true;

    public void Init(Monster monster)
    {
        this.monster = monster;
        monsterSpawnPoint = transform.position;
        playerTransform = Player.Instance.transform;
        SetState(AIStateType.Idle);
        ResetWanderingCount();
    }

    private void Start()
    {
        //SetState(AIStateType.Idle);
        //ResetWanderingCount();
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, playerTransform.position);

        switch (aiState)
        {
            case AIStateType.Idle:
                LookingAtPlayerUpdate();
                break;
            case AIStateType.Wandering:
                PassiveUpdate();
                break;
            case AIStateType.Attacking:
                AttackingUpdate();
                break;
            case AIStateType.Fleeing:
                FleeingUpdate();
                break;
            case AIStateType.MonsterFleeing:
                break;
            case AIStateType.Stun:
                SetStun();
                break;
        }
    }

    public void SetState(AIStateType state)
    {
        if (aiState == state)
            return;

        aiState = state;
        switch (aiState)
        {
            case AIStateType.Wandering:
                agent.speed = walkSpeed;
                break;
            case AIStateType.Attacking:
            case AIStateType.Fleeing:
            case AIStateType.MonsterFleeing:
                agent.speed = runSpeed;
                break;
            case AIStateType.Stun:
                agent.speed = 0f;
                break;
        }
    }

    void PassiveUpdate()
    {
        if (!monster.RendererActive)
        {
            return;
        }

        if (playerDistance < detectDistance && !isHiding && IsPlayerInFieldOfView()) // 플레이어가 감지 범위 안에 있고 숨지 않은 경우
        {
            SetState(AIStateType.Attacking);
        }
        else if ((isHiding || playerDistance > detectDistance) && aiState != AIStateType.Wandering) // 플레이어를 놓친 경우 Wandering으로 전환
        {
            SetState(AIStateType.Wandering);
        }

        if (AIStateType.Wandering == aiState && agent.remainingDistance < 0.1f && canWander)
        {
            canWander = false;
            WanderToNewLocation();  // 새 위치로 이동

            // 이동 횟수를 모두 소진하면 투명화 상태로 전환
            if (wanderingCount <= 0)
            {
                //SetStun();
                //ResetToSpawnPoint();
                ResetCycle();
            }
        }
    }

    private void LookingAtPlayerUpdate()
    {
        if (playerDistance > lookAtPlayerDistance)
        {
            SetState(AIStateType.Wandering);
        }
        else
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void AttackingUpdate()
    {
        if (playerDistance < detectDistance && aiState != AIStateType.MonsterFleeing)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(playerTransform.position, path))
            {
                agent.SetDestination(playerTransform.position);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            SetState(AIStateType.Wandering);
        }
    }

    void FleeingUpdate()
    {
        if (agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else
        {
            ResetWanderingCount();
            SetState(AIStateType.Wandering);
        }
    }

    private IEnumerator SetStun()
    {
        if (isStunned) yield break;

        isStunned = true;
        SetState(AIStateType.Stun);
        yield return YieldCache.WaitForSeconds(2f);

        isStunned = false;
        SetState(AIStateType.Wandering);
    }

    void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
    }

    public void FleeFromPlayer()
    {
        SetState(AIStateType.MonsterFleeing);
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0;
        directionToPlayer.Normalize();

        Vector3 fleeDirection = Quaternion.Euler(0, Random.Range(-150, 150), 0) * directionToPlayer;  // 반대 방향 계산

        float fleeDistance = Random.Range(minWanderDistance, maxWanderDistance);    // 도망칠 거리 계산
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            StartCoroutine(FleeAndTransitionToWandering()); // 도망 후 상태 전환 처리
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

    void ResetWanderingCount()
    {
        wanderingCount = Random.Range(minWanderingCount, maxWanderingCount);
    }

    void ResetCycle()
    {
        SetState(AIStateType.Idle);
        if (timer != null)
        {
            StopCoroutine(timer);
        }
        timer = null;
        ResetWanderingCount();
        canWander = true;
        //monster.Invisible();
        monster.ResetCycle();
    }

    void WanderToNewLocation()
    {
        timer = StartCoroutine(CoTimer.Start(0.5f, () =>
        {
            agent.SetDestination(GetWanderLocation());
            wanderingCount--;
            canWander = true;
        }));

    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    Vector3 GetWanderLocation()
    {
        for (int i = 0; i < 30; i++) // 최대 30번만 시도
        {
            Vector3 randomPoint = transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance));
            randomPoint.y = transform.position.y; // 같은 높이로 설정

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, maxWanderDistance, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    float pathLength = GetPathLength(path);

                    if (pathLength >= minWanderDistance && pathLength <= maxWanderDistance)
                    {
                        return hit.position;
                    }
                }
            }
        }

        return transform.position; // 위치 찾기 실패하면 현재 위치 반환..
    }

    Vector3 GetFleeLocation()
    {
        int i = 0;
        do
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        } while (Vector3.Distance(transform.position, hit.position) < detectDistance);

        return hit.position; // 위치 찾기 실패하면 현재 위치 반환..
    }

    float GetPathLength(NavMeshPath path)
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

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - playerTransform.position, transform.position + targetPos);
    }
}
