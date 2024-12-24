using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    [Header("AI")]
    [SerializeField] protected float lookAtPlayerDistance;

    protected float targetDistance;
    protected bool isAlwaysAttacking; // boss만 true

    [Header("NavMeshAgent")]
    [SerializeField] protected NavMeshAgent agent;
    protected ITargetable target;

    public Vector3 monsterSpawnPoint { get; private set; }

    protected Monster monster;

    public virtual void Init(Monster monster)
    {
        this.monster = monster;
        monsterSpawnPoint = transform.position;
        monster.AiState = AIStateType.Idle;
        agent.enabled = false;

        monster.MyState.AttackingEvent += AttackingUpdate;
    }

    void Update()
    {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
    }

    protected abstract void AttackingUpdate();
    public abstract void PlayerAttackMonster();
    protected abstract void LookingAtTarget();

    public void SetTarget(ITargetable target)
    {
        this.target = target;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - target.transform.position, transform.position + targetPos);
    }

    public void Detacted()
    {
        agent.enabled = false;
    }

    public void Revealed()
    {
        agent.enabled = true;
        LookingAtTarget();
    }
}
