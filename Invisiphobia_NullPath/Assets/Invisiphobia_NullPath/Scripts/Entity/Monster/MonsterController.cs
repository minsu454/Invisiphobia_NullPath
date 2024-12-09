using Common.Timer;
using Common.Yield;
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
    private const float fieldOfView = 120f;

    private float playerDistance;
    private bool isHiding;
    private bool isStunned = false;

    [Header("NavMeshAgent")]
    [SerializeField] private NavMeshAgent agent;
    private Transform playerTransform;

    public Vector3 monsterSpawnPoint { get; private set; }

    private Monster monster;
    private Coroutine timer;
    private bool canWander = true;

    public void Init(Monster monster)
    {
        this.monster = monster;
        monsterSpawnPoint = transform.position;
        playerTransform = Player.Instance.transform;
    }

    private void Start()
    {
        SetState(AIStateType.Idle);
        ResetWanderingCount();
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
                agent.speed = runSpeed;
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
                ResetCycle();
                ResetToSpawnPoint();
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
        if (playerDistance < detectDistance)
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
            ResetCycle();
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
    }

    void ResetWanderingCount()
    {
        wanderingCount = Random.Range(minWanderingCount, maxWanderingCount);
    }

    void ResetCycle()
    {
        SetState(AIStateType.Idle);
        StopCoroutine(timer);
        timer = null;
        ResetWanderingCount();
        canWander = true;
        monster.Invisible();
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

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;

        int i = 0;
        do
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        } while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance);

        return hit.position;
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

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

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - playerTransform.position, transform.position + targetPos);
    }
}
