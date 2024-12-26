using UnityEngine;
using UnityEngine.AI;

public class CowardMonster : MonsterController
{
    [SerializeField] private Transform targetPosition;

    public float fadeDuration = 2f;
    public float fleeDuration = 4f;

    private Color originalColor;
    private float fleeStartTime;

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

    // SetTargetDestination 사용
    // 감지된 후 이동하다가 플레이어의 시야에서 1초동안 감지되지 않으면 그 자리에 멈춰서 다시 투명화 - 감지될 수 있고 감지되면 다시 위치로 이어서 이동
    // fade는 마지막에 위치에 도착하면 멈춰서 fade
    /// <summary>
    /// 도망 조건을 만족하며 도망 위치로 이동하는 함수
    /// </summary>
    void MonsterFleeingUpdate()
    {
        float elapsedTime = Time.time - fleeStartTime;

        if (elapsedTime >= fleeDuration)
        {
            if (agent != null)
            {
                agent.ResetPath();
            }
            gameObject.SetActive(false);
            monster.myRenderer.material.color = originalColor;
        }
        else if (elapsedTime >= fleeDuration - fadeDuration)
        {
            float fadeTime = elapsedTime - (fleeDuration - fadeDuration);
            float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeDuration);
            if (monster.myRenderer.material != null)
            {
                Color fadedColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                monster.myRenderer.material.color = fadedColor;
            }
        }
    }

    private void SetMaxDistanceDestination()
    {
        float maxDistance = runSpeed * fleeDuration;

        Vector3 randomDirection = Random.insideUnitSphere * maxDistance;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    /// <summary>
    /// 지정해준 포지션으로 이동하는 함수
    /// </summary>
    void SetTargetDestination() // 이거로 사용
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
        fleeStartTime = Time.time;

        SetMaxDistanceDestination();
    }

    protected override void LookingAtTarget()
    {
    }
}
