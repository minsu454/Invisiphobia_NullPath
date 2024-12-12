using UnityEngine.AI;

public class BossMonsterController : MonsterController
{
    public override void Init(Monster monster)
    {
        base.Init(monster);
        SetTarget(Player.Instance.transform);
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
}