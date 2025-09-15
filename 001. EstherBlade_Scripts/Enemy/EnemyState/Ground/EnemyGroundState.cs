using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundState : EnemyBaseState
{
    public EnemyGroundState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Enemy.AnimationData.GroundedParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.AnimationData.GroundedParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!stateMachine.Enemy.Controller.isGrounded
            && stateMachine.Enemy.Controller.velocity.y < Physics.gravity.y * 0.5f)
        {
            stateMachine.TransitState(stateMachine.FallState);
            return;
        }
    }
}