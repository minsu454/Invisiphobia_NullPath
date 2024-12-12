using Common.Timer;
using Common.Yield;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class MonsterController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    [Header("AI")]
    [SerializeField] protected float detectDistance;
    [SerializeField] protected float lookAtPlayerDistance;

    protected float targetDistance;
    protected bool isAlwaysAttacking; // boss만 true

    [Header("NavMeshAgent")]
    [SerializeField] protected NavMeshAgent agent;
    protected Transform targetTransform;

    public Vector3 monsterSpawnPoint { get; private set; }

    protected Monster monster;

    public virtual void Init(Monster monster)
    {
        this.monster = monster;
        monsterSpawnPoint = transform.position;
        monster.aiState = AIStateType.Idle;
        
        monster.MyState.AttackingEvent += AttackingUpdate;
    }

    void Update()
    {
        targetDistance = Vector3.Distance(transform.position, targetTransform.position);
    }

    protected abstract void AttackingUpdate();
    public abstract void PlayerAttackMonster();

    public void SetTarget(Transform transform)
    {
        targetTransform = transform;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - targetTransform.position, transform.position + targetPos);
    }
}
