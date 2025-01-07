using Common.Event;
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
    public ITargetable Target { get { return target; } }

    public Vector3 monsterSpawnPoint { get; private set; }

    protected Monster monster;

    public virtual void Init(Monster monster)
    {
        this.monster = monster;
        monsterSpawnPoint = transform.position;
        monster.AiState = AIStateType.Idle;
        agent.enabled = false;

        monster.MyState.AttackingEvent += AttackingUpdate;

        EventManager.Subscribe(GameEventType.UseMonsterPause, OnUseMonsterPause);
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

    public void Detacted()
    {
        agent.enabled = false;
    }

    public void Revealed()
    {
        agent.enabled = true;
        LookingAtTarget();
    }

    protected void Kill()
    {
        monster.myRenderer.enabled = true;
        transform.LookAt(target.transform);
        agent.enabled = false;
        monster.AiState = AIStateType.killing;
    }

    private void OnUseMonsterPause(object args)
    {
        if (monster.StateType != PropStateType.Revealed)
            return;

        if ((bool)args)
        {
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
    }

    private void OnDestroy()
    {
        EventManager.Subscribe(GameEventType.UseMonsterPause, OnUseMonsterPause);
    }
}
