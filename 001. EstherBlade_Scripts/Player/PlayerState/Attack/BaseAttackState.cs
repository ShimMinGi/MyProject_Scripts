using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackState : PlayerAttackState
{
    public BaseAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(currentAttackInfo.AttackNameHash);
    }
    public override void Update()
    {
        base.Update();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (IsAttackComplete(normalizedTime)) return;

        if (ShouldApplyForce(normalizedTime)) TryApplyForce();

        if (ShouldApplyDamage(normalizedTime)) ApplyAttack();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(currentAttackInfo.AttackNameHash);
    }
}