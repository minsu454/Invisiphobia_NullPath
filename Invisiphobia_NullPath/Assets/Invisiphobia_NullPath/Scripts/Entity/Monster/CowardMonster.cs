using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class CowardMonster : MonsterController
{
    [SerializeField] private Transform targetPosition;

    public float fadeDuration = 2f;
    public float timeOutOfSight = 0f;

    private Color originalColor;

    public override void PlayerAttackMonster()
    {
    }

    protected override void AttackingUpdate()
    {
    }

    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(EntityManager.Instance.Player);
        monster.MyState.WanderingEvent += OnWanderingUpdate;
        monster.MyState.MonsterFleeingEvent += MonsterFleeingUpdate;
        monster.myRenderer.enabled = false;

        if (monster.myRenderer != null)
        {
            originalColor = monster.myRenderer.material.color;
        }
    }

    /// <summary>
    /// 도망 조건을 만족하며 도망 위치로 이동하는 함수
    /// </summary>
    void MonsterFleeingUpdate()
    {
        // 도착 지점 마지막에 fade 효과
        float remainingDistance = Vector3.Distance(monster.transform.position, agent.destination);
        if (remainingDistance <= 2f)
        {
            StartFadeEffect(remainingDistance);
        }
        else if (remainingDistance <= 0f)
        {
            // 도달하면 더 이상 바로 사라짐
            monster.myRenderer.enabled = false;
        }
    }

    /// <summary>
    /// 남은 거리에 따라 알파값 조정
    /// </summary>
    void StartFadeEffect(float remainingDistance)
    {
        if (monster.myRenderer != null)
        {
            // 남은 거리가 1에서 0으로 줄어드는 동안 알파값을 1에서 0으로 변경
            float fadeAlpha = Mathf.Lerp(1f, 0f, (1f - remainingDistance) / 1f);
            Color monsterColor = originalColor;
            monsterColor.a = fadeAlpha;
            monster.myRenderer.material.color = monsterColor;

            if (fadeAlpha <= 0.6f)  // 0.6 임시
            {
                monster.myRenderer.enabled = false;
                monster.myRenderer.material.color = originalColor;
                ResetToSpawnPoint();
            }
        }
    }

    /// <summary>
    /// 지정해준 포지션으로 이동하는 함수
    /// </summary>
    void SetTargetDestination()
    {
        float maxPosition = 1.0f;

        if (targetPosition != null)
        {
            if (NavMesh.SamplePosition(targetPosition.position, out NavMeshHit hit, maxPosition, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void OnWanderingUpdate()
    {
        monster.AiState = AIStateType.MonsterFleeing;

        SetTargetDestination();
    }

    void ResetCycle()
    {
        monster.AiState = AIStateType.Idle;
        monster.ResetCycle();
    }

    protected void ResetToSpawnPoint()
    {
        agent.Warp(monsterSpawnPoint);
        ResetCycle();
    }

    protected override void LookingAtTarget()
    {
    }
}
