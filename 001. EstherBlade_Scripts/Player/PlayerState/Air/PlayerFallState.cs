using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = stateMachine.Player.Data.AirData.FallSpeedModifier;

        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.Player.Controller.isGrounded)
        {
            stateMachine.TransitState(stateMachine.IdleState);
            return;
        }
    }
}
