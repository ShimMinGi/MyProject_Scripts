using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackState : PlayerAttackState
{
    private bool alreadyApplyCombo;

    public ComboAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        alreadyApplyCombo = false;
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

        if (ShouldApplyCombo(normalizedTime)) TransitionToNext();

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(currentAttackInfo.AttackNameHash);
    }

    private bool ShouldApplyCombo(float normalizedTime)
    {
        if (alreadyApplyCombo) return false;
        if (currentAttackInfo.ComboStateIndex <= -1) return false;
        if (normalizedTime < currentAttackInfo.ComboTransitionTime) return false;
        if (!stateMachine.IsAttacking) return false;

        return true;
    }

    private void TransitionToNext()
    {
        alreadyAppliedForce = true;
        AttackInfoData attackInfo = attackData.GetAttackInfo(currentAttackInfo.ComboStateIndex);
        ChangeAttackState(attackInfo);
    }
}