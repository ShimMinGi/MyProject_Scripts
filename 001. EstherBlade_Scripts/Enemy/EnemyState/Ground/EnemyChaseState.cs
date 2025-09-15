using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyGroundState
{
    public EnemyChaseState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (IsInAttackRange())
        {
            if (IsInSight())
            {
                stateMachine.TransitState(stateMachine.BaseAttackState);
                return;
            }
        }

        if (!stateMachine.Target)
        {
            if (!SearchForTaget())
            {
                stateMachine.TransitState(stateMachine.IdleState);
                return;
            }
        }
        else
        {
            float distance = Vector3.Distance(stateMachine.Target.transform.position, stateMachine.Enemy.transform.position);
            if (distance >= stateMachine.Enemy.Data.SearchingDistance * 1.2f)
                stateMachine.TransitState(stateMachine.IdleState);
        }
    }

}
