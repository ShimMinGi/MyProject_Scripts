using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyGroundState
{
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        if (stateMachine.Target && stateMachine.Target.IsDead)
            stateMachine.Target = null;

        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        if (IsInAttackRange())
        {
            if (IsInSight())
            {
                stateMachine.TransitState(stateMachine.BaseAttackState);
            }
            else
            {
                base.Update();
            }
        }
        else if (SearchForTaget())
        {
            stateMachine.TransitState(stateMachine.ChaseState);
        }
    }


}
