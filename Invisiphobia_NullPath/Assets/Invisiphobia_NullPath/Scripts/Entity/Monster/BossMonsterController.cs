using Common.Event;
using MimicSpace;
using UnityEngine;
using UnityEngine.AI;

public class BossMonsterController : MonsterController
{
    private Mimic myMimic;

    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(EntityManager.Instance.Player.transform);

        agent.speed = runSpeed;

        var mimic = GetComponent<Mimic>();

        monster.MyState.IdleEvent += LookingAtPlayerUpdate;
        monster.MyState.WanderingEvent += OnAttackingUpdate;

        monster.aiState = AIStateType.Wandering;

        gameObject.SetActive(false);
    }

    public override void PlayerAttackMonster()
    {
    }

    protected override void AttackingUpdate()
    {
        NavMeshPath path = new NavMeshPath();

        if (agent.CalculatePath(targetTransform.position, path))
        {
            agent.SetDestination(targetTransform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Dispatch(GameEventType.GameOver, null);
        }
    }

    void OnAttackingUpdate()
    {
        monster.aiState = AIStateType.Attacking;
    }
}