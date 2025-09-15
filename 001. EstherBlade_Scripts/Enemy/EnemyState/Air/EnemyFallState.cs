using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallState : EnemyAirState
{
    public EnemyFallState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = stateMachine.Enemy.Data.AirData.FallSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.FallParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.FallParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.Enemy.Controller.isGrounded)
        {
            stateMachine.TransitState(stateMachine.IdleState);
        }
    }
}
