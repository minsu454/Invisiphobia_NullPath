using Common.AnimationEx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    //private readonly int revealHash = Animator.StringToHash("Reveal");
    //private readonly int walkHash = Animator.StringToHash("Walk");
    //private readonly int attackHash = Animator.StringToHash("Attack");
    //private readonly int fleeHash = Animator.StringToHash("Flee");

    [SerializeField] private Animator animator;

    public void Init(Monster monster)
    {
        monster.MyState.WanderingEvent += WanderingAnim;
        monster.MyState.AttackingEvent += AttackingAnim;
        monster.MyState.MonsterFleeingEvent += MonsterFleeingAnim;
    }

    private void WanderingAnim()
    {
        AnimationExtansions.SetAnimation(animator, AnimType.Walk);
    }

    private void AttackingAnim()
    {
        AnimationExtansions.SetAnimation(animator, AnimType.Attack);
    }

    private void MonsterFleeingAnim()
    {
        AnimationExtansions.SetAnimation(animator, AnimType.Flee);
    }
}
