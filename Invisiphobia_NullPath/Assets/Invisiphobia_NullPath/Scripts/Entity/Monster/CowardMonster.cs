using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowardMonster : MonsterController
{
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
        SetTarget(Player.Instance.transform);
        monster.MyState.WanderingEvent += OnWanderingUpdate;
        monster.MyState.MonsterFleeingEvent += MonsterFleeingUpdate;

        if (monster.myRenderer != null)
        {
            originalColor = monster.myRenderer.material.color;
        }
    }

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

    void OnWanderingUpdate()
    {
        monster.aiState = AIStateType.MonsterFleeing;
        fleeStartTime = Time.time;

        SetMaxDistanceDestination();
    }
}
