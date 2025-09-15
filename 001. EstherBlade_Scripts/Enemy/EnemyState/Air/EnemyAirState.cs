using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirState : EnemyBaseState
{
    public EnemyAirState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.AirParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.AirParameterHash);
    }
}