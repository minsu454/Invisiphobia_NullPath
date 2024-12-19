using Common.Event;
using Common.Timer;
using Common.Yield;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class StunMonsterController : MonsterController
{
    [Header("Wandering")]
    [SerializeField] protected float minWanderDistance;
    [SerializeField] protected float maxWanderDistance;
    [SerializeField] protected int minWanderingCount;
    [SerializeField] protected int maxWanderingCount;
    [SerializeField] protected float detectDistance;
    protected int wanderingCount;
    protected bool canWander = true;

    [Header("Combat")]
    protected const float fieldOfView = 180f;

    private float hideAttackDistance = 5f;   // 숨었을 때 공격 범위

    private Coroutine timer;
    protected NavMeshHit hit;
    public PlayerMovement playerMovement;

    private bool isStunned = false;
    protected bool isHiding = false;

    public override void Init(Monster monster)
    {
        base.Init(monster);
        Player player = EntityManager.Instance.Player;
        SetTarget(player.transform);
        playerMovement = player.PlayerMovement;
        monster.myRenderer.enabled = false;

        monster.MyState.StunEvent += SetStun;

        ResetWanderingCount();

        monster.MyState.IdleEvent += LookingAtPlayerUpdate;
        monster.MyState.WanderingEvent += PassiveUpdate;
        monster.MyState.FleeingEvent += FleeingUpdate;
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

    protected override void AttackingUpdate()
    {
        if (!playerMovement.playerCanMove)
        {
            if (targetDistance < hideAttackDistance)    // 너무 가까이 있을 때 숨어서 주금
            {
                EventManager.Dispatch(GameEventType.GameOver, null);
                isHiding = false;
            }
            isHiding = true;

            monster.aiState = AIStateType.Wandering;
        }
        else
        {
            isHiding = false;
        }

        if (targetDistance < detectDistance && monster.aiState != AIStateType.MonsterFleeing && !isHiding)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(targetTransform.position, path))
            {
                agent.SetDestination(targetTransform.position);
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            monster.aiState = AIStateType.Wandering;
            agent.speed = walkSpeed;
        }

        if (targetDistance < 2f)     // 닿으면 주금
        {
            agent.speed = 0f;
            EventManager.Dispatch(GameEventType.GameOver, null);
        }
    }

    void ResetWanderingCount()
    {
        wanderingCount = Random.Range(minWanderingCount, maxWanderingCount);
    }

    void PassiveUpdate()
    {
        if (!monster.RendererActive)
        {
            return;
        }

        if (targetDistance < detectDistance && playerMovement.playerCanMove && IsPlayerInFieldOfView()) // 플레이어가 감지 범위 안에 있고 숨지 않은 경우
        {
            monster.aiState = AIStateType.Attacking;
            agent.speed = runSpeed;
        }
        else if ((isHiding || targetDistance > detectDistance) && monster.aiState != AIStateType.Wandering) // 플레이어를 놓친 경우 Wandering으로 전환
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
                ResetToSpawnPoint();
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = targetTransform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
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

    protected void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
    }

    private void OnTriggerEnter(Collider other)
    {
        EventManager.Dispatch(GameEventType.GameOver, null);
    }
}
