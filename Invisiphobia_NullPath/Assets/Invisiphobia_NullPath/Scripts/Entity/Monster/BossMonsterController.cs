using MimicSpace;
using UnityEngine.AI;

public class BossMonsterController : MonsterController
{
    private Mimic myMimic;

    public override void Init(Monster monster)
    {
        base.Init(monster);
<<<<<<< Updated upstream
        SetTarget(EntityManager.Instance.Player.transform);
=======
        //myMimic = GetComponent<Mimic>();
        SetTarget(Player.Instance.transform);
>>>>>>> Stashed changes

        agent.speed = runSpeed;

        var mimic = GetComponent<Mimic>();
        if (mimic != null)
        {
            mimic.velocity = agent.velocity;
        }

        monster.MyState.IdleEvent += LookingAtPlayerUpdate;
        monster.MyState.WanderingEvent += OnAttackingUpdate;
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
            myMimic.velocity = agent.desiredVelocity;
        }
    }

    void OnAttackingUpdate()
    {
        monster.aiState = AIStateType.Attacking;
    }
}