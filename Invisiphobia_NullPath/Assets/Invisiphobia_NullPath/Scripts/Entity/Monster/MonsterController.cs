using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,           // 투명화로 멈춰있는 상태
    Wandering,      // 잠시 어슬렁거리는 상태
    Attacking,      // 플레이어를 쫓고 공격하는 상태
    Fleeing,        // 플레이어가 도망쳤을 때의 상태
    //LookingAtPlayer // 플레이어를 바라보는 상태
}

public class MonsterController : MonoBehaviour
{
    [Header("Stats")]
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    public float detectDistance;
    public float safeDistance;
    public float lookAtPlayerDistance;
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    public float wanderingTime = 10f;

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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        //meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        SetState(AIState.Wandering);
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, playerTransform.position);

        //animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Fleeing:
                FleeingUpdate();
                break;
            //case AIState.LookingAtPlayer:
            //    LookingAtPlayerUpdate();
            //    break;
        }
    }

    public void SetState(AIState state)
    {
        aiState = state;
        Debug.Log(state);
        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                Debug.Log("wandering");
                agent.speed = walkSpeed;
                agent.isStopped = false;
                WanderToNewLocation();
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            //case AIState.LookingAtPlayer:
            //    agent.isStopped = true;
            //    break;
        }

        //animator.speed = agent.speed / walkSpeed;
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (playerDistance < detectDistance)    // 플레이어 감지하고 공격 상태
        {
            SetState(AIState.Attacking);
        }
        else if (aiState != AIState.Wandering)  // 플레이어가 도망갔을 때
        {
            SetState(AIState.Wandering);
            Invoke("BecomeInvisible", Random.Range(minWanderWaitTime, maxWanderWaitTime)); // 떠도는 후 투명화
        }

        //if (playerDistance < lookAtPlayerDistance)     // 플레이어를 감지하고 바라보도록
        //{
        //    SetState(AIState.LookingAtPlayer);
        //}
    }

    private void LookingAtPlayerUpdate()
    {
        if (playerDistance > lookAtPlayerDistance)
        {
            SetState(AIState.Wandering);
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
                Debug.Log("Attack");
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
                SetState(AIState.Wandering);
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
            SetState(AIState.Wandering);
        }
    }

    void BecomeInvisible()
    {
        if (aiState == AIState.Wandering) // Wandering 상태에서만 실행
        {
            SetState(AIState.Idle);
            if (monster != null)
            {
                monster.BecomeInvisible();
            }
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle)
        {
            return;
        }
        if (!meshRenderer.enabled)
        {
            return;
        }
        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
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
