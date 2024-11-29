using Common.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [Header("Stats")]
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    public float detectDistance;
    public float safeDistance;
    public float lookAtPlayerDistance;
    private AIStateType aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    public float wanderingTime = 10f;
    private int wanderingCount;
    private int maxWanderingCount;
    private float wanderingTimer = 0f;


    [Header("Combat")]
    public float attackDistance;
    public float fieldOfView = 120f;

    private float playerDistance;
    private bool isHiding;

    public Transform playerTransform;
    private NavMeshAgent agent;
    [SerializeField] private MeshRenderer meshRenderer;
    //private Animator animator;
    //private SkinnedMeshRenderer[] meshRenderers;

    private Monster monster;
    private Coroutine coroutine;
    private bool check = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        //meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        SetState(AIStateType.Idle);
        ResetWanderingCount();
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, playerTransform.position);

        //animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIStateType.Idle:
            case AIStateType.Wandering:
                PassiveUpdate();
                break;
            case AIStateType.Attacking:
                AttackingUpdate();
                break;
            case AIStateType.Fleeing:
                FleeingUpdate();
                break;
        }

        if (!meshRenderer.enabled)
        {
            LookingAtPlayerUpdate();
        }
    }

    public void SetState(AIStateType state)
    {
        if (aiState == state)
            return;

        aiState = state;
        switch (aiState)
        {
            case AIStateType.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIStateType.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIStateType.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIStateType.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        //animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (!meshRenderer.enabled)
        {
            return;
        }

        if (playerDistance < detectDistance && !isHiding) // 플레이어가 감지 범위 안에 있고 숨지 않은 경우
        {
            SetState(AIStateType.Attacking);
        }
        else if ((isHiding || playerDistance > detectDistance) && aiState != AIStateType.Wandering) // 플레이어를 놓친 경우 Wandering으로 전환
        {
            SetState(AIStateType.Wandering);
        }

        if (AIStateType.Wandering == aiState && agent.remainingDistance < 0.1f && check)
        {
            check = false;
            Debug.Log(wanderingCount);
            WanderToNewLocation();  // 새 위치로 이동

            // 이동 횟수를 모두 소진하면 투명화 상태로 전환
            if (wanderingCount <= 0)
                BecomeInvisible();
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
        if ((playerDistance <= attackDistance))
        {
            agent.isStopped = true;
        }
        else
        {
            if (playerDistance < detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(playerTransform.position, path))
                {
                    agent.SetDestination(playerTransform.position);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIStateType.Wandering);
            }
            //animator.speed = 1;
            //animator.SetTrigger("Attack");
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
            SetState(AIStateType.Wandering);
        }
    }

    void ResetWanderingCount()
    {
        wanderingCount = Random.Range(6, 11);
    }

    void BecomeInvisible()
    {
        SetState(AIStateType.Idle);
        meshRenderer.enabled = false;
        StopCoroutine(coroutine);
        coroutine = null;
        ResetWanderingCount();
        check = true;
    }

    void WanderToNewLocation()
    {
        coroutine = StartCoroutine(CoTimer.Start(0.5f, () =>
        {
            agent.SetDestination(GetWanderLocation());
            wanderingCount--;
            check = true;
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

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
        {

            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - playerTransform.position, transform.position + targetPos);
    }
}
