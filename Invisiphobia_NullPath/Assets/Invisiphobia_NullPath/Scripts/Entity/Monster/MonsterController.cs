using Common.Timer;
using Common.Yield;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class MonsterController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    [Header("AI")]
    [SerializeField] protected float detectDistance;
    [SerializeField] protected float safeDistance;
    [SerializeField] protected float lookAtPlayerDistance;

    [Header("Wandering")]
    [SerializeField] protected float minWanderDistance;
    [SerializeField] protected float maxWanderDistance;
    [SerializeField] protected int minWanderingCount;
    [SerializeField] protected int maxWanderingCount;
    protected int wanderingCount;


    [Header("Combat")]
    [SerializeField] protected float attackDistance;
    protected const float fieldOfView = 180f;

    protected float playerDistance;
    protected bool isHiding;
    protected bool isAlwaysAttacking; // boss만 true

    [Header("NavMeshAgent")]
    [SerializeField] protected NavMeshAgent agent;
    protected Transform playerTransform;
    protected NavMeshHit hit;

    public Vector3 monsterSpawnPoint { get; private set; }

    protected Monster monster;
    private Coroutine timer;
    protected bool canWander = true;

    public virtual void Init(Monster monster)
    {
        this.monster = monster;
        monsterSpawnPoint = transform.position;
        playerTransform = Player.Instance.transform;
        monster.aiState = AIStateType.Idle;
        ResetWanderingCount();

        monster.MyState.IdleEvent += LookingAtPlayerUpdate;
        monster.MyState.WanderingEvent += PassiveUpdate;
        monster.MyState.AttackingEvent += AttackingUpdate;
        monster.MyState.FleeingEvent += FleeingUpdate;
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, playerTransform.position);
    }

    void PassiveUpdate()
    {
        if (!monster.RendererActive)
        {
            return;
        }

        if (playerDistance < detectDistance && !isHiding && IsPlayerInFieldOfView()) // 플레이어가 감지 범위 안에 있고 숨지 않은 경우
        {
            monster.aiState = AIStateType.Attacking;
            agent.speed = runSpeed;
        }
        else if ((isHiding || playerDistance > detectDistance) && monster.aiState != AIStateType.Wandering) // 플레이어를 놓친 경우 Wandering으로 전환
        {
            monster.aiState = AIStateType.Wandering;
            agent.speed = walkSpeed;
        }

        if (AIStateType.Wandering == monster.aiState && agent.remainingDistance < 0.1f && canWander)
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

    protected void LookingAtPlayerUpdate()
    {
        if (playerDistance > lookAtPlayerDistance)
        {
            monster.aiState = AIStateType.Wandering;
            agent.speed = walkSpeed;
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
        if (playerDistance < detectDistance && monster.aiState != AIStateType.MonsterFleeing)
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
            monster.aiState = AIStateType.Wandering;
            agent.speed = walkSpeed;
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
            monster.aiState = AIStateType.Wandering;
            agent.speed = walkSpeed;
        }
    }

    protected void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
    }

    public abstract void PlayerAttackMonster();

    void ResetWanderingCount()
    {
        wanderingCount = Random.Range(minWanderingCount, maxWanderingCount);
    }

    void ResetCycle()
    {
        monster.aiState = AIStateType.Idle;
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

        return hit.position;
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

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - playerTransform.position, transform.position + targetPos);
    }
}
