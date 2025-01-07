using Common.AnimationEx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    private readonly int revealHash = Animator.StringToHash("Reveal");
    private readonly int walkHash = Animator.StringToHash("IsWalking");
    private readonly int attackHash = Animator.StringToHash("IsAttacking");
    private readonly int fleeHash = Animator.StringToHash("IsFleeing");
    private readonly int killHash = Animator.StringToHash("IsDead");

    [SerializeField] private Animator animator;

    public void Init(Monster monster)
    {
        monster.MyState.WanderingEvent += WanderingAnim;
        monster.MyState.AttackingEvent += AttackingAnim;
        monster.MyState.MonsterFleeingEvent += MonsterFleeingAnim;
        monster.MyState.MonsterKillingEvent += MonsterKillingAnim;
    }

    private void ResetAllStates()
    {
        animator.SetBool(walkHash, false);
        animator.SetBool(attackHash, false);
        animator.SetBool(fleeHash, false);
        animator.SetBool(killHash, false);
    }

    private void WanderingAnim()
    {
        //AnimationExtansions.SetAnimation(animator, AnimType.Walk);
        ResetAllStates();
        animator.SetBool(walkHash, true);
    }

    private void AttackingAnim()
    {
        //AnimationExtansions.SetAnimation(animator, AnimType.Attack);
        ResetAllStates();
        animator.SetBool(attackHash, true);
    }

    private void MonsterFleeingAnim()
    {
        //AnimationExtansions.SetAnimation(animator, AnimType.Flee);
        ResetAllStates();
        animator.SetBool(fleeHash, true);
    }

    private void MonsterKillingAnim()
    {
        //AnimationExtansions.SetAnimation(animator, AnimType.Kill);
        ResetAllStates();
        animator.SetBool(killHash, true);
    }
}
