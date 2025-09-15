using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAttackState : EnemyAttackState
{
    public EnemyBaseAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(currentAttackInfo.AttackNameHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(currentAttackInfo.AttackNameHash);
    }

    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if (IsAttackComplete(normalizedTime)) return;
        if (ShouldApplyForce(normalizedTime)) TryApplyForce();
        if (ShouldApplyDamage(normalizedTime)) ApplyAttack();
    }
}