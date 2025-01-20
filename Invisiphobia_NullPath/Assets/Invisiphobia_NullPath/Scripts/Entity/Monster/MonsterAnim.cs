using Common.AnimationEx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    //private readonly int idleHash = Animator.StringToHash("Idle");
    private readonly int revealHash = Animator.StringToHash("Reveal");
    private readonly int walkHash = Animator.StringToHash("IsWalking");
    private readonly int attackHash = Animator.StringToHash("IsAttacking");
    private readonly int fleeHash = Animator.StringToHash("IsFleeing");
    private readonly int killHash = Animator.StringToHash("IsDead");
    private readonly int unrevealKillHash = Animator.StringToHash("UnrevealKill");

    [SerializeField] private Animator animator;
    private IDetectable monster;

    public void Init(Monster monster)
    {
        //monster.MyState.IdleEvent += IdleAnim;
        monster.MyState.RevealEvent += RevealAnim;
        monster.MyState.WanderingEvent += WanderingAnim;
        monster.MyState.AttackingEvent += AttackingAnim;
        monster.MyState.MonsterFleeingEvent += MonsterFleeingAnim;
        monster.MyState.MonsterKillingEvent += MonsterKillingAnim;

        this.monster = monster;
    }

    private void ResetAllStates()
    {
        //animator.SetBool(idleHash, false);
        animator.SetBool(revealHash, false);
        animator.SetBool(walkHash, false);
        animator.SetBool(attackHash, false);
        animator.SetBool(fleeHash, false);
        animator.SetBool(killHash, false);
    }

    private void IdleAnim()
    {
        ResetAllStates();
        //animator.SetBool(idleHash, true);
    }

    private void RevealAnim()
    {
        ResetAllStates();
        animator.SetBool(revealHash, true);
    }

    private void WanderingAnim()
    {
        ResetAllStates();
        animator.SetBool(walkHash, true);
    }

    private void AttackingAnim()
    {
        ResetAllStates();
        animator.SetBool(attackHash, true);
    }

    private void MonsterFleeingAnim()
    {
        ResetAllStates();
        animator.SetBool(fleeHash, true);
    }

    private void MonsterKillingAnim()
    {
        ResetAllStates();
        if (monster.StateType != PropStateType.Revealed)
        {
            animator.SetBool(unrevealKillHash, true);
        }
        else
        {
            animator.SetBool(killHash, true);
        }
    }
}
